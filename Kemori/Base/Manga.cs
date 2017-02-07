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
using System.Linq;
using System.Threading.Tasks;

namespace Kemori.Base
{
    [Serializable]
    public class Manga : IEquatable<Manga>, IComparable<Manga>
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

        private Int32 _hashCode;
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
            this.Chapters = ( await Connector.GetChaptersAsync ( this ) )
                .ToArray ( );
        }

        /// <summary>
        /// Recalculates the InstanceID for this manga
        /// </summary>
        public void ReCalcInstanceID ( )
        {
            _hashCode = $"{Connector.ID}{Connector.Website}{this.Name}{this.Link}"
                    .GetHashCode ( );
            _hash = _hashCode.ToString ( );
        }

        /// <summary>
        /// Returns the <see cref="Manga" /><see cref="String" /> representation
        /// </summary>
        /// <returns></returns>
        public override String ToString ( )
        {
            return Name;
        }

        /// <summary>
        /// Checks wether this <see cref="Manga" /> has the same values as the
        /// <paramref name="other" />
        /// </summary>
        /// <param name="other"><see cref="Manga" /> to check against</param>
        /// <returns></returns>
        public Boolean Equals ( Manga other )
        {
            return ( Object ) other != null &&
                this.InstanceID == other.InstanceID;
        }

        /// <summary>
        /// Checks wether this <see cref="Manga" /> has the same values as the
        /// <paramref name="obj" />
        /// </summary>
        /// <param name="obj"><see cref="Object" /> to check against</param>
        /// <returns></returns>
        public override Boolean Equals ( Object obj )
        {
            return this.Equals ( obj as Manga );
        }

        /// <summary>
        /// Compares this instance with a specified <see cref="Manga" /> object
        /// and indicates whether this instance precedes, follows, or appears in
        /// the same position in the sort order as the specified string.
        /// </summary>
        /// <param name="other">
        /// The <see cref="Manga" /> to compare with this instance
        /// </param>
        /// <returns></returns>
        public Int32 CompareTo ( Manga other )
        {
            return this.Name.CompareTo ( other.Name );
        }

        /// <summary>
        /// Serves as the hash function
        /// </summary>
        /// <returns></returns>
        public override Int32 GetHashCode ( )
        {
            // Dummmy check to recalc id when needed
            if ( InstanceID == null ) return 0;
            return this._hashCode;
        }

        #region Operators

        public static Boolean operator == ( Manga lhs, Manga rhs )
        {
            return lhs?.Equals ( rhs ) ?? false;
        }

        public static Boolean operator == ( Manga lhs, Object rhs )
        {
            return lhs == rhs as Manga;
        }

        public static Boolean operator != ( Manga lhs, Manga rhs )
        {
            return !( lhs == rhs );
        }

        public static Boolean operator != ( Manga lhs, Object rhs )
        {
            return !( lhs == rhs );
        }

        #endregion Operators
    }
}
