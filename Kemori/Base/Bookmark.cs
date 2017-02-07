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

namespace Kemori.Base
{
    [Serializable]
    public class Bookmark : IEquatable<Object>, IEquatable<Bookmark>, IComparable<Bookmark>
    {
        public String ConnectorWebsite;
        public String SearchTerm;

        public override String ToString ( )
        {
            return $"<{ConnectorWebsite}> {SearchTerm}";
        }

        public static Bookmark Parse ( String value )
        {
            var lt = value.IndexOf('<');
            var gt = value.IndexOf('>');

            if ( lt > -1 && gt > -1 )
            {
                var website = value.Substring ( lt + 1, gt - lt - 1 );
                var term = value.Substring ( gt + 1 ).Trim ( );

                return new Bookmark
                {
                    ConnectorWebsite = website,
                    SearchTerm = term
                };
            }
            else throw new Exception ( "Invalid bookmark" );
        }

        public Boolean Equals ( Bookmark book )
        {
            return ( Object ) book != null &&
                this.ConnectorWebsite == book.ConnectorWebsite &&
                this.SearchTerm == book.SearchTerm;
        }

        public override Boolean Equals ( Object obj )
        {
            return this.Equals ( obj as Bookmark );
        }

        public static Boolean Equals ( Bookmark a, Bookmark b )
        {
            return a?.Equals ( b ) ?? false;
        }

        public static Boolean Equals ( Bookmark a, Object b )
        {
            return a?.Equals ( b as Bookmark ) ?? false;
        }

        public Int32 CompareTo ( Bookmark other )
        {
            var webs = this.ConnectorWebsite.CompareTo ( other.ConnectorWebsite );
            // Sort by term if the connector is the same
            return webs == 0 ? this.SearchTerm.CompareTo ( other.SearchTerm ) : webs;
        }

        public override Int32 GetHashCode ( )
        {
            return this.ToString ( ).GetHashCode ( );
        }

        public static Boolean operator == ( Bookmark lhs, Bookmark rhs )
        {
            return ( Object ) lhs != null &&
                ( Object ) rhs != null &&
                lhs.Equals ( rhs );
        }

        public static Boolean operator == ( Bookmark lhs, Object rhs )
        {
            return lhs == rhs as Bookmark;
        }

        public static Boolean operator != ( Bookmark lhs, Bookmark rhs )
        {
            return !( lhs == rhs );
        }

        public static Boolean operator != ( Bookmark lhs, Object rhs )
        {
            return !( lhs == rhs );
        }
    }
}
