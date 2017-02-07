// UTF-8 Enforcer: 足の不自由なハッキング
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GUtils.UI.Dialogs;
using Kemori.Base;
using Kemori.Classes;
using Kemori.Controllers;
using Kemori.Resources;
using Kemori.Utils;

using ILoadProgress = System.IProgress<(System.Int32, System.String)>;

namespace Kemori.Forms
{
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        /// The <see cref="MangaConnector" /> list
        /// </summary>
        /// Forms
        private MangaConnector[] ConnectorCollection;

        /// <summary>
        /// Initial width of the chapter list
        /// </summary>
        private readonly Int32 chListInitialWidth;

        /// <summary>
        /// The initial list of the manga list
        /// </summary>
        private readonly Int32 mangaListInitialWidth;

        /// <summary>
        /// Initial X position of the chapter list
        /// </summary>
        private readonly Int32 chListInitialX;

        #endregion Fields

        #region Properties

        public MangaConnector CurrentConnector
        {
            get
            {
                return cbConnectors.SelectedItem != null
                    ? ConnectorCollection[cbConnectors.SelectedIndex]
                    : null;
            }
            set
            {
                cbConnectors.SelectedIndex = Array.IndexOf ( ConnectorCollection, value );
            }
        }

        #endregion Properties

        public MainForm ( )
        {
            InitializeComponent ( );

            chListInitialX = chList.Location.X;
            mangaListInitialWidth = mangaList.Width;
            chListInitialWidth = chList.Width;
        }

        private void ConfigsManager_SavePathChanged ( Object sender, System.ComponentModel.PropertyChangedEventArgs e )
        {
            dlPathTextbox.Text = ConfigsManager.SavePath;
        }

        #region Event listeners

        /// <summary>
        /// Initializes the whole form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_LoadAsync ( Object sender, EventArgs e )
        {
            var PReporter = new LoadProgress ( );
            PReporter.ProgressChanged += ( s, ee ) => SsLoadProgressSet ( ee.Item1, ee.Item2 );

            // Initialize status bar
            ssLoadProgress.Visible = true;

            // Initialize Logger
            Logger.Init ( );

            // Disable UI when loading
            SetUIEnabledState ( false );

            await ConfigsManager.LoadAsync ( PReporter );

            if ( ConfigsManager.SavePath == null )
                GetSavePathPoignantly ( );

            // Load connectors
            await LoadConnectorsAsync ( PReporter );

            PReporter.Report ( (0, "Loading local manga lists") );

            for ( var i = 0 ; i < ConnectorCollection.Length ; i++ )
            {
                PReporter.Report ( (
                    Number.GetPercentage ( i, ConnectorCollection.Length ),
                    "Loading local manga lists"
                ) );

                await ConnectorCollection[i]
                    .LoadMangaListFromCacheAsync ( );
            }

            PReporter.Report ( (100, "All set!") );

            UpdateMangaListUI ( );

            ChList_Resize ( null, null );

            dlPathTextbox.Text = ConfigsManager.SavePath;
            ConfigsManager.SavePathChanged += ConfigsManager_SavePathChanged;

            SetUIEnabledState ( true );
            ssLoadProgress.Visible = false;
        }

        private void DlPathButton_Click ( Object sender, EventArgs e )
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
        private void ChapterSelectAll_CheckedChanged ( Object sender, EventArgs e )
        {
            chList.AllSelected ( chapterSelectAll.Checked );
        }

        /// <summary>
        /// Changed the selected connector
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbConnectors_SelectedIndexChanged ( Object sender, EventArgs e )
        {
            SetUIEnabledState ( false );
            UpdateMangaListUI ( );
            SetUIEnabledState ( true );
        }

        #region Update All Cache List

