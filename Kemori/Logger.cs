// UTF-8 Enforcer: 足の不自由なハッキング
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
using Kemori.Utils;

namespace Kemori
{
    public static class Logger
    {
        private static readonly Object _lock = new Object ( );
        private static readonly FileInfo _logFile;
        private static readonly StreamWriter _logStream;
        private const Int32 MaxLogSize = 2 * 1024 * 1024;

        static Logger ( )
        {
            _logFile = new FileInfo (
                PathUtils.GetProgramDataPath ( "kemori.log" ) );
            _logStream = new StreamWriter ( _logFile.FullName, true )
            {
                AutoFlush = true,
                NewLine = "\n"
            };
        }

        public static void Init ( )
        {
            lock ( _lock )
            {
                // 2 MB size limit
                if ( _logFile.Exists && _logFile.Length > MaxLogSize )
                {
                    _logFile.Delete ( );
                }

                var b = new String ( '=', 24 );
                Log ( String.Empty );
                Log ( b );
                Log ( $"=== {( DateTime.Now.ToString ( "%Y-%m-%dT%H:%M:%S" ) )} +0000 ===" );
                Log ( b );
                Log ( String.Empty );
            }
        }

        public static void Log ( Object item )
        {
            lock ( _lock )
            {
                _logStream.WriteLine ( item.ToString ( ) );
            }
        }
    }
}
