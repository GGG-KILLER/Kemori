// UTF-8 Enforcer: 足の不自由なハッキング
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Kemori.Base;

namespace Kemori.UI.Components
{
    public partial class BookmarkComboBox : SearchBox
    {
        public delegate void SelectedBookmarkChangedHandler ( BookmarkComboBox sender, Bookmark bookmark );

        private Bookmark[] Bookmarks;

        public event SelectedBookmarkChangedHandler SelectedBookmarkChanged;

        public new Bookmark SelectedItem
        {
            get
            {
                try
                {
                    return Bookmarks[SelectedIndex];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                SelectedIndex = Array.IndexOf ( Bookmarks, value );
            }
        }

        public Bookmark SelectedBookmark { get; private set; }

        public BookmarkComboBox ( )
        {
            InitializeComponent ( );
        }

        public BookmarkComboBox ( IContainer container ) : this ( )
        {
            container.Add ( this );
        }

        public void SetList ( IEnumerable<Bookmark> Bookmarks )
        {
            this.Bookmarks = Bookmarks.ToArray ( );
            base.SetItemList ( Bookmarks.Select ( book =>
                  $"<{book.ConnectorWebsite}> {book.SearchTerm}" ) );
        }

        protected new virtual void OnSearchTermChanged ( String term )
        {
            Bookmark bookmark;
            if ( ( bookmark = GetBookmark ( term ) ) != null )
            {
                SelectedBookmark = bookmark;
                term = bookmark.SearchTerm;

                OnSelectedBookmarkChanged ( bookmark );
            }
            else
                SelectedBookmark = null;

            base.OnSearchTermChanged ( term );
        }

        protected new virtual void OnSelectedIndexChanged ( EventArgs e )
        {
            OnSelectedBookmarkChanged ( SelectedBookmark );
            OnSearchTermChanged ( SelectedBookmark.SearchTerm );
            base.OnSelectedIndexChanged ( e );
        }

        private Bookmark GetBookmark ( String fullterm )
        {
            if ( Bookmarks.Length < 1 )
                return null;

            var lt = fullterm.IndexOf('<');
            var gt = fullterm.IndexOf('>');

            if ( lt > -1 && gt > -1 )
            {
                var website = fullterm.Substring ( lt + 1, gt - lt );
                var term = fullterm.Substring ( gt + 1 ).Trim ( );

                return Bookmarks.FirstOrDefault ( book =>
                    book.ConnectorWebsite == website &&
                        book.SearchTerm == term );
            }
            else return null;
        }

        protected virtual void OnSelectedBookmarkChanged ( Bookmark bookmark )
        {
            SelectedBookmarkChanged?.Invoke ( this, bookmark );
        }
    }
}
