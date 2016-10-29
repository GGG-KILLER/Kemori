using System;
using Kemori.Base;

namespace Kemori.Interfaces
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
        /// Number of the chapter (1, 1.5, etc.)
        /// If not available use sequential numbering or the index on the chapter array.
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