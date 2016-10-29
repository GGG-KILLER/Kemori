using System;
using System.Linq;
using Kemori.Interfaces;

namespace Kemori.Base
{
    public class Manga : IManga
    {
        /// <summary>
        /// The chapters available for this manga
        /// </summary>
        public MangaChapter[] Chapters { get; set; }

        private MangaConnector _connector;
        private Boolean _connectorSet;

        /// <summary>
        /// The connector associated with this manga
        /// </summary>
        public MangaConnector Connector
        {
            get
            {
                return _connector;
            }
            set
            {
                if ( !_connectorSet )
                {
                    _connector = value;
                    _connectorSet = true;
                }
                else
                {
                    throw new InvalidOperationException ( "Connector is a one-time set property!" );
                }
            }
        }

        private String _hash;

        /// <summary>
        /// The hash for this manga
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
        /// Links to the manga(for chapter retrieving)
        /// </summary>
        public String Link { get; set; }

        /// <summary>
        /// Name of the manga
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Download path
        /// </summary>
        public String Path { get; set; }

        /// <summary>
        /// Loads the information for this manga(chapters)
        /// </summary>
        public async void Load ( )
        {
            this.Chapters = ( await Connector.GetChaptersAsync ( this ) ).ToArray ( );
        }

        /// <summary>
        /// Recalculates the InstanceID for this manga
        /// </summary>
        public void ReCalcInstanceID ( )
        {
            _hash = $"{Connector.ID}{Connector.Website}{this.Name}{this.Link}"
                    .GetHashCode ( )
                    .ToString ( );
        }
    }
}