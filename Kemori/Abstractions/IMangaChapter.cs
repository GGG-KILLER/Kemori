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
    /// Manga chapter interface
    /// </summary>
    public interface IMangaChapter
    {
        /// <summary>
        /// The manga it belongs to
        /// </summary>
        Manga Manga { get; }

        /// <summary>
        /// The link to the chapter (usually first page)
        /// </summary>
        String Link { get; }

        /// <summary>
        /// The amount of pages the chapter has
        /// </summary>
        Int32 Pages { get; }

        /// <summary>
        /// Name of the chapter (if existent)
        /// </summary>
        String Name { get; }

        /// <summary>
        /// Number of the chapter (1, 1.5, etc.) If not available use sequential numbering or the
        /// index on the chapter array.
        /// </summary>
        String Chapter { get; }

        /// <summary>
        /// The links of the pages
        /// </summary>
        String[] PageLinks { get; }

        /// <summary>
        /// Instance hash
        /// </summary>
        String InstanceID { get; }

        /// <summary>
        /// Loads the information about the chapter (page count and page links)
        /// </summary>
        void Load ( );

        /// <summary>
        /// Recalculates the instance ID for this chapter
        /// </summary>
        void ReCalcInstanceID ( );
    }
}
