using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;

namespace ZbW.Testing.Dms.Client.Model
{
    [XmlRoot("Root")]
    public class MetadataItem
    {
        public  string Guid { get; set; }
        public string User { get; set; }
        public string FileName { get; set; }
        public DateTime CreareDate { get; set; }
        public DateTime ValueDate { get; set; }
        public string Designation { get; set; }
        public  String Type { get; set; }
        public  List<String> Keywords { get; set; }
        public static MetadataItem Deserialize(string path)
        {
            
                XmlSerializer serializer = new XmlSerializer(typeof(MetadataItem));
                StreamReader reader = new StreamReader(path);
                var metadataItem = (MetadataItem)serializer.Deserialize(reader);
                reader.Close();

                return metadataItem;
            


        }
        public static String Seralize(MetadataItem metadataItem)
        {
            XmlSerializer xmlserializer = new XmlSerializer(typeof(MetadataItem));
            StringWriter stringWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(stringWriter);

            xmlserializer.Serialize(writer, metadataItem);
            var serializeXml = stringWriter.ToString();
            writer.Close();
            return serializeXml;
        }
    }
}