// UTF-8 Enforcer: 足の不自由なハッキング
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
        /// User-Agent to be used with the
        /// </summary>
        public String UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.87 Safari/537.36";

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
        /// <param name="Referer">Common requests' referer</param>
        public Fetch ( String Referer )
        {
            this.Referer = Referer;
        }

        /// <summary>
        /// Checks for the existence of a web client
        /// </summary>
        private void CheckWC ( )
        {
            // Creates the webclient if there isn't any
            if ( _wc == null )
                _wc = new WebClient ( );

            try
            {
                _wc.DownloadProgressChanged -= _wc_DownloadProgressChanged;
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch { }
#pragma warning restore CC0004 // Catch block cannot be empty

            _wc.DownloadProgressChanged += _wc_DownloadProgressChanged;

            _wc.Headers.Set ( HttpRequestHeader.UserAgent, UserAgent );
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
            CheckWC ( );
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
            CheckWC ( );
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
            CheckWC ( );
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
            CheckWC ( );
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
            _wc = null;
            GC.SuppressFinalize ( this );
        }

        #endregion IDisposable Support
    }
}
