using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Backend.Domain.Entities.Util
{
    public class XmlFunction
    {
        public static T Deserialize<T>(string input) where T : class
        {
            if (!string.IsNullOrEmpty(input))
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));

                using (StringReader sr = new StringReader(input))
                {
                    return (T)ser.Deserialize(sr);
                }
            }
            else
                return null;
        }

        public static string Serialize<T>(T value)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(value.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, value, emptyNamespaces);
                return stream.ToString();
            }
        }
    }
}
