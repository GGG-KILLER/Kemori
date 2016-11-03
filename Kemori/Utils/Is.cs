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
    /// <summary>
    /// A class to check for other types as strings
    /// </summary>
    internal static class Is
    {
        /// <summary>
        /// Returns wether the string is a number
        /// </summary>
        /// <param name="s">The value</param>
        /// <returns></returns>
        public static Boolean Number ( String s )
        {
            Int32 resulti; Single results; Double resultd;
            return Int32.TryParse ( s, out resulti ) || Single.TryParse ( s, out results ) || Double.TryParse ( s, out resultd );
        }
    }
}