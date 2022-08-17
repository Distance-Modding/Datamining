using System.Xml.Serialization;
using System;

namespace AkWWISE.Metadata.Soundbanks
{
    [Serializable]
    public class RootPaths
    {
        [XmlElement]
        public string ProjectRoot { get; set; }

        [XmlElement]
        public string SourceFilesRoot { get; set; }

        [XmlElement]
        public string SoundBanksRoot { get; set; }

        [XmlElement]
        public string ExternalSourcesInputFile { get; set; }

        [XmlElement]
        public string ExternalSourcesOutputRoot { get; set; }
	}
}
