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
        /// Event triggered when the search term has changed
        /// </summary>
        public event SearchTermChangedEventHandler SearchTermChanged;

        public SearchBox ( )
        {
            InitializeComponent ( );
            KeystrokeTimer = new Timer
            {
                Enabled = false,
                Interval = 750
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
            Items.Clear ( );
            Items.AddRange ( books.ToArray ( ) );
        }

        // The search term will only ever be updated when the timer stops running. The timer also
        // only stops running after a tick.
        private void T_Tick ( object sender, EventArgs e )
        {
            OnSearchTermChanged ( this.Text );
            KeystrokeTimer.Stop ( );
            Typing = false;
        }

        protected override void OnKeyUp ( KeyEventArgs e )
        {
            base.OnKeyUp ( e );

            if ( !Typing ) Typing = true;
            KeystrokeTimer.Start ( );
        }

        // This triggers the SearchTermChanged event
        protected void OnSearchTermChanged ( String term )
        {
            SearchTermChanged?.Invoke ( this, term );
        }
    }
}
