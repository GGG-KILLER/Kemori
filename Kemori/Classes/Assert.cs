using System;

namespace Kemori.Classes
{
    /// <summary>
    /// Assert class
    /// </summary>
    internal class Assert
    {
        /// <summary>
        /// Makes sure the value is not null and throws an exception otherwise
        /// </summary>
        /// <param name="Value">Value to test</param>
        /// <param name="Text">Text to use on exception on throw</param>
        public static void NotNull ( Object Value, String Text = "Should not be null." )
        {
            if ( Value == null )
                throw new Exception ( Text );
        }

        /// <summary>
        /// Makes sure the result is not false and throws an exception otherwise
        /// </summary>
        /// <param name="Result">Result of boolean operation</param>
        /// <param name="Text">Text to use on exception on throw</param>
        public static void Test ( Boolean Result, String Text )
        {
            if ( !Result )
                throw new Exception ( Text );
        }
    }
}