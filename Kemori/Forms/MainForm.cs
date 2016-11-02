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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUtils.Forms;
using Kemori.Base;
using Kemori.Controllers;
using Kemori.Extensions;
using Kemori.Resources;
using Kemori.Utils;

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
        private Int32 CurrentManga;

        /// <summary>
        /// Currently <see cref="MangaChapter"/> list being displayed to the user
        /// </summary>
        private MangaChapter[] MangaChapterCollection;

        /// <summary>
        /// Wether the search is a bookmark created by the user
        /// </summary>
        private Boolean SearchIsBookmark;

        /// <summary>
        /// General instance of the Logger class
        /// </summary>
        private readonly Logger Logger;

        Int32 chListInitialX,
              mangaListInitialWidth,
              chListInitialWidth;

        public MainForm ( )
        {
            InitializeComponent ( );
            Logger = new Logger ( );

            chListInitialX = chList.Location.X;
            mangaListInitialWidth = mangaList.Width;
            chListInitialWidth = chList.Width;
        }

        #region Event listeners

        /// <summary>
        /// Initializes the whole form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_LoadAsync ( Object sender, EventArgs e )
        {
            Logger.Init ( );
            SetUIEnabledState ( false );

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

            ssLoadProgressSet ( 0, "Loading local manga lists" );

            for ( var i = 0 ; i < ConnectorCollection.Length ; i++ )
            {
                ssLoadProgressSet (
                    Number.GetPercentage ( i, ConnectorCollection.Length ),
                    "Loading local manga lists"
                );
                ConnectorCollection[i].Logger = Logger;
                await ConnectorCollection[i].LoadMangaListFromCacheAsync ( );
            }

            ssLoadProgressSet ( 100, "All set!" );

            UpdateMangaListUI ( );

            chList_Resize ( null, null );

            SetUIEnabledState ( true );
            //ssLoadProgress.Visible = false;
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

        /// <summary>
        /// Loads all connectors into the UI and Form
        /// </summary>
        /// <returns></returns>
        private async Task LoadConnectorsAsync ( )
        {
            await Task.Run ( ( ) =>
              {
                  ssLoadProgressSet ( 0, "Loading connectors" );

                  // Load connectors
                  ConnectorCollection = ( ConnectorsManager.GetAll ( ) )
                        .ToArray ( );

                  // Sort connectors

                  ssLoadProgressSet ( 50, "Sorting connectors" );
                  Array.Sort ( ConnectorCollection, new MCComp ( ) );

                  // Populate connectors combobox
                  ssLoadProgressSet ( 75, "Populating UI with connectors" );

                  cbConnectors.InvokeEx ( cb =>
                  {
                      if ( ConnectorCollection.Count ( ) < 1 )
                          return;

                      foreach ( var connector in ConnectorCollection )
                          cb.Items.Add ( connector.Website );
                      cb.SelectedIndex = 0;
                  } );
              } );
        }

        /// <summary>
        /// Changes the ToolStrip control values thread-safely
        /// </summary>
        /// <param name="Progress">Current progress</param>
        /// <param name="Task">Task being executed</param>
        private void ssLoadProgressSet ( Int32 Progress, String Task )
        {
            ssLoadProgress.InvokeEx ( ss =>
            {
                ( ( ToolStripProgressBar ) ss.Items[nameof ( pbLoadPorgress )] )
                    .Value = Progress;

                ( ( ToolStripLabel ) ss.Items[nameof ( lblLoadProgress )] )
                    .Text = Task;
            } );
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
            await manga.LoadAsync ( );

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
        private void UpdateBookmarkButton ( )
        {
            btnBookmark.Text = SearchIsBookmark ? "-" : "+";
        }

        /// <summary>
        /// Sets the "Enabled" state of all controls in the form
        /// </summary>
        /// <param name="state"></param>
        private void SetUIEnabledState ( Boolean state )
        {
            cbSearch.Enabled = btnBookmark.Enabled =
                cbConnectors.Enabled = updateAllBtn.Enabled =
                mangaList.Enabled = chapterSelectAll.Enabled =
                chList.Enabled = dlButton.Enabled = state;
        }

        private async void updateAllBtn_ClickAsync ( Object sender, EventArgs e )
        {
            var ret = MessageBox.Show ( this, "This operation can take from 5 minutes to *a lot*, are you sure you want to do this?", "Kemori - Are you sure?", MessageBoxButtons.OKCancel );
            if ( ret != DialogResult.OK )
                return;

            SetUIEnabledState ( false );
            ssLoadProgress.Visible = true;

            ssLoadProgressSet ( 0, "Updating local manga lists" );

            for ( var i = 0 ; i < ConnectorCollection.Length ; i++ )
            {
                await ConnectorCollection[i].UpdateMangaListCacheAsync ( );
                ssLoadProgressSet (
                    Number.GetPercentage ( i, ConnectorCollection.Length ),
                    "Updating local manga lists"
                );
            }

            ssLoadProgressSet ( 100, "Local manga lists updated" );

            ssLoadProgress.Visible = false;
            SetUIEnabledState ( true );
        }

        private void chList_Resize ( Object sender, EventArgs e )
        {
            chNameHeader.Width = chList.Width - 2;
        }

        private void MainForm_Resize ( Object sender, EventArgs e )
        {
            var proportion = ( ( Double ) this.Width ) / ( ( Double ) this.MinimumSize.Width );

            chList.Location = new Point ( ( Int32 ) Math.Floor ( chListInitialX * proportion ), chList.Location.Y );
            chapterSelectAll.Location = new Point ( chList.Location.X, chapterSelectAll.Location.Y );

            chList.Width = ( Int32 ) Math.Floor ( chListInitialWidth * proportion );

            mangaList.Width = ( Int32 ) Math.Ceiling ( mangaListInitialWidth * proportion );
        }
    }
}