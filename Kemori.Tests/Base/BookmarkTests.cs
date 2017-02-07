// UTF-8 Enforcer: 足の不自由なハッキング
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kemori.Base.Tests
{
    [TestClass]
    public class BookmarkTests
    {
        [TestMethod]
        public void ToStringTest ( )
        {
            var book = new Bookmark
            {
                ConnectorWebsite = "Website",
                SearchTerm = "Term"
            };

            Assert.AreEqual ( "<Website> Term", book.ToString ( ) );
        }

        [TestMethod]
        public void ParseTest ( )
        {
            var text = "<Website> Term";
            var parsed = Bookmark.Parse ( text );

            Assert.AreEqual ( "Website", parsed.ConnectorWebsite );
            Assert.AreEqual ( "Term", parsed.SearchTerm );
        }

        [TestMethod]
        public void EqualsTest ( )
        {
            var book1 = new Bookmark
            {
                ConnectorWebsite = "Website1",
                SearchTerm = "Term1"
            };
            var book2 = new Bookmark
            {
                ConnectorWebsite = "Website2",
                SearchTerm = "Term2"
            };

            Assert.IsFalse ( book1 == book2 );

            book1.ConnectorWebsite =
                book2.ConnectorWebsite = "Website";
            book1.SearchTerm =
                book2.SearchTerm = "Term";

            Assert.IsTrue ( book1 == ( Object ) book2 );
        }

        [TestMethod]
        public void CompareToTest ( )
        {
            #region Different Websites

            var b1 = new Bookmark
            {
                ConnectorWebsite = "Website1",
                SearchTerm = "Term1"
            };

            var b2 = new Bookmark
            {
                ConnectorWebsite = "Website2",
                SearchTerm = "Term2"
            };

            Assert.AreEqual ( -1, b1.CompareTo ( b2 ) );
            Assert.AreEqual ( 1, b2.CompareTo ( b1 ) );
            Assert.AreEqual ( 0, b1.CompareTo ( b1 ) );

            #endregion Different Websites

            #region Same Websites

            b1.ConnectorWebsite = b2.ConnectorWebsite = "Website";

            Assert.AreEqual ( -1, b1.CompareTo ( b2 ) );
            Assert.AreEqual ( 1, b2.CompareTo ( b1 ) );
            Assert.AreEqual ( 0, b1.CompareTo ( b1 ) );

            #endregion Same Websites
        }

        [TestMethod]
        public void GetHashCodeTest ( )
        {
            var b = new Bookmark
            {
                ConnectorWebsite = "Website",
                SearchTerm = "Term"
            };

            Assert.AreEqual (
                $"<{b.ConnectorWebsite}> {b.SearchTerm}".GetHashCode ( ),
                b.GetHashCode ( ) );
        }
    }
}
