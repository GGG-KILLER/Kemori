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
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Utils;

namespace Kemori.Resources
{
    internal class ConfigsManager
    {
        /// <summary>
        /// The path where the configuration file is being stored
        /// </summary>
        private static readonly String ConfigPath;

        /// <summary>
        /// The instance of the settings object
        /// </summary>
        private static Configs Config;

        /// <summary>
        /// Initializes the manager needs to operate
        /// </summary>
        static ConfigsManager ( )
        {
            ConfigPath = PathUtils.GetPathForFile ( "kemori.conf" );
            Load ( );
        }

        /// <summary>
        /// Event fired when Save Path changes
        /// </summary>
        public static event PropertyChangedEventHandler SavePathChanged;

        /// <summary>
        /// The path where the mangas should be saved to
        /// </summary>
        public static String SavePath
        {
            set
            {
                Config.SavePath = value;

                SavePathChanged?.Invoke ( null, new PropertyChangedEventArgs ( nameof ( SavePath ) ) );

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
            try
            {
                SerializerUtils.SerializeToFile ( Config, ConfigPath );
            }
            catch ( Exception )
            {
                return;
            }
        }

        /// <summary>
        /// Saves the configurations to a file asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task SaveAsync ( )
        {
            try
            {
                await SerializerUtils.SerializeToFileAsync ( Config, ConfigPath );
            }
            catch ( Exception )
            {
                return;
            }
        }

        /// <summary>
        /// Loads the configurations from a file
        /// </summary>
        public static void Load ( )
        {
            try
            {
                Config = SerializerUtils.DeserializeFromFile<Configs> ( ConfigPath );
            }
            catch ( Exception )
            {
                Config = new Configs ( );
            }
        }

        /// <summary>
        /// Loads the configurations from a file asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task LoadAsync ( )
        {
            try
            {
                Config = await SerializerUtils.DeserializeFromFileAsync<Configs> ( ConfigPath );

                if ( Config == null )
                    Config = new Configs ( );
            }
            catch ( Exception )
            {
                Config = new Configs ( );
            }
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