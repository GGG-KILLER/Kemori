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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Kemori.Base;

namespace Kemori.UI.Components
{
    // We won't take any measures to protect aganinst modifying Items as the
    // application itself won't be even touching it
    public partial class MangaListBox : ListBox
    {
        /// <summary>
        /// Manga List
        /// </summary>
        private IList<Manga> _mangaList;

        private MangaConnector _connector;

        /// <summary>
        /// The connector being used with this <see cref="MangaListBox"/>
        /// </summary>
        public MangaConnector Connector
        {
            get => _connector;
            set => SetConnector ( value );
        }

        private void SetConnector ( MangaConnector value )
        {
            _connector = value;
            if ( value == null )
                return;
        }

        public MangaListBox ( )
        {
            InitializeComponent ( );

            SetStyle ( ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true );

            this._mangaList = new List<Manga> ( );
        }

        public MangaListBox ( IContainer container ) : this ( )
        {
            container.Add ( this );
        }

        public void SetList ( IEnumerable<Manga> List )
        {
            this.BeginUpdate ( );

            List = List.OrderBy ( x => x.Name );

            this._mangaList.Clear ( );
            this.Items.Clear ( );
            foreach ( var Item in List )
            {
                Add ( Item );
            }

            this.EndUpdate ( );
        }

        public void Add ( Manga Item )
        {
            this.Items.Add ( Item.Name ); // For rendering purposes
            this._mangaList.Add ( Item );
        }

        public Boolean Contains ( Manga Item )
        {
            return this._mangaList.Contains ( Item );
        }

        public void Remove ( Manga Item )
        {
            this.Items.Remove ( Item.Name ); // For rendering purposes
            this._mangaList.Remove ( Item );
        }

        public new Manga SelectedItem
        {
            get
            {
                return ( Manga ) base.SelectedItem;
            }
            set
            {
                base.SelectedItem = value;
            }
        }
    }
}
