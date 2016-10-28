using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kemori.Forms
{
    public partial class MainForm : Form
    {
        Boolean chapterSelectAllCurrent;

        public MainForm ( )
        {
            InitializeComponent ( );
        }

        private void MainForm_Load ( Object sender, EventArgs e )
        {
            mangaLabel.Text = downloadProgressLabel.Text = downloadProgressLabel.Text = "";
            downloadProgressBar.Value = 0;
        }

        /// <summary>
        /// Converts from bytes to megabytes (precision of 2)
        /// </summary>
        /// <param name="Bytes">Number of bytes</param>
        /// <returns></returns>
        private static Double LongToMB ( Int64 Bytes )
        {
            //                    B  ->   KB   ->  MB
            return Math.Round ( ( Bytes / 1024D / 1024D ) * 100 ) / 100;
            // Math.Round also seems to think I don't want
            // the double precision floating numbers
            // so the multiplication and division is necessary to
            // preserve them
        }

        /// <summary>
        /// Returns the number making sure it's 5 characters (including padding 0's)
        /// </summary>
        /// <param name="Num"></param>
        /// <returns></returns>
        private static String DoublePrecisionString ( Double Num )
        {
            // I hope no one will use this to download more than 9.99999... GB
            return Num.ToString ( "##00.00" );
        }

        private void dlPathButton_Click ( Object sender, EventArgs e )
        {

        }

        private void chapterSelectAll_CheckedChanged ( Object sender, EventArgs e )
        {
            if( !chapterSelectAllCurrent )
            {
                for ( var i = 0 ; i < chapterList.Items.Count ; i++ )
                    chapterList.SetSelected ( i, true );
                chapterSelectAllCurrent = true;
            }
            else
            {
                chapterList.ClearSelected ( );
                chapterSelectAllCurrent = false;
            }
        }
    }
}
