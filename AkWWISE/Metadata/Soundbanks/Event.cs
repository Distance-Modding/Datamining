using System.Xml.Serialization;
using System;

namespace AkWWISE.Metadata.Soundbanks
{
    [Serializable]
    public class Event
    {
        [XmlAttribute]
        public ulong Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string ObjectPath { get; set; }

        [XmlAttribute]
        public double MaxAttenuation { get; set; }
    }
}
