/*
 * Kemori - An open source and community friendly manga downloader
 * Copyright (C) 2016  GGG KILLER
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Net;
using System.Threading.Tasks;
using Kemori.Web;

namespace Kemori.Classes
{
    /// <summary>
    /// HTTP Utility class
    /// </summary>
    public class Fetch : IDisposable
    {
        /// <summary>
        /// WebClient
        /// </summary>
        private WebClient _wc;

        /// <summary>
        /// Request Referer
        /// </summary>
        private String Referer;

        /// <summary>
        /// Called when the download progress is changed
        /// </summary>
        public event DownloadProgressChangedEventHandler DownloadProgressChanged;

        /// <summary>
        /// HTTP Utility class
        /// </summary>
        public Fetch ( ) { }

        /// <summary>
        /// HTTP Utility Class
        /// </summary>
        /// <param name="URL">Main website</param>
        public Fetch ( String URL )
        {
            CheckWC ( URL );
        }

        /// <summary>
        /// HTTP Utility Class
        /// </summary>
        /// <param name="URL">Main website</param>
        /// <param name="Referer">Common requests' referer</param>
        public Fetch ( String URL, String Referer ) : this ( URL )
        {
            this.Referer = Referer;
        }

        /// <summary>
        /// Checks for the existence of a web client
        /// </summary>
        /// <param name="URL">The URL we're attempting to connect to</param>
        private void CheckWC ( String URL )
        {
            // Creates the webclient if there isn't any
            //if ( _wc == null )
            //    _wc = CloudflareEvader.CreateBypassedWebClient ( URL, Referer );
            if ( _wc == null )
                _wc = new WebClient ( );

            _wc.DownloadProgressChanged += _wc_DownloadProgressChanged;

            // Set the referer if there's one
            if ( Referer != null )
                _wc.Headers.Set ( nameof ( Referer ), Referer );
        }

        /// <summary>
        /// Pipes the events to our handlers
        /// </summary>
        /// <param name="sender">Sender of the event</param>
        /// <param name="e">The arguments of the event</param>
        private void _wc_DownloadProgressChanged ( Object sender, DownloadProgressChangedEventArgs e )
        {
            DownloadProgressChanged?.Invoke ( sender, e );
        }

        /// <summary>
        /// Returns the URL contents as a string
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <param name="Referer">Referer to use when connecting</param>
        /// <returns>The content</returns>
        public String GetString ( String URL, String Referer = null )
        {
            this.Referer = Referer ?? this.Referer;
            CheckWC ( URL );
            return _wc.DownloadString ( URL );
        }

        /// <summary>
        /// Returns the URL contents as a string asynchronously
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <param name="Referer">Referer to use when connecting</param>
        /// <returns>The content</returns>
        public async Task<String> GetStringAsync ( String URL, String Referer = null )
        {
            this.Referer = Referer ?? this.Referer;
            CheckWC ( URL );
            return await _wc.DownloadStringTaskAsync ( URL );
        }

        /// <summary>
        /// Returns the URL contents as a byte array
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <param name="Referer">Referer to use when connecting</param>
        /// <returns>The content</returns>
        public Byte[] GetData ( String URL, String Referer = null )
        {
            this.Referer = Referer ?? this.Referer;
            CheckWC ( URL );
            return _wc.DownloadData ( URL );
        }

        /// <summary>
        /// Returns the URL content as a byte array asynchronously
        /// </summary>
        /// <param name="URL">The URL</param>
        /// <param name="Referer">Referer to use when connecting</param>
        /// <returns>The content</returns>
        public async Task<Byte[]> GetDataAsync ( String URL, String Referer = null )
        {
            this.Referer = Referer ?? this.Referer;
            CheckWC ( URL );
            return await _wc.DownloadDataTaskAsync ( URL );
        }

        #region IDisposable Support

        ~Fetch ( )
        {
            Dispose ( );
        }

        public void Dispose ( )
        {
            _wc?.Dispose ( );
            GC.SuppressFinalize ( this );
        }

        #endregion IDisposable Support
    }
}