using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.Utils;

namespace Kemori.Controllers
{
    internal class ConnectorsManager
    {
        /// <summary>
        /// Retrieves all <see cref="MangaConnector"/>s from all assemblies in the
        /// "Connectors" folder.
        /// </summary>
        /// <returns></returns>
        public static IList<MangaConnector> GetAll ( )
        {
            var di = new DirectoryInfo ( "." );
            if ( !di.Exists )
            {
                di.Create ( );
                throw new Exception ( "No connectors in the \"Connectors\" folder" );
            }

            var assemblies = di.GetFiles ( "*.dll", SearchOption.TopDirectoryOnly )
// Debug builds check for .dlls in the same directory too
#if DEBUG
                .Concat (
                    new FileInfo (
                        Assembly.GetExecutingAssembly ( ).FullName
                    )
                        .Directory
                        .GetFiles ( "*.dll" )
                )
#endif
                .Where ( fi => fi.FullName.Split ( '.' )[0] == "Kemori" )
                .Select ( fi =>
                {
                    try
                    {
                        return Assembly.LoadFile ( fi.FullName );
                    }
                    catch ( Exception e )
                    {
#if DEBUG
                        var l = new Logger ( );
                        l.LogAsync ( "Couldn't load assembly: " )
                        .ContinueWith ( t => l.LogAsync ( e ) );
#endif
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

        /// <summary>
        /// Retrieves all <see cref="MangaConnector"/>s from all assemblies in the
        /// "Connectors" folder asynchronously
        /// </summary>
        /// <returns></returns>
        public static async Task<IList<MangaConnector>> GetAllAsync()
        {
            return await Task.Run ( ( ) => GetAll ( ) );
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