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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kemori.Base;
using Kemori.Resources;

namespace Kemori.Classes
{
    /// <summary>
    /// Restricted IO class
    /// </summary>
    public class IO
    {
        /// <summary>
        /// Root of the restricted IO
        /// </summary>
        private String Root;

        #region TempAppend

        /// <summary>
        /// Temporary append state class
        /// </summary>
        public class TempAppend : IDisposable
        {
            private readonly IO Class;

            /// <summary>
            /// Starts the append state
            /// </summary>
            /// <param name="Class">Class to apply the state to</param>
            internal TempAppend ( IO Class )
            {
                this.Class = Class;
                this.Class.Append = true;
            }

            /// <summary>
            /// Stops the append state
            /// </summary>
            public void Dispose ( )
            {
                this.Class.Append = false;
            }
        }

        #endregion TempAppend

        /// <summary>
        /// Wether we should append or overwrite
        /// </summary>
        private Boolean Append;

        /// <summary>
        /// Restricted IO class
        /// </summary>
        /// <param name="root"></param>
        public IO ( String root )
        {
            this.Root = Path.GetFullPath ( root );
        }

        /// <summary>
        /// Returns the path inside the fixed environment
        /// </summary>
        /// <param name="path">Path to get</param>
        /// <returns></returns>
        private String GetPath ( String path )
        {
            return Path.Combine ( Root, path.Replace ( "..", "." ) );
        }

        /// <summary>
        /// Writes all bytes to a file (makes use of append mode)
        /// </summary>
        /// <param name="Path">The path of the file</param>
        /// <param name="Data">The data to write</param>
        public void WriteData ( String Path, Byte[] Data )
        {
            Path = GetPath ( Path );
            var fi = new FileInfo ( Path );

            // Creates the directory structure if it doesn't exists
            fi.Directory.Create ( );

            // Deletes the file if it exists and we're not on append mode
            if ( fi.Exists && !Append )
                fi.Delete ( );

            // Creates and opens the file for writing
            using ( var writer = fi.OpenWrite ( ) )
                writer.Write ( Data, ( Int32 ) ( !Append && fi.Exists ? 0 : fi.Length ), Data.Length );
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Text"></param>
        public void WriteAllText ( String Path, String Text )
        {
            WriteData ( Path, Encoding.UTF8.GetBytes ( Text ) );
        }

        /// <summary>
        /// Writes all lines to the given file
        /// </summary>
        /// <param name="Path">File to write to</param>
        /// <param name="Lines">Lines to write to the file</param>
        public void WriteAllLines ( String Path, IEnumerable<String> Lines )
        {
            var bytes = new List<Byte> ( );

            foreach ( var Line in Lines )
            {
                bytes.AddRange ( Encoding.UTF8.GetBytes ( Line ) );
            }

            WriteData ( Path, bytes.ToArray ( ) );
        }

        /// <summary>
        /// Turns on append mode temporarely
        /// </summary>
        /// <returns></returns>
        public TempAppend AppendMode ( )
        {
            return new TempAppend ( this );
        }

        /// <summary>
        /// Returns a path for a provided <see cref="Manga"/>
        /// </summary>
        /// <param name="Manga"><see cref="Manga"/> to return path for</param>
        /// <returns></returns>
        public String PathForManga ( Manga Manga )
        {
            return Path.Combine (
                ConfigsManager.SavePath,
                SafeFolderName ( Manga.Name )
            );
        }

        /// <summary>
        /// Returns a path for a provided manga
        /// </summary>
        /// <param name="Name">Manga to return path for</param>
        /// <returns></returns>
        public String PathForManga ( String Name )
        {
            try
            {
                return Path.Combine (
                    ConfigsManager.SavePath,
                    SafeFolderName ( Name )
                );
            }
            catch ( Exception )
            {
                var l = new Logger ( );
                l.Log ( "Failed to generate path for manga :" );
                l.Log ( $"\tName: {Name}" );
                l.Log ( $"\tSafeName: {SafeFolderName ( Name )}" );

                return Path.Combine (
                    ConfigsManager.SavePath,
                    SafeFolderName ( Name )
                );
            }
        }

        /// <summary>
        /// Returns a path for a provided <see cref="MangaChapter"/>
        /// </summary>
        /// <param name="Chapter"><see cref="MangaChapter"/> to return path for</param>
        /// <returns></returns>
        public String PathForChapter ( MangaChapter Chapter )
        {
            return Path.Combine (
                PathForManga ( Chapter.Manga ),
                SafeFolderName ( Chapter.Name )
            );
        }

        /// <summary>
        /// Returns a pth for a provided manga chapter
        /// </summary>
        /// <param name="MangaName"></param>
        /// <param name="ChapterName"></param>
        /// <returns></returns>
        public String PathForChapter ( String MangaName, String ChapterName )
        {
            return Path.Combine (
                PathForManga ( MangaName ),
                SafeFolderName ( ChapterName )
            );
        }

        /// <summary>
        /// Returns a path stripped of invalid path characters
        /// </summary>
        /// <param name="Unsafe">Unstripped path</param>
        /// <returns></returns>
        public String SafeFolderName ( String Unsafe )
        {
            var safe = new StringBuilder ( );
            var badChars = Path.GetInvalidPathChars ( );

            // *hopefully* faster than String.Replace
            foreach ( var ch in Unsafe )
                if ( !badChars.Contains ( ch ) )
                    safe.Append ( ch );

            return safe.ToString ( );
        }
    }
}