using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Binarysharp.MemoryManagement.Core.Helpers
{
    /// <summary>
    ///     Static helper class providing tools for serializing/deserializing objects.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        ///     Serializes the specified object and writes the XML document to the specified path.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="path">The path where the file is saved.</param>
        /// <param name="encoding">The encoding to generate.</param>
        public static void ExportToFile<T>(T obj, string path, Encoding encoding)
        {
            // Create the stream to write into the specified file
            using (var file = new StreamWriter(path, false, encoding))
            {
                // Write the content by calling the method to serialize the object
                file.Write(ExportToString(obj));
            }
        }

        /// <summary>
        ///     Serializes the specified object and writes the XML document to the specified path using
        ///     <see cref="Encoding.UTF8" /> encoding.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="path">The path where the file is saved.</param>
        public static void ExportToFile<T>(T obj, string path)
        {
            ExportToFile(obj, path, Encoding.UTF8);
        }

        /// <summary>
        ///     Serializes the specified object and returns the XML document.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>XML document of the serialized object.</returns>
        public static string ExportToString<T>(T obj)
        {
            // Initialize the required objects for serialization
            var serializer = new XmlSerializer(typeof (T));
            using (var stringWriter = new StringWriter())
            {
                // Serialize the object
                serializer.Serialize(stringWriter, obj);
                // Return the serialized object
                return stringWriter.ToString();
            }
        }

        /// <summary>
        ///     Deserializes the specified file into an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="path">The path where the object is read.</param>
        /// <param name="encoding">The character encoding to use. </param>
        /// <returns>The deserialized object.</returns>
        public static T ImportFromFile<T>(string path, Encoding encoding)
        {
            // Create the stream to read the specified file
            using (var file = new StreamReader(path, encoding))
            {
                // Read the content of the file and call the method to deserialize the object
                return ImportFromString<T>(file.ReadToEnd());
            }
        }

        /// <summary>
        ///     Deserializes the specified file into an object using <see cref="Encoding.UTF8" /> encoding.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="path">The path where the object is read.</param>
        /// <returns>The deserialized object.</returns>
        public static T ImportFromFile<T>(string path)
        {
            return ImportFromFile<T>(path, Encoding.UTF8);
        }

        /// <summary>
        ///     Deserializes the XML document to the specified object.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="serializedObj">The string representing the serialized object.</param>
        /// <returns>The deserialized object.</returns>
        public static T ImportFromString<T>(string serializedObj)
        {
            // Initialize the required objects for deserialization
            var serializer = new XmlSerializer(typeof (T));
            using (var stringWriter = new StringReader(serializedObj))
            {
                // Return the serialized object
                return (T) serializer.Deserialize(stringWriter);
            }
        }
    }
}