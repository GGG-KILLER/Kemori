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
        public String Root { get; private set; }

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
        /// </summary>
        /// <param name="path"></param>
        /// <param name="sanitizeFolderNames"></param>
        /// <returns></returns>
        public String NormalizePath ( String path, Boolean sanitizeFolderNames = true )
        {
            var stack = new Stack<String> ( );
            var parts = path.Split ( Path.DirectorySeparatorChar,
                Path.AltDirectorySeparatorChar );

            foreach ( var part in parts )
            {
                switch ( part )
                {
                    case ".":
                        continue;
                    case "..":
                        if ( stack.Count < 1 )
                            throw new Exception ( "Path cannot go a directory up from the root." );
                        else
                            stack.Pop ( );
                        break;

                    default:
                        stack.Push ( sanitizeFolderNames ?
                            SafeFolderName ( part ) :
                            part );
                        break;
                }
            }

            return String.Join ( Path.DirectorySeparatorChar.ToString ( ), stack );
        }

        /// <summary>
        /// Returns the path inside the fixed environment
        /// </summary>
        /// <param name="path">Path to get</param>
        /// <returns></returns>
        private String GetPath ( String path )
        {
            return Path.Combine (
                Root,
                NormalizePath ( path )
            );
        }

        public Boolean FileInUse ( FileInfo file )
        {
            FileStream stream = null;

            try
            {
                stream = file.Open ( FileMode.Open, FileAccess.ReadWrite, FileShare.None );
            }
            catch ( IOException )
            {
                // the file is unavailable because it is: still being written to
                // or being processed by another thread or does not exist (has
                // already been processed)
                return true;
            }
            finally
            {
                if ( stream != null )
                    stream.Close ( );
            }

            //file is not locked
            return false;
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
            {
                fi.Delete ( );
            }

            // Creates the file if it doesn't exists
            fi.Create ( );

            // Creates and opens the file for writing
            using ( var writer = fi.Open (
                Append ? FileMode.Append : FileMode.Open,
                FileAccess.Write,
                FileShare.None ) )
            {
                writer.Write ( Data, 0, Data.Length );
                writer.Flush ( );
                writer.Close ( );
            }
        }

        /// <summary>
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
            var newLine = Encoding.UTF8.GetBytes ( Environment.NewLine );
            foreach ( var Line in Lines )
            {
                bytes.AddRange ( Encoding.UTF8.GetBytes ( Line ) );
                bytes.AddRange ( newLine );
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
        /// Returns a path for a provided <see cref="Manga" />
        /// </summary>
        /// <param name="Manga"><see cref="Manga" /> to return path for</param>
        /// <returns></returns>
        public String PathForManga ( Manga Manga )
        {
            return PathForManga ( Manga.ToString ( ) );
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
                // If we fail, log all possibly related information
                Logger.Log ( "Failed to generate path for manga :" );
                Logger.Log ( $"\tName: {Name}" );
                Logger.Log ( $"\tSafeName: {SafeFolderName ( Name )}" );

                return Path.Combine (
                    ConfigsManager.SavePath,
                    SafeFolderName ( Name )
                );
            }
        }

        /// <summary>
        /// Returns a path for a provided <see cref="MangaChapter" />
        /// </summary>
        /// <param name="Chapter">
        /// <see cref="MangaChapter" /> to return path for
        /// </param>
        /// <returns></returns>
        public String PathForChapter ( MangaChapter Chapter )
        {
            return Path.Combine (
                PathForManga ( Chapter.Manga.ToString ( ) ),
                SafeFolderName ( Chapter.ToString ( ) )
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
        /// Removes all invalid folder name characters from a provided folder name
        /// </summary>
        /// <param name="Unsafe">Unsafe folder name</param>
        /// <returns></returns>
        public String SafeFolderName ( String Unsafe )
        {
            var safe = new StringBuilder ( );
            var badChars = Path.GetInvalidPathChars ( );

            // faster than String.Replace
            foreach ( var ch in Unsafe )
                if ( !badChars.Contains ( ch ) )
                    safe.Append ( ch );

            return safe.ToString ( );
        }

        /// <summary>
        /// Removes all invalid file name characters from a provided filename
        /// </summary>
        /// <param name="Unsafe">Unsafe file name</param>
        /// <returns></returns>
        public String SafeFileName ( String Unsafe )
        {
            var safe = new StringBuilder ( );
            var badChars = Path.GetInvalidFileNameChars ( );

            foreach ( var ch in Unsafe )
                if ( !badChars.Contains ( ch ) )
                    safe.Append ( ch );

            return safe.ToString ( );
        }
    }
}
