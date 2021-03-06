﻿// UTF-8 Enforcer: 足の不自由なハッキング
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
using System.IO;

namespace Kemori.Utils
{
    public class PathUtils
    {
        public static String GetProgramDataPath ( String File )
        {
            var di = new FileInfo ( Path.Combine (
                Environment.GetFolderPath ( Environment.SpecialFolder.CommonApplicationData ),
                nameof ( Kemori ),
                File
            ) );
            di.Directory.Create ( );
            return di.FullName;
        }

        public static String GetAppDataPath ( String File )
        {
            var di = new FileInfo ( Path.Combine (
                Environment.GetFolderPath
                    ( Environment.SpecialFolder.ApplicationData ),
                nameof ( Kemori ),
                File
            ) );
            di.Directory.Create ( );
            return di.FullName;
        }
    }
}
