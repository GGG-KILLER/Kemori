using System;

namespace Kemori.Extensions
{
    public static class StringExtensions
    {
        public static String Before ( this String @this, Char Separator, Int32 startIndex = 0 ) =>
            @this.Substring ( 0, @this.IndexOf ( Separator, startIndex ) );

        public static String Before ( this String @this, String Separator, Int32 startIndex = 0 ) =>
            @this.Substring ( 0, @this.IndexOf ( Separator, startIndex ) );

        public static String After ( this String @this, Char Separator, Int32 startIndex = 0 ) =>
            @this.Substring ( @this.IndexOf ( Separator, startIndex ) + 1 );

        public static String After ( this String @this, String Separator, Int32 startIndex = 0 ) =>
            @this.Substring ( @this.IndexOf ( Separator, startIndex ) + Separator.Length );

        public static Int32 IndexOfAfter ( this String @this, Char Separator, Int32 startIndex = 0 ) =>
            @this.IndexOf ( Separator, startIndex ) + 1;

        public static Int32 IndexOfAfter ( this String @this, String Separator, Int32 startIndex = 0 ) =>
            @this.IndexOf ( Separator, startIndex ) + Separator.Length;
    }
}