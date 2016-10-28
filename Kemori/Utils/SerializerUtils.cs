using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Kemori.Utils
{
    class SerializerUtils
    {
        public static void SerializeToFile ( Object graph, String FileName )
        {
            var formatter = new BinaryFormatter
            {
                AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
                TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.XsdString
            };

            using ( var stream = File.Open ( FileName, FileMode.OpenOrCreate ) )
                formatter.Serialize ( stream, graph );
        }

        public static T DeserializeFromFile<T>(String FileName)
        {
            var formatter = new BinaryFormatter
            {
                AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
                TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.XsdString
            };

            using ( var stream = File.Open ( FileName, FileMode.Open ) )
                return ( T ) formatter.Deserialize ( stream );
        }
    }
}
