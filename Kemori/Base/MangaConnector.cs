﻿/*
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
using System.Threading.Tasks;
using Ionic.Zip;
using Kemori.Classes;
using Kemori.Interfaces;
using Kemori.Resources;
using Kemori.Utils;

namespace Kemori.Base
{
    public class MangaConnector : IMangaConnector
    {
        public Logger Logger;

        /// <summary>
        /// Base manga connector (do not instance this)
        /// </summary>
        public MangaConnector ( )
        {
            InitHTTP ( );
            InitIO ( );
            MangaList = new Manga[0];
            Logger = new Logger ( );
        }

        /// <summary>
        /// Initializes the restricted IO system
        /// </summary>
        private void InitIO ( )
        {
            IO = new IO ( ConfigsManager.SavePath );
        }

        /// <summary>
        /// Class' HTTP client
        /// </summary>
        public Fetch HTTP;

        /// <summary>
        /// Class' IO handler
        /// </summary>
        public IO IO;

        String _id;
        /// <summary>
        /// The UNIQUE ID of this connector (can only be set once)
        /// </summary>
        /// 
        public String ID
        {
            get
            {
                return _id;
            }
            set
            {
                if ( _id == null )
                    _id = value;
                else
                    throw new Exception ( "ID is a readonly property." );
            }
        }

        /// <summary>
        /// The list of mangas available for this provider
        /// </summary>
        public Manga[] MangaList
        {
            get; private set;
        }


        String _website;
        /// <summary>
        /// The website (MangaFox, MangaHere, etc.) this downloader is associated to
        /// </summary>
        public String Website
        {
            get
            {
                return _website;
            }
            set
            {
                if ( _website == null )
                    _website = value;
                else
                    throw new Exception ( "Website is a readonly property." );
            }
        }

        /// <summary>
        /// Called when the download makes progress
        /// </summary>
        public virtual event MangaDownloadProgressChangedHandler MangaDownloadProgressChanged;

        /// <summary>
        /// Essential to call to get rid of the HTTP client
        /// </summary>
        public void Dispose ( )
        {
            if ( HTTP != null )
                HTTP.Dispose ( );
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Gets all page images links for a given chapter
        /// </summary>
        /// <param name="Chapter">The chapter</param>
        /// <returns></returns>
        public virtual async Task<String[]> GetPageLinksAsync ( MangaChapter Chapter )
        {
            throw new NotImplementedException ( );
        }

        /// <summary>
        /// Downloads a chapter from a manga
        /// </summary>
        /// <param name="Chapter">The chapter to download</param>
        public virtual async Task DownloadChapterAsync ( MangaChapter Chapter )
        {
            throw new NotImplementedException ( );
        }

        /// <summary>
        /// Returns all chapters from a manga
        /// </summary>
        /// <param name="Manga">The manga to get the chapters from</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<MangaChapter>> GetChaptersAsync ( Manga Manga )
        {
            throw new NotImplementedException ( );
        }

        /// <summary>
        /// Loads all mangas
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Manga>> UpdateMangaListAsync ( )
        {
            throw new NotImplementedException ( );
        }

#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

        /// <summary>
        /// Should be overriden to initialize the HTTP client properly
        /// </summary>
        public virtual void InitHTTP ( )
        {
            HTTP = new Fetch ( );
        }

        // Shamefully copied from hakuneko (few modifications):
        /// <summary>
        /// Normalizes chapter numbers so they all follow the same style
        /// inside the application
        /// </summary>
        /// <param name="ChapterNumber">The string that contains the chapter number</param>
        /// <returns></returns>
        public String NormalizeChapterNumber ( String ChapterNumber )
        {
            /*
             * From hakuneko:
             *
             * Plain chapter variations:
             * %c = "13-18"
             * %c = "13"
             * %c = "13.5"
             * %c = "13v2"
             * %c = "13v.2"
             * %c = "13.5v2"
             * %c = "13.5v.2"
             *
             * Variations combined with text:
             * %ct = "Text Text"
             * %ct = "%c Text Text"
             * %ct = "Text Text %c"
             */

            // Remove preceding/suceeding text from chapter number
            var chNumberResidualLength = 0;

            // Sometimes chapter numbers are followed by text
            var posFirstSpace = ChapterNumber.IndexOf ( " " );

            // Sometimes chapter numbers are preceded by text
            var posLastSpace = ChapterNumber.LastIndexOf ( " " );

            if ( posFirstSpace > -1 && posLastSpace > -1 )
            {
                // NOTE: in case numbers are at beginning and ending, the number at beginning got higher priority
                if ( Is.Int ( ChapterNumber.Substring ( 0, posFirstSpace ) ) )
                {
                    return ChapterNumber.Substring ( 0, posFirstSpace );
                }

                if ( Is.Int ( ChapterNumber.Substring ( posLastSpace + 1 ) ) )
                {
                    return ChapterNumber.Substring ( posLastSpace + 1 );
                }
            }

            // sometimes chapter numbers are spanned with a hyphen (i.e. 13-18)
            var posHyphen = ChapterNumber.IndexOf ( '-' );
            if ( posHyphen > -1 )
            {
                var from = ChapterNumber.Substring ( 0, posHyphen );
                var to = ChapterNumber.Substring ( posHyphen );

                from = NormalizeChapterNumber ( from );
                to = NormalizeChapterNumber ( to );

                return $"{from}-{to}";
            }

            // sometimes chapter numbers are followed by version (i.e. 13v2, 13v.2)
            var posVchar = ChapterNumber.IndexOf ( 'v' );
            if ( posVchar > -1 )
            {
                chNumberResidualLength = Math.Max ( chNumberResidualLength, ChapterNumber.Length - posVchar );
            }

            // sometimes chapter numbers contains a dot (i.e. 13.5, 13.5v2, 13.5.v2)
            var posDot = ChapterNumber.IndexOf ( '.' );
            if ( posDot > -1 )
            {
                chNumberResidualLength = Math.Max ( chNumberResidualLength, ChapterNumber.Length - posDot );
            }

            // Replaces the while *ChapterNumber = wxT("0") + *ChapterNumber
            return ChapterNumber.PadLeft (
                // This replaces the ChapterNumber.Length - chNumberResidualLength < 4
                Math.Max ( 4 - ( ChapterNumber.Length + chNumberResidualLength ), 0 ),
                '0'
            );
        }

        public class ChapterFileProcessor : IDisposable
        {
            private readonly String root;
            private readonly Boolean Compressed;
            private readonly MangaChapter Chapter;
            private readonly ZipFile ZipFile;

            /// <summary>
            /// Initializes a chapter processor
            /// </summary>
            /// <param name="Chapter"></param>
            public ChapterFileProcessor ( MangaChapter Chapter )
            {
                root = new IO ( ConfigsManager.SavePath )
                    .PathForChapter ( Chapter );
                this.Chapter = Chapter;

                if ( Compressed = ConfigsManager.CompressChapters )
                {
                    ZipFile = new ZipFile ( root + ".cbz" );
                }
            }

            /// <summary>
            /// Saves a file to the filesystem
            /// </summary>
            /// <param name="FileName">File name</param>
            /// <param name="Contents">Contents of the file</param>
            public async Task SaveFileAsync ( String FileName, Byte[] Contents )
            {
                if ( Compressed )
                {
                    ZipFile.AddEntry ( FileName, Contents );
                }
                else
                {
                    using ( var stream = File.Open ( Path.Combine ( root, FileName ), FileMode.OpenOrCreate ) )
                        await stream.WriteAsync ( Contents, 0, Contents.Length );
                }
            }

            public void Dispose ( )
            {
                // Saves the zip file
                ZipFile?.Save ( );

                // Dispose the ZipFile if it exists
                ZipFile?.Dispose ( );

                GC.SuppressFinalize ( this );
            }
        }

        /// <summary>
        /// Saves all bytes to the file according to the user's settings (compression, etc.)
        /// </summary>
        /// <param name="Chapter">Chapter that is being downloaded</param>
        public static ChapterFileProcessor GetFileProcessor ( MangaChapter Chapter )
        {
            return new ChapterFileProcessor ( Chapter );
        }

        public FileInfo GetMangaListInfo ( )
        {
            var fi = new FileInfo ( PathUtils.GetPathForFile ( $"lists/{ID}.list" ) );
            fi.Directory.Create ( );
            if ( !fi.Exists )
                fi.Create ( );
            return fi;
        }

        /// <summary>
        /// Loads the manga list from the local cache
        /// </summary>
        public async void LoadMangaListFromCacheAsync ( )
        {
            var fi = GetMangaListInfo ( );

            try
            {
                if ( fi.Exists && fi.Length > 0 )
                    this.MangaList = await SerializerUtils
                        .DeserializeFromFileAsync<Manga[]> ( fi.FullName );
            }
            catch ( Exception )
            {
                // For some reason the config didn't load
                return;
            }
        }

        /// <summary>
        /// Updates the local cache of the manga list
        /// </summary>
        public async void UpdateMangaListCacheAsync ( )
        {
            var list = ( await UpdateMangaListAsync ( ) )
                .ToArray ( );

            this.MangaList = list;

            await SerializerUtils.SerializeToFileAsync (
                list,
                GetMangaListInfo ( ).FullName
            );
        }

        /// <summary>
        /// Returns the <see cref="MangaConnector"/> <see cref="String"/> representation
        /// </summary>
        /// <returns></returns>
        public override String ToString ( )
        {
            return Website;
        }
    }
}