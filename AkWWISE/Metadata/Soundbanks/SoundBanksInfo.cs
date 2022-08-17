using System;
using System.Xml.Serialization;

namespace AkWWISE.Metadata.Soundbanks
{
	[Serializable]
	[XmlInclude(typeof(string)), XmlInclude(typeof(string))]
	[XmlRoot(nameof(SoundBanksInfo), IsNullable = false, Namespace = nameof(SoundBanksInfo))]
	public class SoundBanksInfo
    {
        [XmlAttribute]
        public string Platform { get; set; }

        [XmlAttribute]
        public string BasePlatform { get; set; }

        [XmlAttribute]
        public ulong SchemaVersion { get; set; }

        [XmlAttribute]
        public ulong SoundbankVersion { get; set; }

        [XmlElement]
        public RootPaths RootPaths { get; set; }

        [XmlArray]
        public SoundBank[] SoundBanks { get; set; }
    }
}
