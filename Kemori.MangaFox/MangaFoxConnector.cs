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
using System.Threading.Tasks;
using System.Web;
using Kemori.Base;
using Kemori.Extensions;
using Kemori.Interfaces;

namespace Kemori.Connectors
{
    public class MangaFoxConnector : MangaConnector
    {
        /// <summary>
        /// Called when the download makes progress
        /// </summary>
        public override event MangaDownloadProgressChangedHandler MangaDownloadProgressChanged;

        public MangaFoxConnector ( )
        {
            this.ID = "KemoriMangaFoxConnector";
            this.Website = "MangaFox.me";
        }

        /// <summary>
        /// Initializes the <see cref="Classes.Fetch"/> of this Connector
        /// </summary>
        public override void InitHTTP ( )
        {
            HTTP = new Classes.Fetch ( "http://mangafox.me", "http://mangafox.me" );
        }

        /// <summary>
        /// Downloads a certain chapter
        /// </summary>
        /// <param name="Chapter">Chapter to download</param>
        public override async Task DownloadChapterAsync ( MangaChapter Chapter )
        {
            // Loads all pages images and page count
            Chapter.Load ( );

            HTTP.DownloadProgressChanged += HTTP_DownloadProgressChanged;

            // Creates a ChapterFileProcessor
            using ( var pageProcessor = GetFileProcessor ( Chapter ) )
            {
                // Loops through all pages
                for ( var i = 0 ; i < Chapter.Pages ; i++ )
                {
                    var page = Chapter.PageLinks[i];

                    // Saves the file bytes
                    await pageProcessor.SaveFileAsync (
                        // 00X.ext file formats
                        i.ToString ( ).PadLeft ( 3, '0' ) + Path.GetExtension ( page ),
                        // Downloads the bytes from the image
                        await HTTP.GetDataAsync ( page )
                    );
                }
            }

            HTTP.DownloadProgressChanged -= HTTP_DownloadProgressChanged;
        }

        /// <summary>
        /// Pipes the progresschanged event from the <see cref="Classes.Fetch"/>
        /// event to ours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HTTP_DownloadProgressChanged ( Object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            this.MangaDownloadProgressChanged?.Invoke ( this, e );
        }

        // Shamefully copied from hakuneko
        /// <summary>
        /// Retrieves the latest manga list from MangaFox
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Manga>> UpdateMangaListAsync ( )
        {
            var mangaList = new List<Manga> ( );
            Logger.Log ( "Starting to update the manga list of mangafox.me" );

            try
            {
                var HTML = await HTTP.GetStringAsync ( "http://mangafox.me/manga/", "http://mangafox.me" );
                var indexStart = HTML.IndexOf ( "<div class=\"manga_list\">" ) + 24;
                var indexEnd = HTML.IndexOf ( "<div id=\"footer\">" );

                // Only continue if we got the right html
                if ( indexStart > 23 && indexEnd >= -1 )
                {
                    HTML = HTML.Substring ( indexStart, indexEnd - indexStart );
                    indexEnd = 0;

                    // Example Entry: <li><a href="http://mangafox.me/manga/name/" rel="8894" class="series_preview manga_open">Label</a></li>
                    while ( ( indexStart = HTML.IndexOf ( "<li><a href=\"", indexEnd ) ) > -1 )
                    {
                        indexStart += 13;
                        indexEnd = HTML.IndexOf ( '"', indexStart );
                        var mangaLink = HTML.Substring ( indexStart, indexEnd - indexStart );

                        indexStart = HTML.IndexOf ( '>', indexEnd ) + 1;
                        indexEnd = HTML.IndexOf ( '<', indexStart );
                        var mangaLabel = HTML
                            .Substring ( indexStart, indexEnd - indexStart )
                            .Trim ( );

                        if ( mangaLabel != "" )
                        {
                            mangaLabel = HttpUtility.HtmlDecode ( mangaLabel );
                            mangaList.Add ( new Manga
                            {
                                Name = mangaLabel,
                                Link = mangaLink,
                                Connector = this,
                                Path = IO.PathForManga ( mangaLabel )
                            } );
                        }
                    }
                }
            }
            catch ( Exception e )
            {
                Logger.Log ( "Error retrieving manga list: " );
                Logger.Log ( e );
            }

            Logger.Log ( "Done retrieving manga list." );

            return mangaList;
        }

