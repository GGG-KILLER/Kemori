using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Kemori.Classes
{
    // Derived from: http://www.codeproject.com/Articles/1966/An-INI-file-handling-class-using-C
    /// <summary>
    /// Create a New INI file to store or load data
    /// </summary>
    public class IniFile
    {
        /// <summary>
        /// Path of the INI file
        /// </summary>
        public String path;

        #region Unmanaged funcs

        [DllImport ( "kernel32" )]
        private static extern Int64 WritePrivateProfileString ( String section,
            String key, String val, String filePath );

        [DllImport ( "kernel32" )]
        private static extern Int32 GetPrivateProfileString ( String section,
                 String key, String def, StringBuilder retVal,
            Int32 size, String filePath );

        #endregion Unmanaged funcs

        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile ( String INIPath )
        {
            path = INIPath;
        }

        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <param name="Value">Value name</param>
        public void IniWriteValue ( String Section, String Key, Object Value )
        {
            WritePrivateProfileString ( Section, Key, Value.ToString ( ), this.path );
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <param name="Section">Section name</param>
        /// <param name="Key">Key name</param>
        /// <returns></returns>
        public T IniReadValue<T> ( String Section, String Key )
        {
            var value = new StringBuilder ( 255 );
            var i = GetPrivateProfileString ( Section, Key, "", value, 255, this.path );
            return ( T ) Convert.ChangeType ( value.ToString ( ), typeof ( T ) );
        }
    }
}