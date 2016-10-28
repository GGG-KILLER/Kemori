using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kemori.Classes;
using Kemori.Interfaces;
using Kemori.Resources;

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
        public async Task<String[]> GetPageLinks ( MangaChapter Chapter )
        {
            throw new NotImplementedException ( );
        }

        /// <summary>
        /// Downloads a chapter from a manga
        /// </summary>
        /// <param name="Chapter">The chapter to download</param>
        public virtual async Task DownloadChapter ( MangaChapter Chapter )
        {
            throw new NotImplementedException ( );
        }

        /// <summary>
        /// Returns all chapters from a manga
        /// </summary>
        /// <param name="Manga">The manga to get the chapters from</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<MangaChapter>> GetChapters ( Manga Manga )
        {
            throw new NotImplementedException ( );
        }

        /// <summary>
        /// Loads all mangas
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<Manga>> UpdateMangaList ( )
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
    }
}