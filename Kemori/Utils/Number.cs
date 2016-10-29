using System;

namespace Kemori.Utils
{
    internal class Number
    {
        /// <summary>
        /// Converts from bytes to megabytes (precision of 2 houses)
        /// </summary>
        /// <param name="Bytes">Number of bytes</param>
        /// <returns></returns>
        public static Double LongToMB ( Int64 Bytes )
        {
            //                    B  ->   KB   ->  MB
            return Math.Round ( ( Bytes / 1024D / 1024D ) * 100D ) / 100D;
            // Math.Round also seems to think I don't want
            // the double precision numbers so the multiplication
            // and division are necessary to preserve them
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