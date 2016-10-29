using System;
using System.Windows.Forms;
using Kemori.Forms;

namespace Kemori
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main ( )
        {
            Application.EnableVisualStyles ( );
            Application.SetCompatibleTextRenderingDefault ( false );
            using ( var mainForm = new MainForm ( ) )
            {
                Application.Run ( mainForm );
            }
        }
    }
}