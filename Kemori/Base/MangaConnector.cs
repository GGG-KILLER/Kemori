using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// The UNIQUE ID of this connector
        /// </summary>
        public String ID { get; private set; }

        /// <summary>
        /// The list of mangas available for this provider
        /// </summary>
        public Manga[] MangaList { get; private set; }

        /// <summary>
        /// The website (MangaFox, MangaHere, etc.) this downloader is associated to
        /// </summary>
        public String Website { get; private set; }

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
            String root;
            Boolean Compressed;
            MangaChapter Chapter;
            ZipFile ZipFile;

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
    }
}