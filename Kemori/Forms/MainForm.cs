using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUtils.Forms;
using Kemori.Base;
using Kemori.Controllers;
using Kemori.Extensions;
using Kemori.Resources;

namespace Kemori.Forms
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The <see cref="MangaConnector"/> list
        /// </summary>
        private MangaConnector[] ConnectorCollection;

        /// <summary>
        /// Current <see cref="MangaConnector"/> being used
        /// </summary>
        private Int32 CurrentConnector;

        /// <summary>
        /// Current <see cref="Manga"/> list being displayed to the user
        /// </summary>
        private Manga[] MangaCollection;

        /// <summary>
        /// Curently selected <see cref="Manga"/>
        /// </summary>
        Int32 CurrentManga;

        /// <summary>
        /// Currently <see cref="MangaChapter"/> list being displayed to the user
        /// </summary>
        MangaChapter[] MangaChapterCollection;

        /// <summary>
        /// Wether the search is a bookmark created by the user
        /// </summary>
        Boolean SearchIsBookmark;

        public MainForm ( )
        {
            InitializeComponent ( );
            new Logger ( ).InitAsync ( );
        }

        #region Event listeners

        /// <summary>
        /// Initializes the whole form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_LoadAsync ( Object sender, EventArgs e )
        {
            await ConfigsManager.LoadAsync ( );

            if ( ConfigsManager.SavePath == null )
                GetSavePathPoignantly ( );

            // Initialize status bar
            ssLoadProgress.Visible = true;
            pbLoadPorgress.Value = 0;
            lblLoadProgress.Text = "Loading connectors...";

            // Load connectors
            await LoadConnectorsAsync ( );

            // Hide status bar
            lblLoadProgress.Text = "Connectors loaded.";
            pbLoadPorgress.Value = 100;

            // Config loading
            try
            {
                await ConfigsManager.LoadAsync ( );
            }
            catch ( Exception )
            {
                // A redundant call so we don't get an "catch shouldn't be empty error"
                SetUIEnabledState ( true );
            }

            UpdateMangaListUI ( );

            ssLoadProgress.Visible = false;
        }

        /// <summary>
        /// A class to compare <see cref="MangaConnector"/>s
        /// </summary>
        private class MCComp : IComparer<MangaConnector>
        {
            public Int32 Compare ( MangaConnector left, MangaConnector right )
            {
                return left.Website.CompareTo ( right.Website );
            }
        }

        /// <summary>
        /// A class to compare <see cref="Manga"/>s
        /// </summary>
        private class MComp : IComparer<Manga>
        {
            public Int32 Compare ( Manga left, Manga right )
            {
                return left.Name.CompareTo ( right.Name );
            }
        }

        /// <summary>
        /// A class to compare <see cref="MangaChapter"/>s in ascending order
        /// </summary>
        private class CCompAsc : IComparer<MangaChapter>
        {
            public Int32 Compare ( MangaChapter x, MangaChapter y )
            {
                return x.Chapter.CompareTo ( y.Chapter );
            }
        }

        /// <summary>
        /// A class to compare <see cref="MangaChapter"/>s in ascending order
        /// </summary>
        private class CCompDesc : IComparer<MangaChapter>
        {
            public Int32 Compare ( MangaChapter x, MangaChapter y )
            {
                // * -1 to reverse the order (descending)
                return x.Chapter.CompareTo ( y.Chapter ) * -1;
            }
        }

        private void dlPathButton_Click ( Object sender, EventArgs e )
        {
            var fsd = new FolderSelectDialog ( );
            if ( !fsd.ShowDialog ( ) )
                return;

            ConfigsManager.SavePath = fsd.FileName;
        }

        /// <summary>
        /// Handles the "select all" operation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chapterSelectAll_CheckedChanged ( Object sender, EventArgs e )
        {
            for ( var i = 0 ; i < chList.Items.Count ; i++ )
                chList.Items[i].Selected = chapterSelectAll.Checked;
        }

        /// <summary>
        /// Re-searches when text on search box changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbSearch_TextChanged ( Object sender, EventArgs e )
        {
            UpdateMangaListUI ( );
            UpdateBookmarkButton ( );
        }

        /// <summary>
        /// Changed the selected connector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbConnectors_SelectedIndexChanged ( Object sender, EventArgs e )
        {
            CurrentConnector = cbConnectors.SelectedIndex;
        }

        private async void mangaList_SelectedIndexChangedAsync ( Object sender, EventArgs e )
        {
            CurrentManga = mangaList.SelectedIndex;
            await UpdateChapterListUIAsync ( );
        }

        /// <summary>
        /// Manages bookmark saving-unsaving
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnBookmark_ClickAsync ( Object sender, EventArgs e )
        {
            var term = GetSearchTerm ( );
            if ( term == "" || MangaCollection.Length < 1 )
                return;

            var books = ConfigsManager.Bookmarks;
            var book = new Bookmark
            {
                ConnectorWebsite = ConnectorCollection[CurrentConnector].Website,
                MangaName = term
            };

            if ( !SearchIsBookmark )
            {
                if ( books.IndexOf ( book ) > -1 )
                {
                    SearchIsBookmark = true;
                    UpdateBookmarkButton ( );
                    btnBookmark_ClickAsync ( sender, e );
                    return;
                }
                books.Add ( book );
            }
            else
            {
                if ( books.IndexOf ( book ) < 0 )
                {
                    SearchIsBookmark = false;
                    UpdateBookmarkButton ( );
                    btnBookmark_ClickAsync ( sender, e );
                    return;
                }

                books.Remove ( book );
            }
            await ConfigsManager.SaveAsync ( );
        }

        #endregion Event listeners

        /// <summary>
        /// Forces the user to enter a save path
        /// </summary>
        private void GetSavePathPoignantly ( )
        {
            var fsd = new FolderSelectDialog
            {
                Title = "Kemori - Choose Save Path"
            };

            while ( !fsd.ShowDialog ( ) )
            {
                var res = MessageBox.Show ( this, "A save path is required for the program to work!", "Kemori - Error", MessageBoxButtons.OKCancel );

                if ( res == DialogResult.Cancel )
                    Application.Exit ( );
            }

            ConfigsManager.SavePath = fsd.FileName;
        }

        void Msg ( Object o ) { MessageBox.Show ( this, "MSG: " + o ); }

        /// <summary>
        /// Loads all connectors into the UI and Form
        /// </summary>
        /// <returns></returns>
        private async Task LoadConnectorsAsync ( )
        {
            // Load connectors
            ConnectorCollection = ( await ConnectorsManager.GetAllAsync ( ) )
                .ToArray ( );
            Msg ( ConnectorCollection.Length );

            // Sort connectors
            pbLoadPorgress.Value = 50;
            Array.Sort ( ConnectorCollection, new MCComp ( ) );
            Msg ( ConnectorCollection.Length );
#if DEBUG
            lblLoadProgress.Text = "Populating connectors combobox...";
#endif
            // Populate connectors combobox
            pbLoadPorgress.Value = 75;
            foreach ( var connector in ConnectorCollection )
                cbConnectors.Items.Add ( connector.Website );

            Msg ( cbConnectors.Items.Count );
        }
        /// <summary>
        /// Returns a <see cref="MangaConnector"/> associated to a <see cref="Bookmark"/>
        /// </summary>
        /// <param name="MangaName">Name of the manga</param>
        /// <param name="ConnectorWebsite">Website of the connector</param>
        /// <returns></returns>
        private MangaConnector FindBookmarkConnector ( String MangaName, String ConnectorWebsite )
        {
            // Get the connector for the bookmark website
            var conn = ConnectorCollection.FirstOrDefault ( con => con.Website == ConnectorWebsite );

            // If we can't even find the connector related to this bookmark then forget it
            if ( conn == null )
                return null;

            // Check that the bookmark exists
            var book = ConfigsManager.Bookmarks
                .FirstOrDefault (
                    // Check for identical website
                    fav => fav.ConnectorWebsite == conn.Website &&
                    // Check for identical manga name
                    conn.MangaList.FirstOrDefault ( m => m.Name == MangaName ) != null
                );

            // Give up if it doesn't
            if ( book == null )
                return null;

            // Return the connector otherwise
            return conn;
        }

        /// <summary>
        /// Returns the appropriate search term entered by the user
        /// </summary>
        /// <returns></returns>
        private String GetSearchTerm ( )
        {
            var searchterm = cbSearch.Text.Trim ( );

            if ( searchterm == "" )
                return "";

            if ( searchterm.IndexOf ( '<' ) > -1 && searchterm.IndexOf ( '>' ) > -1 )
            {
                // It's a bookmark
                searchterm = searchterm.Before ( '<' ).Trim ( );
                var connWebsite = searchterm.After ( '<' ).Before ( '>' );

                var conn = FindBookmarkConnector ( searchterm, connWebsite );
                if ( conn != null )
                {
                    var index = cbConnectors.Items.IndexOf ( conn.Website );
                    cbConnectors.SelectedIndex = index;
                    CurrentConnector = index;
                    SearchIsBookmark = true;
                }
                else
                {
                    SearchIsBookmark = false;
                }
                // else: The cake is a lie
            }
            else
            {
                SearchIsBookmark = false;
            }

            return searchterm;
        }

        /// <summary>
        /// Updated the manga list so it contains the chapters of the selected connector
        /// </summary>
        private void UpdateMangaListUI ( )
        {
            mangaList.Items.Clear ( );

            var term = GetSearchTerm ( );
#if DEBUG
            Console.WriteLine ( $"{ConnectorCollection.Length} | {CurrentConnector}" );
#endif
            var conn = ConnectorCollection[CurrentConnector];

            MangaCollection = conn.MangaList;

            MangaCollection = term != "" ?
                MangaCollection.Where ( man => man.Name.Contains ( term ) ).ToArray ( ) :
                MangaCollection.ToArray ( );

            if ( MangaCollection.Length > 0 )
                Array.Sort ( MangaCollection, new MComp ( ) );

            foreach ( var manga in MangaCollection )
                mangaList.Items.Add ( manga );
        }

        /// <summary>
        /// Updates the chapter list so it contains the chapters of the selected manga
        /// </summary>
        private async Task UpdateChapterListUIAsync ( )
        {
            chList.Items.Clear ( );

            var manga = MangaCollection[CurrentManga];
            await manga.Load ( );

            MangaChapterCollection = manga.Chapters;
            Array.Sort ( MangaChapterCollection, new CCompDesc ( ) );

            foreach ( var chapter in MangaChapterCollection )
            {
                var i = chList.Items.Add ( chapter.ToString ( ) );
                i.ForeColor = chapter.IsDownloaded ? Color.BlueViolet : Color.Black;
            }
        }

        /// <summary>
        /// Manages the updating of the bookmark button text
        /// </summary>
        void UpdateBookmarkButton ( )
        {
            btnBookmark.Text = SearchIsBookmark ? "-" : "+";
        }

        /// <summary>
        /// Sets the "Enabled" state of all controls in the form
        /// </summary>
        /// <param name="state"></param>
        void SetUIEnabledState ( Boolean state )
        {
            cbSearch.Enabled = btnBookmark.Enabled =
                cbConnectors.Enabled = updateAllBtn.Enabled =
                mangaList.Enabled = chapterSelectAll.Enabled =
                chList.Enabled = dlButton.Enabled = state;
        }
    }
}