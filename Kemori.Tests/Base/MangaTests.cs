// UTF-8 Enforcer: 足の不自由なハッキング
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kemori.Base.Tests
{
    [TestClass ( )]
    public class MangaTests
    {
        private static TestMangaConnector C = new TestMangaConnector ( );

        [TestMethod ( )]
        public void ReCalcInstanceIDTest ( )
        {
            var M = C.GetManga ( );

            Assert.AreEqual ( "TestMangaConnectorExampleManga 1https://example.com/manga1"
                .GetHashCode ( ).ToString ( ), M.InstanceID );
        }

        [TestMethod ( )]
        public void ToStringTest ( )
        {
            Assert.AreEqual ( "Manga 1", C.GetManga ( ) );
        }

        [TestMethod ( )]
        public void EqualsTest ( )
        {
            var M1 = C.GetManga ( );
            var M2 = C.GetManga ( );

            Assert.AreEqual ( M1, M2 );
        }

        [TestMethod ( )]
        public void EqualsTest1 ( )
        {
            var M1 = C.GetManga ( );
            var M2 = C.GetManga ( );

            Assert.IsTrue ( M1.Equals ( ( Object ) M2 ) );
        }

        [TestMethod ( )]
        public void CompareToTest ( )
        {
            var M1 = C.GetManga ( 1 );
            var M2 = C.GetManga ( 2 );

            Assert.AreEqual ( M1.CompareTo ( M2 ), -1 );
            Assert.AreEqual ( M2.CompareTo ( M1 ), 1 );
            Assert.AreEqual ( M1.CompareTo ( M1 ), 0 );
        }

        [TestMethod ( )]
        public void GetHashCodeTest ( )
        {
            var M1 = C.GetManga ( );

            Assert.AreEqual ( M1.InstanceID.GetHashCode ( ), M1.GetHashCode ( ) );
        }
    }
}
