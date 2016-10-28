using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Kemori
{
    public class Logger
    {
        private readonly FileInfo LogFile;

        public Logger ( )
        {
            LogFile = new FileInfo (
                Path.Combine (
                    Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ),
                    "kemori.log"
                )
            );
        }

        public async Task InitAsync ( )
        {
            // 2 MB size limit
            if ( LogFile.Exists && LogFile.Length > 2 * 1024 * 1024 )
            {
                LogFile.Delete ( );
            }

            var b = new String ( '=', 24 );
            await this.LogAsync ( String.Empty );
            await this.LogAsync ( b );
            await this.LogAsync ( $"=== {( DateTime.Now.ToString ( "%Y-%m-%dT%H:%M:%S" ) )} +0000 ===" );
            await this.LogAsync ( b );
            await this.LogAsync ( String.Empty );
        }

        public async Task LogAsync ( Object item )
        {
            using ( var log = new StreamWriter ( LogFile.FullName, true ) )
            {
                await log.WriteLineAsync ( item.ToString ( ) );
                await log.FlushAsync ( );
            }
        }
    }
}