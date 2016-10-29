using System;
using System.IO;

namespace Kemori.Utils
{
    internal class PathUtils
    {
        public static String GetPathForFile ( String File )
        {
            var di = new FileInfo ( Path.Combine (
                Environment.GetFolderPath ( Environment.SpecialFolder.CommonApplicationData ),
                nameof ( Kemori ),
                File
            ) );
            di.Directory.Create ( );
            return di.FullName;
        }
    }
}