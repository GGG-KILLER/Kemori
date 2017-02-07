// UTF-8 Enforcer: 足の不自由なハッキング
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kemori.Base.Tests
{
    internal class TestMangaConnector : MangaConnector
    {
        public TestMangaConnector ( )
        {
            this.ID = "TestMangaConnector";
            this.Website = "Example";
        }

        // Task.FromResult shorthand
        public Task<T> GT<T> ( T v )
        {
            return Task.FromResult ( v );
        }

        public Manga GetManga ( Int32 N = 1 )
        {
            Manga M = new Manga
            {
                Connector = this,
                Link = "https://example.com/manga" + N,
                Name = "Manga " + N,
                Path = "/Mangas/Manga " + N
            };
            // Generate the mock chapters
            M.Chapters = Enumerable.Range ( 1, 10 )
                .Select ( n => GetChapter ( n, M ) )
                .ToArray ( );

            return M;
        }

        public MangaChapter GetChapter ( Int32 N = 1, Manga M = null )
        {
            M = M ?? GetManga ( );
            return new MangaChapter
            {
                Chapter = N.ToString ( ),
                Link = $"/Mangas/{M.Name}/Chapter {N}",
                Manga = M,
                Name = "Chapter " + N,
                Volume = ""
            };
        }

        public override Task<IEnumerable<MangaChapter>> GetChaptersAsync ( Manga Manga )
        {
            return GT (
                Enumerable.Range ( 1, 10 )
                    .Select ( num => GetChapter ( num, Manga ) ) );
        }

        public override Task<String> GetImageLinkAsync ( String pageLink )
        {
            return GT ( $"{pageLink}.jpg" );
        }

        public override Task<String[]> GetPageLinksAsync ( MangaChapter Chapter )
        {
            return GT ( Enumerable.Range ( 1, 10 )
                .Select ( num => $"https://example.com/{Chapter.Manga.Name}/{Chapter.Chapter}" )
                .ToArray ( ) );
        }

        public override void InitHTTP ( )
        {
            return;
            // Do nothing
        }

        public override Task<IEnumerable<Manga>> UpdateMangaListAsync ( )
        {
            return GT ( Enumerable.Range ( 1, 10 )
                .Select ( num => GetManga ( num ) ) );
        }
    }
}
