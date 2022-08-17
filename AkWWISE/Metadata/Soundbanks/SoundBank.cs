using System.Xml.Serialization;
using System;

namespace AkWWISE.Metadata.Soundbanks
{
    [Serializable]
    [XmlInclude(typeof(Event)), XmlInclude(typeof(File))]
    public class SoundBank
    {
        [XmlAttribute]
        public ulong Id { get; set; }

        [XmlElement]
        public string Language { get; set; }

        [XmlElement]
        public string ObjectPath { get; set; }

        [XmlElement]
        public string ShortName { get; set; }

        [XmlElement]
        public string Path { get; set; }

        [XmlArray]
        public Event[] IncludedEvents { get; set; }

        [XmlArray]
        public File[] ReferencedStreamedFiles { get; set; }

        [XmlArray]
        public File[] IncludedMemoryFiles { get; set; }
    }
}
