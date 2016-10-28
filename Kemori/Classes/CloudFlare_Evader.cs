﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

// Credit goes to http://stackoverflow.com/a/32426051/2671392 (user https://stackoverflow.com/users/5296568/maximilian-gerhardt)
namespace Kemori.Classes
{
    public class CloudflareEvader
    {
        /// <summary>
        /// User agent to use on requests
        /// </summary>
        public static String UA = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:40.0) Gecko/20100101 Firefox/40.0";

        /// <summary>
        /// Tries to return a webclient with the neccessary cookies installed
        /// to do requests for a cloudflare protected website.
        /// </summary>
        /// <param name="url">The page which is behind cloudflare's anti-dDoS protection</param>
        /// <param name="reff">Refferrer URL to use on the request</param>
        /// <returns>A WebClient object or null on failure</returns>
        public static WebClient CreateBypassedWebClient ( String url, String reff = null )
        {
            var JSEngine = new Jint.Engine ( ); //Use this JavaScript engine to compute the result.

            //Download the original page
            var uri = new Uri ( url );
            var req = ( HttpWebRequest ) WebRequest.Create ( url );

            if ( reff != null )
                req.Referer = reff;
            req.UserAgent = UA;

            //Try to make the usual request first. If this fails with a 503, the page is behind cloudflare.
            try
            {
                var res = req.GetResponse ( );
                var html = "";
                using ( var reader = new StreamReader ( res.GetResponseStream ( ) ) )
                    html = reader.ReadToEnd ( );
                return new WebClient ( );
            }
            catch ( WebException ex ) //We usually get this because of a 503 service not available.
            {
                var html = "";
                using ( var reader = new StreamReader ( ex.Response.GetResponseStream ( ) ) )
                    html = reader.ReadToEnd ( );
                //If we get on the landing page, Cloudflare gives us a User-ID token with the cookie. We need to save that and use it in the next request.
                var cookie_container = new CookieContainer ( );
                //using a custom function because ex.Response.Cookies returns an empty set ALTHOUGH cookies were sent back.
                var initial_cookies = GetAllCookiesFromHeader ( ex.Response.Headers["Set-Cookie"], uri.Host );
                foreach ( Cookie init_cookie in initial_cookies )
                    cookie_container.Add ( init_cookie );

                /* solve the actual challenge with a bunch of RegEx's. Copy-Pasted from the python scrapper version.*/
                var challenge = Regex.Match ( html, "name=\"jschl_vc\" value=\"(\\w+)\"" ).Groups[1].Value;
                var challenge_pass = Regex.Match ( html, "name=\"pass\" value=\"(.+?)\"" ).Groups[1].Value;

                var builder = Regex.Match ( html, @"setTimeout\(function\(\){\s+(var t,r,a,f.+?\r?\n[\s\S]+?a\.value =.+?)\r?\n" ).Groups[1].Value;
                builder = Regex.Replace ( builder, @"a\.value =(.+?) \+ .+?;", "$1" );
                builder = Regex.Replace ( builder, @"\s{3,}[a-z](?: = |\.).+", "" );

                //Format the javascript..
                builder = Regex.Replace ( builder, @"[\n\\']", "" );

                //Execute it.
                var solved = long.Parse ( JSEngine.Execute ( builder ).GetCompletionValue ( ).ToObject ( ).ToString ( ) );
                solved += uri.Host.Length; //add the length of the domain to it.

                //Console.WriteLine ( "***** SOLVED CHALLENGE ******: " + solved );
                Thread.Sleep ( 3000 ); //This sleeping IS requiered or cloudflare will not give you the token!!

                //Retreive the cookies. Prepare the URL for cookie exfiltration.
                var cookie_url = string.Format ( "{0}://{1}/cdn-cgi/l/chk_jschl", uri.Scheme, uri.Host );
                var uri_builder = new UriBuilder ( cookie_url );
                var query = HttpUtility.ParseQueryString ( uri_builder.Query );
                //Add our answers to the GET query
                query["jschl_vc"] = challenge;
                query["jschl_answer"] = solved.ToString ( );
                query["pass"] = challenge_pass;
                uri_builder.Query = query.ToString ( );

                //Create the actual request to get the security clearance cookie
                var cookie_req = ( HttpWebRequest ) WebRequest.Create ( uri_builder.Uri );
                cookie_req.AllowAutoRedirect = false;
                cookie_req.CookieContainer = cookie_container;
                cookie_req.Referer = url;
                cookie_req.UserAgent = UA;
                //We assume that this request goes through well, so no try-catch
                var cookie_resp = ( HttpWebResponse ) cookie_req.GetResponse ( );
                //The response *should* contain the security clearance cookie!
                if ( cookie_resp.Cookies.Count != 0 ) //first check if the HttpWebResponse has picked up the cookie.
                    foreach ( Cookie cookie in cookie_resp.Cookies )
                        cookie_container.Add ( cookie );
                else //otherwise, use the custom function again
                {
                    //the cookie we *hopefully* received here is the cloudflare security clearance token.
                    if ( cookie_resp.Headers["Set-Cookie"] != null )
                    {
                        var cookies_parsed = GetAllCookiesFromHeader ( cookie_resp.Headers["Set-Cookie"], uri.Host );
                        foreach ( Cookie cookie in cookies_parsed )
                            cookie_container.Add ( cookie );
                    }
                    else
                    {
                        //No security clearence? something went wrong.. return null.
                        //Console.WriteLine("MASSIVE ERROR: COULDN'T GET CLOUDFLARE CLEARANCE!");
                        return null;
                    }
                }
                //Create a custom webclient with the two cookies we already acquired.
                WebClient modedWebClient = new WebClientEx ( cookie_container );
                modedWebClient.Headers.Add ( "User-Agent", UA );
                if ( reff != null )
                    modedWebClient.Headers.Add ( "Referer", reff );
                return modedWebClient;
            }
        }

