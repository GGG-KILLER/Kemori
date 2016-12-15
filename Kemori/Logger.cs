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
    public class Logger
    {
        private readonly FileInfo LogFile;

        public Logger ( )
        {
            LogFile = new FileInfo ( PathUtils.GetProgramDataPath ( "kemori.log" ) );
        }

        public void Init ( )
        {
            lock ( LogFile )
            {// 2 MB size limit
                if ( LogFile.Exists && LogFile.Length > 2 * 1024 * 1024 )
                {
                    LogFile.Delete ( );
                }

                var b = new String ( '=', 24 );
                this.Log ( String.Empty );
                this.Log ( b );
                this.Log ( $"=== {( DateTime.Now.ToString ( "%Y-%m-%dT%H:%M:%S" ) )} +0000 ===" );
                this.Log ( b );
                this.Log ( String.Empty );
            }
        }

        public void Log ( Object item )
        {
            lock ( LogFile )
            {
                using ( var log = new StreamWriter ( LogFile.FullName, true ) )
                {
                    log.WriteLine ( item.ToString ( ) );
                    log.Flush ( );
                }
            }
        }
    }
}