        // Shamefully copied from Hakuneko
        /// <summary>
        /// Retrieves the list of <see cref="MangaChapter"/> from the
        /// provided <see cref="Manga"/>
        /// </summary>
        /// <param name="Manga">The <see cref="Manga"/> to get the chapters from</param>
        /// <returns></returns>
        public async override Task<IEnumerable<MangaChapter>> GetChaptersAsync ( Manga Manga )
        {
            var chapterList = new List<MangaChapter> ( );

            try
            {
                String volumePrefix = String.Empty,
                    chNumber,
                    chTitle,
                    chLink;

                var HTML = await HTTP.GetStringAsync ( Manga.Link );
                var indexStart = HTML.IndexOf ( "</h2><hr/>" ) + 10;
                var indexEnd = HTML.IndexOf ( "<div id=\"discussion\"", indexStart );
                var volumeIndexCurrent = 0;
                var volumeIndexNext = 0;

                if ( indexStart > 9 && indexEnd >= -1 )
                {
                    HTML = HTML.Substring ( indexStart, indexEnd - indexStart );
                    indexEnd = 0;
                    volumeIndexNext = HTML.IndexOf ( "<h3 class=\"volume\">", volumeIndexNext );

                    // Example Volume: <h3 class="volume">Volume 02 <span>Chapter 5 - 8</h3>
                    // Example Entry: <a href="http://mangafox.me/manga/manga/v02/c008/1.html" title="thx" class="tips">Manga 8</a>         <span class="title nowrap">Label</span>
                    while ( ( indexStart = HTML.IndexOf ( "<a href=\"", indexEnd ) ) > -1 )
                    {
                        // automatically true on first interation
                        if ( volumeIndexNext > -1 && volumeIndexNext < indexStart )
                        {
                            // get the volume name of the current volume
                            volumeIndexCurrent = volumeIndexNext + 19;
                            volumeIndexNext = HTML.IndexOf ( '<', volumeIndexCurrent );
                            volumePrefix = HTML.Substring ( volumeIndexCurrent, volumeIndexNext - volumeIndexCurrent );
                            // go to next volume
                            volumeIndexNext = HTML.IndexOf ( "<h3 class=\"volume\">", volumeIndexNext );
                        }

                        // Getting link of chapter
                        indexStart += 9; // "href=\""
                        indexEnd = HTML.IndexOf ( '"', indexStart ); // "\" title="
                        chLink = HTML.Substring ( indexStart, indexEnd - indexStart );

                        // Getting chapter number
                        indexStart = indexEnd + 2;
                        indexEnd = HTML.IndexOf ( '<', indexStart ); // "</a>"
                        chNumber = HTML.Substring ( indexStart, indexEnd - indexStart );
                        chNumber = chNumber.Substring ( chNumber.LastIndexOf ( ' ' ) + 1 ); // AfterLast(' ');

                        chNumber = NormalizeChapterNumber ( chNumber );

                        // optional, some chapters don't have title <span>...
                        indexStart = HTML.IndexOf ( '>', indexEnd + 5 ) + 1; // ">"
                        if ( HTML.Substring ( indexStart - 5, 5 ) == "</h3>" )
                        {
                            chTitle = String.Empty;
                        }
                        else
                        {
                            indexEnd = HTML.IndexOf ( '<', indexStart ); // "</span>"
                            chTitle = HTML.Substring ( indexStart, indexEnd - indexStart );
                        }

                        chTitle = HttpUtility.HtmlDecode ( chTitle.Trim ( ) );
                        chLink = chLink.Trim ( );
                        chNumber = HttpUtility.HtmlDecode ( chNumber.Trim ( ) );

                        // Personally don't like the "new" Hakuneko leaves in the chapter titles which causes to re-download when MangaFox decides that the chapter is now "old"
                        chTitle = chTitle == "new" ? String.Empty : chTitle;

                        chapterList.Add ( new MangaChapter
                        {
                            Volume = HttpUtility.HtmlDecode ( volumePrefix ),
                            Manga = Manga,
                            Chapter = chNumber,
                            Link = chLink,
                            Name = chTitle
                        } );
                    }
                }
            }
            catch ( Exception e )
            {
                Logger.Log ( "Error retrieving chapter list: " );
                Logger.Log ( e );
            }

            return chapterList;
        }

