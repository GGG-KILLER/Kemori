using System;
using System.IO;

namespace Kemori.Utils
{
    internal class PathUtils
    {
        public static String GetPathForFile ( String File )
        {
            return Path.Combine (
                Environment.GetFolderPath ( Environment.SpecialFolder.CommonApplicationData ),
                nameof ( Kemori ),
                File
            );
        }
    }
}