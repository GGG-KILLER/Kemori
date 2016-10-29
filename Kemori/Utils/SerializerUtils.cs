using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Kemori.Utils
{
    internal class SerializerUtils
    {
        /// <summary>
        /// Serializes an object to a file
        /// </summary>
        /// <param name="graph">Object to serialize</param>
        /// <param name="FileName">File name to save serialized object</param>
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

        /// <summary>
        /// Serializes an object to a file
        /// </summary>
        /// <param name="graph">Object to serialize</param>
        /// <param name="FileName">File name to save serialized object</param>
        public static async Task SerializeToFileAsync ( Object graph, String FileName )
        {
            // BinaryFormmater doesn't has support for asynchronous operations, so we have to do this
            await Task.Run ( ( ) => SerializeToFile ( graph, FileName ) );
        }

        /// <summary>
        /// Deserialized an type from a file
        /// </summary>
        /// <typeparam name="T">Type that was serialized</typeparam>
        /// <param name="FileName">The file to deserialize from</param>
        /// <returns></returns>
        public static T DeserializeFromFile<T> ( String FileName )
        {
            var formatter = new BinaryFormatter
            {
                AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
                TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.XsdString
            };

            using ( var stream = File.Open ( FileName, FileMode.Open ) )
                return ( T ) formatter.Deserialize ( stream );
        }

        /// <summary>
        /// Deserializes a class from a file asynchronously
        /// </summary>
        /// <typeparam name="T">Type that was serialized</typeparam>
        /// <param name="FileName">The file to deserialize from</param>
        public static async Task<T> DeserializeFromFileAsync<T> ( String FileName )
        {
            return await Task.Run ( ( ) => DeserializeFromFile<T> ( FileName ) );
        }
    }
}