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
using Kemori.Base;

namespace Kemori.Abstractions
{
    /// <summary>
    /// Manga interface
    /// </summary>
    public interface IManga
    {
        /// <summary>
        /// Name of the manga
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Connector used with the manga
        /// </summary>
        MangaConnector Connector { get; }

        /// <summary>
        /// Chapter list
        /// </summary>
        MangaChapter[] Chapters { get; }

        /// <summary>
        /// Downloaded path (if existent)
        /// </summary>
        String Path { get; }

        /// <summary>
        /// Link to the manga
        /// </summary>
        String Link { get; }

        /// <summary>
        /// Instance hash
        /// </summary>
        String InstanceID { get; }

        /// <summary>
        /// Loads the information about the manga
        /// </summary>
        System.Threading.Tasks.Task LoadAsync ( );

        /// <summary>
        /// Recalculates the hash for this manga
        /// </summary>
        void ReCalcInstanceID ( );
    }
}