        /* Credit goes to https://stackoverflow.com/questions/15103513/httpwebresponse-cookies-empty-despite-set-cookie-header-no-redirect
           (user https://stackoverflow.com/users/541404/cameron-tinker) for these functions
        */

        public static CookieCollection GetAllCookiesFromHeader ( string strHeader, string strHost )
        {
            var al = new ArrayList ( );
            var cc = new CookieCollection ( );
            if ( strHeader != string.Empty )
            {
                al = ConvertCookieHeaderToArrayList ( strHeader );
                cc = ConvertCookieArraysToCookieCollection ( al, strHost );
            }
            return cc;
        }

        private static ArrayList ConvertCookieHeaderToArrayList ( string strCookHeader )
        {
            strCookHeader = strCookHeader.Replace ( "\r", "" );
            strCookHeader = strCookHeader.Replace ( "\n", "" );
            var strCookTemp = strCookHeader.Split ( ',' );
            var al = new ArrayList ( );
            var i = 0;
            var n = strCookTemp.Length;
            while ( i < n )
            {
                if ( strCookTemp[i].IndexOf ( "expires=", StringComparison.OrdinalIgnoreCase ) > 0 )
                {
                    al.Add ( strCookTemp[i] + "," + strCookTemp[i + 1] );
                    i = i + 1;
                }
                else
                    al.Add ( strCookTemp[i] );
                i = i + 1;
            }
            return al;
        }

        private static CookieCollection ConvertCookieArraysToCookieCollection ( ArrayList al, string strHost )
        {
            var cc = new CookieCollection ( );

            var alcount = al.Count;
            string strEachCook;
            string[] strEachCookParts;
            for ( int i = 0 ; i < alcount ; i++ )
            {
                strEachCook = al[i].ToString ( );
                strEachCookParts = strEachCook.Split ( ';' );
                var intEachCookPartsCount = strEachCookParts.Length;
                var strCNameAndCValue = string.Empty;
                var strPNameAndPValue = string.Empty;
                var strDNameAndDValue = string.Empty;
                string[] NameValuePairTemp;
                var cookTemp = new Cookie ( );

                for ( int j = 0 ; j < intEachCookPartsCount ; j++ )
                {
                    if ( j == 0 )
                    {
                        strCNameAndCValue = strEachCookParts[j];
                        if ( strCNameAndCValue != string.Empty )
                        {
                            var firstEqual = strCNameAndCValue.IndexOf ( "=" );
                            var firstName = strCNameAndCValue.Substring ( 0, firstEqual );
                            var allValue = strCNameAndCValue.Substring ( firstEqual + 1, strCNameAndCValue.Length - ( firstEqual + 1 ) );
                            cookTemp.Name = firstName;
                            cookTemp.Value = allValue;
                        }
                        continue;
                    }
                    if ( strEachCookParts[j].IndexOf ( "path", StringComparison.OrdinalIgnoreCase ) >= 0 )
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if ( strPNameAndPValue != string.Empty )
                        {
                            NameValuePairTemp = strPNameAndPValue.Split ( '=' );
                            if ( NameValuePairTemp[1] != string.Empty )
                                cookTemp.Path = NameValuePairTemp[1];
                            else
                                cookTemp.Path = "/";
                        }
                        continue;
                    }

                    if ( strEachCookParts[j].IndexOf ( "domain", StringComparison.OrdinalIgnoreCase ) >= 0 )
                    {
                        strPNameAndPValue = strEachCookParts[j];
                        if ( strPNameAndPValue != string.Empty )
                        {
                            NameValuePairTemp = strPNameAndPValue.Split ( '=' );

                            if ( NameValuePairTemp[1] != string.Empty )
                                cookTemp.Domain = NameValuePairTemp[1];
                            else
                                cookTemp.Domain = strHost;
                        }
                        continue;
                    }
                }

                if ( cookTemp.Path == string.Empty )
                    cookTemp.Path = "/";
                if ( cookTemp.Domain == string.Empty )
                    cookTemp.Domain = strHost;
                cc.Add ( cookTemp );
            }
            return cc;
        }
    }

    /*Credit goes to  https://stackoverflow.com/questions/1777221/using-cookiecontainer-with-webclient-class
 (user https://stackoverflow.com/users/129124/pavel-savara) */

    public class WebClientEx : WebClient
    {
        public WebClientEx ( CookieContainer container )
        {
            this.container = container;
        }

        public CookieContainer CookieContainer
        {
            get { return container; }
            set { container = value; }
        }

        private CookieContainer container = new CookieContainer();

        protected override WebRequest GetWebRequest ( Uri address )
        {
            var r = base.GetWebRequest ( address );
            var request = r as HttpWebRequest;
            if ( request != null )
            {
                request.CookieContainer = container;
            }
            return r;
        }

        protected override WebResponse GetWebResponse ( WebRequest request, IAsyncResult result )
        {
            var response = base.GetWebResponse ( request, result );
            ReadCookies ( response );
            return response;
        }

        protected override WebResponse GetWebResponse ( WebRequest request )
        {
            var response = base.GetWebResponse ( request );
            ReadCookies ( response );
            return response;
        }

        private void ReadCookies ( WebResponse r )
        {
            var response = r as HttpWebResponse;
            if ( response != null )
            {
                var cookies = response.Cookies;
                container.Add ( cookies );
            }
        }
    }
}