using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kemori.Base;

namespace Kemori.Interfaces
{
    /// <summary>
    /// Defined the handler that will receive the progress of the manga downloader
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MangaDownloadProgressChangedHandler ( MangaConnector sender, System.Net.DownloadProgressChangedEventArgs e );

    /// <summary>
    /// Provides the base for creating a manga connector
    /// </summary>
    public interface IMangaConnector : IDisposable
    {
        /// <summary>
        /// The UNIQUE ID of this connector
        /// </summary>
        String ID { get; }

        /// <summary>
        /// The website (MangaFox, MangaHere, etc.) this downloader is associated to
        /// </summary>
        String Website { get; }

        /// <summary>
        /// The list of mangas available for this provider
        /// </summary>
        Manga[] MangaList { get; }

        /// <summary>
        /// Called when the download makes progress
        /// </summary>
        event MangaDownloadProgressChangedHandler MangaDownloadProgressChanged;

        /// <summary>
        /// Loads all mangas
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Manga>> UpdateMangaListAsync ( );

        /// <summary>
        /// Returns all chapters from a manga
        /// </summary>
        /// <param name="Manga">The manga to get the chapters from</param>
        /// <returns></returns>
        Task<IEnumerable<MangaChapter>> GetChaptersAsync ( Manga Manga );

        /// <summary>
        /// Downloads a chapter from a manga
        /// </summary>
        /// <param name="Chapter">The chapter to download</param>
        Task DownloadChapterAsync ( MangaChapter Chapter );

        /// <summary>
        /// Gets all page image links from a chapter
        /// </summary>
        /// <param name="Chapter">The chapter</param>
        /// <returns></returns>
        Task<String[]> GetPageLinksAsync ( MangaChapter Chapter );
    }
}