        /// <summary>
        /// "Update All" Button Click handler: Updates all local manga list
        /// caches after user confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateAllBtn_ClickAsync ( Object sender, EventArgs e )
        {
            var ret = MessageBox.Show ( this, "This operation can take from 5 minutes to *a lot*, are you sure you want to do this?", "Kemori - Are you sure?", MessageBoxButtons.OKCancel );
            if ( ret != DialogResult.OK )
                return;

            SetUIEnabledState ( false );
            ssLoadProgress.Visible = true;

            SsLoadProgressSet ( 0, "Updating local manga lists" );

            for ( var i = 0 ; i < ConnectorCollection.Length ; i++ )
            {
                await ConnectorCollection[i].UpdateMangaListCacheAsync ( );
                SsLoadProgressSet (
                    Number.GetPercentage ( i, ConnectorCollection.Length ),
                    "Updating local manga lists"
                );
            }

            UpdateMangaListUI ( );

            SsLoadProgressSet ( 100, "Local manga lists updated" );

            ssLoadProgress.Visible = false;
            SetUIEnabledState ( true );
        }

        #endregion Update All Cache List

        #region Resizing Handlers

        private void ChList_Resize ( Object sender, EventArgs e )
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

        #endregion Resizing Handlers

        #endregion Event listeners

        #region Save Path Requester

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

        #endregion Save Path Requester

        #region Connector Loader

        /// <summary>
        /// Loads all connectors into the UI and Form
        /// </summary>
        /// <returns></returns>
        private async Task LoadConnectorsAsync ( ILoadProgress P = null )
        {
            await Task.Run ( ( ) =>
            {
                P.Report ( (0, "Loading connectors") );

                // Load connectors
                ConnectorCollection = ( ConnectorsManager.GetAll ( ) )
                      .ToArray ( );

                // Sort connectors

                P.Report ( (50, "Sorting connectors") );
                Array.Sort ( ConnectorCollection, ( MangaConnector x, MangaConnector y ) => x.Website.CompareTo ( y.Website ) );

                // Populate connectors combobox
                P.Report ( (75, "Populating UI with connectors") );

                cbConnectors.InvokeEx ( cb =>
                {
                    if ( ConnectorCollection.Count ( ) < 1 )
                        return;

                    foreach ( var connector in ConnectorCollection )
                        cb.Items.Add ( connector.Website );
                    cb.SelectedIndex = 0;
                } );

                P.Report ( (100, "Connectors loaded") );
            } );
        }

        #endregion Connector Loader

        /// <summary>
        /// Changes the ToolStrip control values thread-safely
        /// </summary>
        /// <param name="Progress">Current progress</param>
        /// <param name="Task">Task being executed</param>
        private void SsLoadProgressSet ( Int32 Progress, String Task )
        {
            ssLoadProgress.InvokeEx ( ss =>
            {
                ( ( ToolStripProgressBar ) ss.Items[nameof ( pbLoadPorgress )] )
                    .Value = Progress;

                ( ( ToolStripLabel ) ss.Items[nameof ( lblLoadProgress )] )
                    .Text = Task;
            } );
        }

        #region UI Updaters

        /// <summary>
        /// Updated the manga list so it contains the chapters of the selected connector
        /// </summary>
        private void UpdateMangaListUI ( )
        {
            var term = "";
            var conn = CurrentConnector;

            if ( conn == null )
                return;

            var col = term != ""
                ? conn.MangaList
                    .Where ( man => man.Name.Contains ( term ) )
                    .ToArray ( )
                : conn.MangaList.ToArray ( );

            mangaList.SetList ( col );
        }

        /// <summary>
        /// Updates the chapter list so it contains the chapters of the selected manga
        /// </summary>
        private async Task UpdateChapterListUIAsync ( )
        {
            var manga = mangaList.SelectedItem;

            if ( manga.Chapters.Length < 1 )
                await manga.LoadAsync ( );

            chList.SetList ( manga.Chapters );
        }

        /// <summary>
        /// Manages the updating of the bookmark button text
        /// </summary>
        private void UpdateBookmarkUI ( )
        {
            bookmarkCB.SetList ( ConfigsManager.Bookmarks );
        }

        /// <summary>
        /// Sets the "Enabled" state of all controls in the form
        /// </summary>
        /// <param name="state"></param>
        private void SetUIEnabledState ( Boolean state )
        {
            cbConnectors.Enabled = updateAllBtn.Enabled =
                mangaList.Enabled = chapterSelectAll.Enabled =
                chList.Enabled = dlButton.Enabled = state;
        }

        #endregion UI Updaters
    }
}
