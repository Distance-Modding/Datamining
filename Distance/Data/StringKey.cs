using System.Collections.Generic;

namespace Distance.Data
{
	public struct StringKey
	{
		private readonly string path;

		public StringKey(string path) => this.path = path;

		public override bool Equals(object obj)
			=> obj is StringKey other
			&& path.ToLower() == other.path.ToLower();

		public override int GetHashCode() => -1757656154 + EqualityComparer<string>.Default.GetHashCode(path.ToLower());

		public override string ToString() => this;

		public static implicit operator string(StringKey instance) => instance.path;
	}
}
