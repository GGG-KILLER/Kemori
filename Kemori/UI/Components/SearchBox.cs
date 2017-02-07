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

namespace Kemori.UI.Components
{
    public delegate void SearchTermChangedEventHandler ( SearchBox sender, String term );

    public partial class SearchBox : ComboBox
    {
        private Boolean Typing;
        private Timer KeystrokeTimer;

        /// <summary>
        /// Dictates the tipying speed of the user (amount of miliseconds the
        /// user takes to press another key)
        /// </summary>
        public Int32 KeystrokeInterval
        {
            get => KeystrokeTimer.Interval;
            set => KeystrokeTimer.Interval = value;
        }

        #region Search Term

        protected String _searchTerm;

        public String SearchTerm
        {
            get
            {
                return GetSearchTerm ( );
            }
            set
            {
                SetSearchTerm ( value );
            }
        }

        protected virtual void SetSearchTerm ( String value )
        {
            _searchTerm = value;
        }

        protected virtual String GetSearchTerm ( )
        {
            return _searchTerm;
        }

        #endregion Search Term

        /// <summary>
        /// Event triggered when the search term has changed
        /// </summary>
        public event SearchTermChangedEventHandler SearchTermChanged;

        public SearchBox ( )
        {
            InitializeComponent ( );
            KeystrokeTimer = new Timer
            {
                Enabled = false,
                Interval = 600
            };
            KeystrokeTimer.Tick += T_Tick;
        }

        public SearchBox ( IContainer container ) : this ( )
        {
            container.Add ( this );
        }

        /// <summary>
        /// Sets the list of pre-made options the user can choose from
        /// </summary>
        /// <param name="books"></param>
        public void SetItemList ( IEnumerable<String> books )
        {
            this.BeginUpdate ( );

            Items.Clear ( );
            Items.AddRange ( books.ToArray ( ) );

            this.EndUpdate ( );
        }

        // The search term will only ever be updated when the timer stops
        // running. The timer also only stops running after a tick.
        private void T_Tick ( Object sender, EventArgs e )
        {
            OnSearchTermChanged ( this.Text );

            KeystrokeTimer.Stop ( );
            Typing = false;
        }

        /// <summary>
        /// Raises the <see cref="SearchBox" />.KeyUp event.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnKeyUp ( KeyEventArgs e )
        {
            if ( !Typing ) Typing = true;
            KeystrokeTimer.Stop ( );
            KeystrokeTimer.Start ( );

            base.OnKeyUp ( e );
        }

        /// <summary>
        /// Raises the <see cref="SearchBox" />.SearchTermChanged event.
        /// </summary>
        /// <param name="term"></param>
        protected virtual void OnSearchTermChanged ( String term )
        {
            SearchTerm = term;
            SearchTermChanged?.Invoke ( this, SearchTerm );
        }
    }
}