        // Shamefully copied from hakuneko (barely modified)
        /// <summary>
        /// Returns the url from all pages of the provided <see cref="MangaChapter"/>
        /// </summary>
        /// <param name="Chapter">The <see cref="MangaChapter"/> to scrap</param>
        /// <returns></returns>
        public async override Task<String[]> GetPageLinksAsync ( MangaChapter Chapter )
        {
            var pageLinks = new List<String> ( );

            try
            {
                var HTML = await HTTP.GetStringAsync ( Chapter.Link );

                var indexStart = HTML.IndexOf ( "<select onchange=\"change_page(this)\" class=\"m\">" ) + 48;
                // ignore last option (comments -> value="0")
                var indexEnd = HTML.IndexOf ( "<option value=\"0\"", indexStart );

                if ( indexStart > 47 && indexEnd >= -1 )
                {
                    HTML = HTML.Substring ( indexStart, indexEnd - indexStart );
                    indexEnd = 0;

                    // Example Entry: <option value="1" selected="selected">1</option>
                    while ( ( indexStart = HTML.IndexOf ( "<option value=\"", indexEnd ) ) > -1 )
                    {
                        indexStart += 15;
                        indexEnd = HTML.IndexOf ( '"', indexStart );

                        pageLinks.Add (
                            await GetImageLinkAsync (
                                Chapter.Link.BeforeLast ( '/' ) + '/' + HTML.Substring ( indexStart, indexEnd - indexStart ) + ".html"
                            )
                        );
                    }
                }
            }
            catch ( Exception e )
            {
                Logger.Log ( "Couldn't retrieve page links:" );
                Logger.Log ( e );
            }

            return pageLinks.ToArray ( );
        }

        // Shamefully copied from hakuneko
        /// <summary>
        /// Returns the URL of the image in the specified page
        /// </summary>
        /// <param name="pageLink">The link of the page to search on</param>
        /// <returns></returns>
        public async Task<String> GetImageLinkAsync ( String pageLink )
        {
            try
            {
                var HTML = await HTTP.GetStringAsync ( pageLink );

                // Example Entry: <img src="http://c.mfcdn.net/store/manga/10235/01-001.0/compressed/pimg001.jpg" onerror="this.src='http://l.mfcdn.net/store/manga/10235/01-001.0/compressed/pimg001.jpg'" width="728" id="image" alt="Tenshin Ranman: Lucky or Unlucky!? 1: Unlucky Nature &amp;amp; Kiss at MangaFox.me"/>
                // use the onerror-server instead the src-server (to save bandwith for web-browsing users which will read from src-server)
                var indexStart = HTML.IndexOf ( "this.src='" ) + 10;
                var indexEnd = HTML.IndexOf ( '\'', indexStart );

                return indexStart > 9 && indexEnd >= -1 ? HTML.Substring ( indexStart, indexEnd - indexStart ) : String.Empty;
            }
            catch ( Exception e )
            {
                Logger.Log ( "Couldn't retrieve image link:" );
                Logger.Log ( e );
            }

            return null;
        }
    }
}