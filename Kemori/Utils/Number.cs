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

namespace Kemori.Utils
{
    internal class Number
    {
        public static Int32 GetPercentage ( Int32 Current, Int32 Total )
        {
            return ( Int32 ) ( Current / ( Double ) Total * 100D );
        }

        /// <summary>
        /// Converts from bytes to megabytes (precision of 2 houses)
        /// </summary>
        /// <param name="Bytes">Number of bytes</param>
        /// <returns></returns>
        public static Double LongToMB ( Int64 Bytes )
        {
            // B -> KB -> MB
            return Math.Round ( ( Bytes / 1024D / 1024D ) * 100D ) / 100D;
            // Math.Round also seems to think I don't want the double precision
            // numbers so the multiplication and division are necessary to
            // preserve them
        }

        /// <summary>
        /// Returns the number making sure it's 5 characters (including padding 0's)
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        public static String DoublePrecisionString ( Double Num )
        {
            // I hope no one will use this to download more than 9.99999... GB
            return Num.ToString ( "##00.00" );
        }
    }
}
