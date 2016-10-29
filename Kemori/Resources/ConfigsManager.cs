using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Utils;

namespace Kemori.Resources
{
    internal class ConfigsManager
    {
        private static readonly String ConfigPath;
        private static Configs Config;

        /// <summary>
        /// Initializes the manager needs to operate
        /// </summary>
        static ConfigsManager ( )
        {
            ConfigPath = PathUtils.GetPathForFile ( "kemori.conf" );
        }

        /// <summary>
        /// The path where the mangas should be saved to
        /// </summary>
        public static String SavePath
        {
            set
            {
                Config.SavePath = value;
                Save ( );
            }
            get
            {
                return Config.SavePath;
            }
        }

        /// <summary>
        /// User's bookmarked mangas
        /// </summary>
        public static List<Bookmark> Bookmarks
        {
            get
            {
                Save ( );
                return Config.Bookmarks;
            }
        }

        /// <summary>
        /// Wether the chapters should be compressed to a .cbz file
        /// </summary>
        public static Boolean CompressChapters
        {
            get
            {
                return Config.CompressChapters;
            }
            set
            {
                Config.CompressChapters = value;
                Save ( );
            }
        }

        /// <summary>
        /// Saves the configurations to a file
        /// </summary>
        public static void Save ( )
        {
            SerializerUtils.SerializeToFile ( Config, ConfigPath );
        }

        /// <summary>
        /// Saves the configurations to a file asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task SaveAsync ( )
        {
            await SerializerUtils.SerializeToFileAsync ( Config, ConfigPath );
        }

        /// <summary>
        /// Loads the configurations from a file
        /// </summary>
        public static void Load ( )
        {
            Config = SerializerUtils.DeserializeFromFile<Configs> ( ConfigPath );
        }

        /// <summary>
        /// Loads the configurations from a file asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task LoadAsync ( )
        {
            Config = await SerializerUtils.DeserializeFromFileAsync<Configs> ( ConfigPath );
        }
    }

    /// <summary>
    /// The class to use to serialize the configurations to the configs file
    /// </summary>
    [Serializable]
    internal class Configs
    {
        internal String SavePath;
        internal List<Bookmark> Bookmarks = new List<Bookmark> ( );
        internal Boolean CompressChapters;
    }
}