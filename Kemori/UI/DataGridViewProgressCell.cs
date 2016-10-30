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

namespace GUtils.Forms.DataGridView
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    // Modified from:
    // https://social.msdn.microsoft.com/Forums/windows/en-US/769ca9d6-1e9d-4d76-8c23-db535b2f19c2/sample-code-datagridview-progress-bar-column?forum=winformsdatacontrols
    // Disclaimer: I did not make this, all credits goes to it's original author(s), I only take credit for the modifications I made on it:
    // - Add usings
    // - Static color field
    // - Removed unused backColorBrush
    // - Left a single g.DrawString call
    // - Removed the "else" from the if(progressVal > 0.0)

    internal class DataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        private static readonly Image emptyImage;

        // Create the Color only once to avoid having to re-create it all the time (and for each instance)
        private static readonly Color progressBarColor = Color.FromArgb ( 203, 235, 108 );

        static DataGridViewProgressCell ( )
        {
            emptyImage = new Bitmap ( 1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb );
        }

        public DataGridViewProgressCell ( )
        {
            this.ValueType = typeof ( Int32 );
        }

        // Method required to make the Progress Cell consistent with the default Image Cell.
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override Object GetFormattedValue ( Object value, Int32 rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context )
        {
            return emptyImage;
        }

        protected override void Paint ( Graphics g, Rectangle clipBounds, Rectangle cellBounds, Int32 rowIndex, DataGridViewElementStates cellState, Object value, Object formattedValue, String errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts )
        {
            try
            {
                var progressVal = ( Int32 ) value;
                var percentage = progressVal / 100.0f; // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
                using ( Brush foreColorBrush = new SolidBrush ( cellStyle.ForeColor ) )
                {
                    // Draws the cell grid
                    base.Paint ( g, clipBounds, cellBounds,
                     rowIndex, cellState, value, formattedValue, errorText,
                     cellStyle, advancedBorderStyle, ( paintParts & ~DataGridViewPaintParts.ContentForeground ) );

                    // Draw the progress bar
                    if ( percentage > 0.0 )
                        using ( var solidBrush = new SolidBrush ( Color.FromArgb ( 203, 235, 108 ) ) )
                        {
                            // Draw the progress bar and the text
                            g.FillRectangle ( solidBrush, cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32 ( ( percentage * cellBounds.Width - 4 ) ), cellBounds.Height - 4 );
                        }

                    g.DrawString ( progressVal + "%", cellStyle.Font, foreColorBrush, cellBounds.X + ( cellBounds.Width / 2 ) - 5, cellBounds.Y + 2 );
                }
            }
#pragma warning disable CC0004 // Catch block cannot be empty
            catch ( Exception ) { }
#pragma warning restore CC0004 // Catch block cannot be empty
        }
    }
}