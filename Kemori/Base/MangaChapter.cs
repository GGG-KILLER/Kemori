using System;
using Kemori.Interfaces;

namespace Kemori.Base
{
    public class MangaChapter : IMangaChapter
    {
        /// <summary>
        /// Chapter number (1, 1.5, etc.)
        /// Use manga chapter array index if not existent.
        /// </summary>
        public Single Chapter { get; private set; }

        private String _hash;

        /// <summary>
        /// ID of the instance (used for unique identifying)
        /// </summary>
        public String InstanceID
        {
            get
            {
                if ( _hash == null )
                {
                    ReCalcInstanceID ( );
                }

                return _hash;
            }
        }

        /// <summary>
        /// Link to this chapter (for page getting)
        /// </summary>
        public String Link { get; private set; }

        /// <summary>
        /// The manga this chapter belongs to
        /// </summary>
        public Manga Manga { get; private set; }

        /// <summary>
        /// Name of the chapter (if existent)
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// Links to the pages
        /// </summary>
        public String[] PageLinks { get; private set; }

        /// <summary>
        /// Number of pages
        /// </summary>
        public Int32 Pages { get; private set; }

        /// <summary>
        /// Loads number of pages and page links
        /// </summary>
        public async void Load ( )
        {
            this.PageLinks = await Manga.Connector.GetPageLinks ( this );
            this.Pages = this.PageLinks.Length;
        }

        /// <summary>
        /// Recalculates the instance ID
        /// </summary>
        public void ReCalcInstanceID ( )
        {
            _hash = $"{Manga.InstanceID}{this.Name}{this.Link}"
                .GetHashCode ( )
                .ToString ( );
        }
    }
}