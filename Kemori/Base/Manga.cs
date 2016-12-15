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
using System.Linq;
using System.Threading.Tasks;
using Kemori.Abstractions;

namespace Kemori.Base
{
    [Serializable]
    public class Manga : IManga
    {
        /// <summary>
        /// The chapters available for this manga
        /// </summary>
        public MangaChapter[] Chapters { get; set; }

        [NonSerialized]
        private MangaConnector _connector;

        /// <summary>
        /// The connector associated with this manga
        /// </summary>
        public MangaConnector Connector
        {
            get
            {
                return _connector;
            }
            set
            {
                if ( _connector == null )
                    _connector = value;
                else
                    throw new InvalidOperationException ( "Connector is a one-time set property!" );
            }
        }

        private String _hash;

        /// <summary>
        /// The hash for this manga
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
        /// Links to the manga(for chapter retrieving)
        /// </summary>
        public String Link { get; set; }

        /// <summary>
        /// Name of the manga
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Download path
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// Loads the information for this manga(chapters)
        /// </summary>
        public async Task LoadAsync ( )
        {
            this.Chapters = ( await Connector.GetChaptersAsync ( this ) ).ToArray ( );
        }

        /// <summary>
        /// Recalculates the InstanceID for this manga
        /// </summary>
        public void ReCalcInstanceID ( )
        {
            _hash = $"{Connector.ID}{Connector.Website}{this.Name}{this.Link}"
                    .GetHashCode ( )
                    .ToString ( );
        }

        /// <summary>
        /// Returns the <see cref="Manga"/><see cref="String"/> representation
        /// </summary>
        /// <returns></returns>
        public override String ToString ( )
        {
            return Name;
        }
    }
}
