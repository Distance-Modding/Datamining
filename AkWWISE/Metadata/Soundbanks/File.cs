using System;
using System.Xml.Serialization;

namespace AkWWISE.Metadata.Soundbanks
{
	[Serializable]
    public class File
    {
        [XmlAttribute]
        public ulong Id { get; set; }

        [XmlAttribute]
        public string Language { get; set; }

        [XmlElement]
        public string ShortName { get; set; }

        [XmlElement]
        public string Path { get; set; }
	}
}
