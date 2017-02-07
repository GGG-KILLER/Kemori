// UTF-8 Enforcer: 足の不自由なハッキング
using System.IO;
using Kemori.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kemori.Classes.Tests
{
    [TestClass]
    public class IOTests
    {
        [TestMethod ( )]
        public void IOTest ( )
        {
            Assert.AreEqual ( new IO ( "." ).Root, Path.GetFullPath ( "." ) );
        }

        [TestMethod ( )]
        public void PathForMangaTest ( )
        {
            ConfigsManager.SavePath = Path.Combine ( "C:", "Mangas" );
            var io = new IO ( "." );
            var path = io.PathForManga ( new Base.Manga
            {
                Name = "Manga"
            } );

            Assert.AreEqual ( Path.Combine ( "C:", "Mangas", "Manga" ), path );
        }

        [TestMethod ( )]
        public void PathForMangaTest1 ( )
        {
            ConfigsManager.SavePath = Path.Combine ( "C:", "Mangas" );
            var io = new IO ( "." );

            Assert.AreEqual ( Path.Combine ( "C:", "Mangas", "Manga" ),
                io.PathForManga ( "Manga" ) );
        }

        [TestMethod ( )]
        public void PathForChapterTest ( )
        {
            ConfigsManager.SavePath = Path.Combine ( "C:", "Mangas" );
            var io = new IO ( "." );

            Assert.AreEqual (
                Path.Combine ( "C:", "Mangas", "Manga", "[] - 1 - Chapter" ),
                io.PathForChapter ( new Base.MangaChapter
                {
                    Chapter = "1",
                    Name = "Chapter",
                    Volume = "",
                    Manga = new Base.Manga
                    {
                        Name = "Manga"
                    }
                } ) );
        }

        [TestMethod ( )]
        public void PathForChapterTest1 ( )
        {
            ConfigsManager.SavePath = Path.Combine ( "C:", "Mangas" );
            var io = new IO ( "." );

            Assert.AreEqual ( Path.Combine ( "C:", "Mangas", "Manga", "Chapter" ),
                io.PathForChapter ( "Manga", "Chapter" ) );
        }

        [TestMethod ( )]
        public void SafeFolderNameTest ( )
        {
            var io = new IO ( "." );
            var bc = Path.GetInvalidPathChars ( );
            Assert.AreEqual ( "folder",
                io.SafeFolderName ( $"f{bc[0]}o{bc[1]}l{bc[2]}d{bc[3]}er" ) );
        }

        [TestMethod ( )]
        public void SafeFileNameTest ( )
        {
            var io = new IO ( "." );
            var bc = Path.GetInvalidFileNameChars ( );
            Assert.AreEqual ( "filename.exe",
                io.SafeFileName ( $"file{bc[0]}name{bc[1]}.{bc[2]}exe" ) );
        }
    }
}
