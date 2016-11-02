using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kemori.Base;
using Kemori.ConnectorTest.Controllers;

namespace Kemori.ConnectorTest
{
    class Program
    {
        static void Main ( )
        {
            var connectorsErrors = ConnectorsManager.ValidateAll ( );

            Console.WriteLine ( "Errors:" );

            foreach ( var connectorErrors in connectorsErrors )
            {
                Console.WriteLine ( $"\t{connectorErrors.Key.ID}:" );

                foreach ( var error in connectorErrors.Value )
                {
                    Console.WriteLine ( "\t\t" + error );
                }
            }

            Console.ReadKey ( );
        }
    }
}
