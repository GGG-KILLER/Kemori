using System;
using Kemori.Base;

namespace Kemori.Interfaces
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
        System.Threading.Tasks.Task Load ( );

        /// <summary>
        /// Recalculates the hash for this manga
        /// </summary>
        void ReCalcInstanceID ( );
    }
}