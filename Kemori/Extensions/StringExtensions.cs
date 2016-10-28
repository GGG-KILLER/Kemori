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
        public static Int32 LastIndexOfAfter(this String @this, Char value, Int32 startIndex = 0)
        {
            return @this.LastIndexOf ( value, startIndex ) + 1;
        }

        /// <summary>
        /// Returns the last index of the string including the string length
        /// </summary>
        /// <param name="this">the string</param>
        /// <param name="value">Unicode string to seek</param>
        /// <param name="startIndex">Index to start at</param>
        /// <returns></returns>
        public static Int32 LastIndexOfAfter(this String @this, String value, Int32 startIndex = 0)
        {
            return @this.LastIndexOf ( value, startIndex ) + value.Length;
        }
    }
}