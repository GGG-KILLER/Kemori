using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Kemori.Base;

namespace Kemori.Resources
{
    internal class ConfigsManager
    {
        private static readonly FileInfo ConfigPath;
        private static Configs Config;
        private static DateTime LastSaved, LastChanged;

        static ConfigsManager ( )
        {
            ConfigPath = new FileInfo (
                Path.Combine (
                    Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ),
                    "kemori.conf"
                )
            );
        }

        public static String SavePath
        {
            set
            {
                LastChanged = DateTime.Now;
                Config.SavePath = value;
                Save ( );
            }
            get
            {
                return Config.SavePath;
            }
        }

        public static List<Favorite> Favorites
        {
            get
            {
                Save ( );
                return Config.Favorites;
            }
        }

        public static void Save ( )
        {
            LastChanged = DateTime.Now;
            LastSaved = DateTime.Now;
            using ( var stream = ConfigPath.Open ( FileMode.OpenOrCreate ) )
                new BinaryFormatter ( )
                    .Serialize ( stream, Config );
        }

        public static void Load ( )
        {
            if ( LastChanged != null && LastSaved != null && LastChanged < LastSaved )
                return;

            try
            {
                using ( var stream = ConfigPath.Open ( FileMode.Open ) )
                    Config = ( Configs ) new BinaryFormatter ( )
                        .Deserialize ( stream );
            }
            catch ( Exception )
            {
                Config = null;
            }
        }
    }

    [Serializable]
    internal class Configs
    {
        internal String SavePath;
        internal List<Favorite> Favorites;
    }
}