using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using AkWWISE.Metadata.Soundbanks;

namespace Distance.Data
{
    [Serializable]
    public struct WwiseEventItem
    {
		[XmlIgnore]
        public Event Original { get; internal set; }

		[XmlAttribute]
        public string Event => Original.Name;

		[XmlText]
        public string Path => Original.ObjectPath;

        public WwiseEventItem(Event e) => Original = e;

		public override bool Equals(object obj) 
			=> obj is WwiseEventItem item
			&& Event == item.Event
			&& Path == item.Path;

		public override int GetHashCode()
		{
			int hashCode = -471456659;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Event);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Path);
			return hashCode;
		}
	}
}
