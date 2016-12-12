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
using Kemori.Abstractions;

namespace Kemori.Base
{
    public class MangaChapter : IMangaChapter
    {
        /// <summary>
        /// Chapter number (1, 1.5, etc.)
        /// Use manga chapter array index if not existent.
        /// </summary>
        public String Chapter { get; set; }

        /// <summary>
        /// Wether this <see cref="MangaChapter"/> was already downloaded
        /// (does not check if it was actually sucessfuly downloaded)
        /// </summary>
        public Boolean IsDownloaded
        {
            get
            {
                var dirName = Manga?.Connector?.IO?.PathForChapter ( this );
                return dirName != null && ( System.IO.File.Exists ( dirName + ".cbz" ) || System.IO.Directory.Exists ( dirName ) );
            }
        }

        private String _hash;

        /// <summary>
        /// ID of the instance (used for unique identifying)
        /// </summary>
        public String InstanceID
        {
            get
            {
                if ( _hash == null )
                {
                    ReCalcInstanceID ( );
                }

                return _hash;
            }
        }

        /// <summary>
        /// Link to this chapter (for page getting)
        /// </summary>
        public String Link { get; set; }

        /// <summary>
        /// The manga this chapter belongs to
        /// </summary>
        public Manga Manga { get; set; }

        /// <summary>
        /// Name of the chapter (if existent)
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Links to the page images
        /// </summary>
        public String[] PageLinks { get; private set; }

        /// <summary>
        /// Number of pages
        /// </summary>
        public Int32 Pages { get; private set; }

        /// <summary>
        /// Volume of the manga
        /// </summary>
        public String Volume { get; set; }

        /// <summary>
        /// Loads number of pages and page links
        /// </summary>
        public async void Load ( )
        {
            this.PageLinks = await Manga.Connector.GetPageLinksAsync ( this );
            this.Pages = this.PageLinks.Length;
        }

        /// <summary>
        /// Recalculates the instance ID
        /// </summary>
        public void ReCalcInstanceID ( )
        {
            _hash = $"{Manga.InstanceID}{this.Name}{this.Link}"
                .GetHashCode ( )
                .ToString ( );
        }

        /// <summary>
        /// Returns the <see cref="MangaChapter"/> <see cref="String"/> representation
        /// </summary>
        /// <returns></returns>
        public override String ToString ( )
        {
            return $"[{Volume}] - {Chapter} - {Name}";
        }
    }
}