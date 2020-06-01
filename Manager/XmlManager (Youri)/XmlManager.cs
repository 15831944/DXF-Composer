/*H**********************************************************************
* FILENAME :        HardwareConfig.cs
*
* DESCRIPTION :
*       Reads and Writes XML Files.
* 
* AUTHOR :    Youri Seichter     START DATE :    30 Okt 2016
* 
*H*/

using System;
using System.IO;
using System.Xml.Serialization;

namespace XmlSystem
{
    public class XmlHandler
    {

        /// <summary>
        /// Writes an object instance to an XML file.
        /// </summary>
        /// <param name="obj">The Object Type you want extract!</param>
        /// <param name="filePath">The file path to write the object instance to.</param>
        public static void XmlWriter<T>(T obj, string filePath)
        {
            TextWriter writer = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath);
                serializer.Serialize(writer, obj);
            }
            finally
            {
                writer?.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// </summary>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T XmlReader<T>(string filePath)
        {
            TextReader reader = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.UnknownAttribute += Serializer_UnknownAttribute;
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception mm_ex)
            {
                string mm = mm_ex.Message;
                return default(T);
            }
            finally
            {
                reader?.Close(); // is same as : if(writer != null) writer.Close();
            }
        }

        private static void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
