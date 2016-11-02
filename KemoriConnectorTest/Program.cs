using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kemori.Base;

namespace Kemori.ConnectorTest
{
    class Program
    {
        static Type MangaConnectorType = typeof ( MangaConnector );

        static void Main ( )
        {
            var paths = ListAssemblies ( );

            var validCons = new List<MangaConnector> ( );

            foreach ( var path in paths )
            {
                var conns = GetMangaConnectors ( path );

                if ( conns.Count > 0 )
                    validCons.AddRange ( conns );
            }

            Console.WriteLine ( "Finished checking connectors." );
            Console.WriteLine ( "Results:" );
            Console.WriteLine ( $"\tFound {validCons.Count} connectors:" );
            for ( var i = 0 ; i < validCons.Count ; i++ )
                Console.WriteLine ( $"\t\t{i}. {validCons[i].Website}" );

            Console.ReadKey ( );
        }

        static IEnumerable<String> ListAssemblies ( )
        {
            var di = new DirectoryInfo ( "Connectors" );
            Console.WriteLine ( "Searching for Kemori.*.dll on Connectors/:" );
            return di.EnumerateFiles ( "Kemori.*.dll" )
                .Select ( fi =>
                 {
                     Console.WriteLine ( $"\t{fi.FullName}" );
                     return fi.FullName;
                 } );
        }

        static IList<MangaConnector> GetMangaConnectors ( String FullName )
        {
            Console.WriteLine ( $"Validating assembly: {FullName}" );
            var assm = Assembly.LoadFrom ( FullName );
            Console.WriteLine ( "\tAssembly loaded" );

            var types = assm.GetExportedTypes ( );
            Console.WriteLine ( $"\tFound {types.Length} exported types (publically declared classes, structs, enums, etc.)" );

            var list = new List<MangaConnector> ( );

            Console.WriteLine ( "\tChecking exported types:" );
            foreach ( var type in types )
            {
                Console.WriteLine ( $"\t\tChecking type {type.FullName}..." );
                if ( type.IsSubclassOf ( MangaConnectorType ) )
                {
                    Console.WriteLine ( "\t\t\tIs Connector" );
                    list.Add ( ( MangaConnector ) Activator.CreateInstance ( type ) );
                }
                else
                {
                    Console.WriteLine ( "\t\t\tIsn't Connector" );
                }
            }

            return list;
        }
    }
}
