using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Extensions;
using Kemori.Interfaces;

namespace Kemori.Connectors
{
    public class MangaFoxConnector : MangaConnector
    {
        public override event MangaDownloadProgressChangedHandler MangaDownloadProgressChanged;

        public override void InitHTTP ( )
        {
            Console.WriteLine ( "[MangaFoxConnector]InitHTTP Called" );
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

        private void HTTP_DownloadProgressChanged ( Object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            this.MangaDownloadProgressChanged?.Invoke ( this, e );
        }

        // Shamefully copied from hakuneko
        public override async Task<IEnumerable<Manga>> UpdateMangaListAsync ( )
        {
            var mangaList = new List<Manga> ( );
            try
            {
                var HTML = ( await HTTP.GetStringAsync ( "http://mangafox.me/manga/" ) );
                var indexStart = HTML.IndexOfAfter ( "<div class=\"manga_list\">" );
                var indexEnd = HTML.IndexOf ( "<div id=\"footer\">" );

                // Only continue if we got the right html
                if ( indexStart > 23 && indexEnd >= -1 )
                {
                    HTML = HTML.Substring ( indexStart, indexEnd - indexStart );
                    indexEnd = 0;

                    // Example Entry: <li><a href="http://mangafox.me/manga/name/" rel="8894" class="series_preview manga_open">Label</a></li>
                    while ( ( indexStart = HTML.IndexOfAfter ( "<li><a href=\"", indexEnd ) ) > 12 )
                    {
                        indexEnd = HTML.IndexOf ( '"', indexStart );
                        var mangaLink = HTML.Substring ( indexStart, indexEnd - indexStart );

                        indexStart = HTML.IndexOfAfter ( '>', indexEnd );
                        indexEnd = HTML.IndexOf ( '<', indexStart );
                        var mangaLabel = HTML.Substring ( indexStart, indexEnd - indexStart );

                        if ( mangaLabel != "" )
                        {
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
                await Logger.LogAsync ( "Error retrieving manga list: " );
                await Logger.LogAsync ( e );
            }

            return mangaList;
        }

        // Shamefully copied from Hakuneko
        public async override Task<IEnumerable<MangaChapter>> GetChaptersAsync ( Manga Manga )
        {
            var chapterList = new List<MangaChapter> ( );

            try
            {
                var HTML = await HTTP.GetStringAsync ( Manga.Link );
                var indexStart = HTML.IndexOfAfter ( "</h2><hr/>" );
                var indexEnd = HTML.IndexOf ( "<div id=\"discussion\"", indexStart );
                var volumeIndexCurrent = 0;
                var volumeIndexNext = 0;

                String volumePrefix = String.Empty, chNumber, chTitle, chLink;

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
                        }

                        // Getting link of chapter
                        indexStart += 9; // "href=\""
                        indexEnd = HTML.IndexOf ( '"', indexStart ); // "\" title="
                        chLink = HTML.Substring ( indexStart, indexEnd - indexStart );

                        // Getting chapter number
                        indexStart = indexEnd + 2;
                        indexEnd = HTML.IndexOf ( '<', indexStart ); // "</a>"
                        chNumber = HTML.Substring ( indexStart, indexEnd - indexStart )
                            .AfterLast ( ' ' );

                        chNumber = NormalizeChapterNumber ( chNumber );

                        // optional, some chapters don't have title <span>...
                        indexStart = HTML.IndexOfAfter ( '>', indexEnd + 5 ); // ">"
                        if ( HTML.Substring ( indexStart - 5, indexStart ) == "</h3>" )
                        {
                            chTitle = String.Empty;
                        }
                        else
                        {
                            indexEnd = HTML.IndexOf ( '<', indexStart ); // "</span>"
                            chTitle = HTML.Substring ( indexStart, indexEnd - indexStart );
                        }

                        chTitle = chTitle.Trim ( );
                        chLink = chLink.Trim ( );
                        chNumber = chNumber.Trim ( );

                        // Personally don't like the "new" Hakuneko leaves in the chapter titles which causes to re-download when MangaFox decides that the chapter is now "old"
                        chTitle = chTitle == "new" ? String.Empty : chTitle;

                        chapterList.Add ( new MangaChapter
                        {
                            Volume = volumePrefix,
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
                await Logger.LogAsync ( "Error retrieving chapter list: " );
                await Logger.LogAsync ( e );
            }

            return chapterList;
        }

        // Shamefully copied from hakuneko (barely modified)
        public async override Task<String[]> GetPageLinksAsync ( MangaChapter Chapter )
        {
            var pageLinks = new List<String> ( );

            try
            {
                var HTML = await HTTP.GetStringAsync ( Chapter.Link );

                var indexStart = HTML.IndexOfAfter ( "<select onchange=\"change_page(this)\" class=\"m\">" );
                // ignore last option (comments -> value="0")
                var indexEnd = HTML.IndexOf ( "<option value=\"0\"", indexStart );

                if ( indexStart > 47 && indexEnd >= -1 )
                {
                    HTML = HTML.Substring ( indexStart, indexEnd - indexStart );

                    // Example Entry: <option value="1" selected="selected">1</option>
                    while ( ( indexStart = HTML.IndexOf ( "<option value=\"", indexEnd ) ) > -1 )
                    {
                        indexStart += 15;
                        indexEnd = HTML.IndexOf ( '"', indexStart );

                        pageLinks.Add (
                            await GetImageLinkAsync (
                                HTML.Substring ( indexStart, indexEnd - indexStart )
                            )
                        );
                    }
                }
            }
            catch ( Exception e )
            {
                await Logger.LogAsync ( "Couldn't retrieve page links:" );
                await Logger.LogAsync ( e );
            }

            return pageLinks.ToArray ( );
        }

        // Shamefully copied from hakuneko
        public async Task<String> GetImageLinkAsync ( String pageLink )
        {
            try
            {
                var HTML = await HTTP.GetStringAsync ( pageLink );
                // Example Entry: <img src="http://c.mfcdn.net/store/manga/10235/01-001.0/compressed/pimg001.jpg" onerror="this.src='http://l.mfcdn.net/store/manga/10235/01-001.0/compressed/pimg001.jpg'" width="728" id="image" alt="Tenshin Ranman: Lucky or Unlucky!? 1: Unlucky Nature &amp;amp; Kiss at MangaFox.me"/>
                // use the onerror-server instead the src-server (to save bandwith for web-browsing users which will read from src-server)
                var indexStart = HTML.IndexOfAfter ( "this.src='" );
                var indexEnd = HTML.IndexOf ( '\'', indexStart );

                if ( indexStart > 9 && indexEnd >= -1 )
                {
                    return HTML.Substring ( indexStart, indexEnd - indexStart );
                }
            }
            catch ( Exception e )
            {
                await Logger.LogAsync ( "Couldn't retrieve image link:" );
                await Logger.LogAsync ( e );
            }

            return null;
        }
    }
}