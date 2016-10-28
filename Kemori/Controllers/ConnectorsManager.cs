using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Classes;

namespace Kemori.Controllers
{
    class ConnectorsManager
    {
        public static IList<MangaConnector> GetAll ( )
        {
            var di = new DirectoryInfo ( "." );
            if ( !di.Exists )
            {
                di.Create ( );
                throw new Exception ( "No connectors in the \"Connectors\" folder" );
            }

            var assemblies = di.GetFiles ( "*.dll", SearchOption.TopDirectoryOnly )
                //.Concat ( new FileInfo ( Assembly.GetExecutingAssembly ( ).FullName ).Directory.GetFiles ( "*.dll" ) ) // Debug
                .Where ( fi => fi.FullName.Split ( '.' )[0] == "Kemori" )
                .Select ( fi =>
                {
                    try
                    {
                        return Assembly.LoadFile ( fi.FullName );
                    }
                    catch ( Exception )
                    {
                        return null;
                    }
                } )
                .Where ( @as => @as != null )
                .Where ( @as =>
                {
                    return ReflectionUtils.GetTypesInNamespace<MangaConnector> ( @as, "Kemori.Connectors" ).Count ( ) > 1;
                } );

            var connectors = new List<MangaConnector> ( );
            foreach ( var assembly in assemblies )
            {
                connectors.AddRange
                (
                    ReflectionUtils.GetTypesInNamespace<MangaConnector> ( assembly, "Kemori.Connectors" )
                );
            }

            return connectors;
        }

        public static MangaConnector GetByID ( String ID )
        {
            return GetAll ( )
                .FirstOrDefault ( conn => conn.ID == ID );
        }

        public static IEnumerable<MangaConnector> GetByWebsite ( String Website )
        {
            return GetAll ( )
                .Where ( conn => conn.Website == Website );
        }
    }
}
