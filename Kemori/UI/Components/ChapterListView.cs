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
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Kemori.Base;

namespace Kemori.UI.Components
{
    public delegate void ChapterCheckedEventHandler ( ChapterListView sender, MangaChapter item );

    public partial class ChapterListView : ListView
    {
        private IDictionary<MangaChapter, ListViewItem> ChapterList;

        public event ChapterCheckedEventHandler ChapterChecked;

        public ChapterListView ( )
        {
            InitializeComponent ( );
            SetStyle ( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true );

            this.CheckBoxes = true;
            this.LabelWrap = false;
            this.HeaderStyle = ColumnHeaderStyle.None;
            this.MultiSelect = false;
            this.UseCompatibleStateImageBehavior = false;
            this.View = View.Details;

            this.ChapterList = new Dictionary<MangaChapter, ListViewItem> ( );
        }

        protected override void OnItemChecked ( ItemCheckedEventArgs e )
        {
            base.OnItemChecked ( e );

            foreach ( var chapter in ChapterList )
            {
                if ( chapter.Key.ToString ( ) == e.Item.Text )
                {
                    this.ChapterChecked?.Invoke ( this, chapter.Key );
                }
            }
        }

        public ChapterListView ( IContainer container ) : base ( )
        {
            container.Add ( this );
        }

        public void Add ( MangaChapter Item )
        {
            var lvi = this.Items.Add ( Item.ToString ( ) );
            lvi.ForeColor = Item.IsDownloaded ? System.Drawing.Color.LightBlue : System.Drawing.Color.Black;

            this.ChapterList.Add ( Item, lvi );
        }

        public Boolean Contains ( MangaChapter Item )
        {
            return this.ChapterList.ContainsKey ( Item );
        }

        public void Remove ( MangaChapter Item )
        {
            this.Items.Remove ( this.ChapterList[Item] );
            this.ChapterList.Remove ( Item );
        }

        public void SetList ( IEnumerable<MangaChapter> List )
        {
            this.BeginUpdate ( );

            List = List.OrderByDescending ( x => x.Chapter );

            this.ChapterList.Clear ( );
            this.Items.Clear ( );
            foreach ( var Item in List )
            {
                Add ( Item );
            }

            this.EndUpdate ( );
        }
    }
}
