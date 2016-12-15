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

namespace Kemori.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the string before the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String Before ( this String @this, Char Separator, Int32 startIndex = 0 )
        {
            return @this.Substring ( 0, @this.IndexOf ( Separator, startIndex ) );
        }

        /// <summary>
        /// Returns the string before the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode string to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String Before ( this String @this, String Separator, Int32 startIndex = 0 )
        {
            return @this.Substring ( 0, @this.IndexOf ( Separator, startIndex ) );
        }

        /// <summary>
        /// Returns teh string before the last match of the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String BeforeLast ( this String @this, Char Separator, Int32 startIndex = 0 )
        {
            startIndex = startIndex > 0 ? startIndex : @this.Length - 1;
            return @this.Substring ( 0, @this.LastIndexOf ( Separator, startIndex ) );
        }

        /// <summary>
        /// Returns the string before the last match of the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode string to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String BeforeLast ( this String @this, String Separator, Int32 startIndex = 0 )
        {
            startIndex = startIndex > 0 ? startIndex : @this.Length - 1;
            return @this.Substring (
                0,
                @this.LastIndexOf ( Separator, startIndex ) + Separator.Length
            );
        }

        /// <summary>
        /// Returns the string after the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String After ( this String @this, Char Separator, Int32 startIndex = 0 )
        {
            return @this.Substring ( @this.IndexOf ( Separator, startIndex ) + 1 );
        }

        /// <summary>
        /// Returns the string after the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String After ( this String @this, String Separator, Int32 startIndex = 0 )
        {
            return @this.Substring (
                @this.IndexOf ( Separator, startIndex ) + Separator.Length
            );
        }

        /// <summary>
        /// Returns the string after the last match of the provided separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String AfterLast ( this String @this, Char Separator, Int32 startIndex = 0 )
        {
            startIndex = startIndex > 0 ? startIndex : @this.Length - 1;
            return @this.Substring ( @this.LastIndexOf ( Separator, startIndex ) + 1 );
        }

        /// <summary>
        /// Returns the string after the last match of the separator
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="Separator">Unicode string to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static String AfterLast ( this String @this, String Separator, Int32 startIndex = 0 )
        {
            startIndex = startIndex > 0 ? startIndex : @this.Length - 1;
            return @this.Substring (
                @this.LastIndexOf ( Separator, startIndex ) + Separator.Length
            );
        }

        /// <summary>
        /// Returns the index of the character including the character length
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="value">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static Int32 IndexOfAfter ( this String @this, Char value, Int32 startIndex = 0 )
        {
            return @this.IndexOf ( value, startIndex ) + 1;
        }

        /// <summary>
        /// Returns the index of the string including the string length
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="value">Unicode string to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static Int32 IndexOfAfter ( this String @this, String value, Int32 startIndex = 0 )
        {
            return @this.IndexOf ( value, startIndex ) + value.Length;
        }

        /// <summary>
        /// Returns the last index of the character including the character length
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="value">Unicode character to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static Int32 LastIndexOfAfter ( this String @this, Char value, Int32 startIndex = 0 )
        {
            startIndex = startIndex > 0 ? startIndex : @this.Length - 1;
            return @this.LastIndexOf ( value, startIndex ) + 1;
        }

        /// <summary>
        /// Returns the last index of the string including the string length
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="value">Unicode string to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static Int32 LastIndexOfAfter ( this String @this, String value, Int32 startIndex = 0 )
        {
            startIndex = startIndex > 0 ? startIndex : @this.Length - 1;
            return @this.LastIndexOf ( value, startIndex ) + value.Length;
        }
    }
}
