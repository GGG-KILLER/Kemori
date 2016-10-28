using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Extensions;
using Kemori.Interfaces;

namespace Kemori.MangaFox
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
        /// <param name="Chapter"></param>
        public override async Task DownloadChapter ( MangaChapter Chapter )
        {
            Chapter.Load ( );

            HTTP.DownloadProgressChanged += HTTP_DownloadProgressChanged;
            var bytes = await HTTP.GetDataAsync ( Chapter.Link );
            HTTP.DownloadProgressChanged -= HTTP_DownloadProgressChanged;
        }

        private void HTTP_DownloadProgressChanged ( Object sender, System.Net.DownloadProgressChangedEventArgs e )
        {
            this.MangaDownloadProgressChanged?
                .Invoke ( this, new MangaDownloadProgressChangedArgs
                {
                    CurrentBytes = e.BytesReceived,
                    TotalBytes = e.TotalBytesToReceive
                } );
        }

        // Shamefully copied from hakuneko
        public override async Task<IEnumerable<Manga>> UpdateMangaList ( )
        {
            var mangaList = new List<Manga> ( );
            try
            {
                var HTML = ( await HTTP.GetStringAsync ( "http://mangafox.me/manga/" ) );
                var indexStart = HTML.IndexOfAfter ( "<div class=\"manga_list\">" );
                var indexEnd = HTML.IndexOf ( "<div id=\"footer\">" );

                if ( indexStart > 23 && indexEnd >= -1 )
                {
                    HTML = HTML.Substring ( indexStart, indexEnd - indexStart );
                    indexEnd = 0;

                    // length of string is 13, so
                    // it has to be bigger than 12 because not finding would be -1 + 13 = 12
                    // Example Entry: <li><a href="http://mangafox.me/manga/name/" rel="8894" class="series_preview manga_open">Label</a></li>
                    while ( ( indexStart = HTML.IndexOfAfter ( "<li><a href=\"", indexStart ) ) > 12 )
                    {
                        indexEnd = HTML.IndexOf ( '"', indexStart );
                        var mangaLink = HTML.Substring ( indexStart, indexEnd - indexStart );


                        indexStart = HTML.IndexOfAfter ( '>', indexEnd );
                        indexEnd = HTML.IndexOf ( '<', indexStart );
                        var mangaLabel = HTML.Substring ( indexStart, indexEnd - indexStart );

                        if ( mangaLabel != "" )
                        {
                            var m = new Manga
                            {
                                Name = mangaLabel,
                                Link = mangaLink,
                                Connector = this
                            };
                            m.Path = IO.PathForManga ( m );
                            mangaList.Add ( m );
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
        public async override Task<IEnumerable<MangaChapter>> GetChapters ( Manga Manga )
        {
            var HTML = await HTTP.GetStringAsync ( Manga.Link );

            return new MangaChapter[0];
        }
    }
}