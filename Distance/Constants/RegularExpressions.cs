using System.Text.RegularExpressions;

namespace Distance.Constants
{
	public static class RegularExpressions
	{
		public const string MANIFEST_EXPRESSION = @"(?<depot>\d+)_(?<manifest>\w+)\.bin.*";
		public static readonly Regex ManifestRegex = new Regex(MANIFEST_EXPRESSION);
		public const string GAMEDIR_EXPRESSION = @"(?<platform>\w+) (?<branch>[\w-]+)( (?<manifest>.+))?";
		public static readonly Regex GameDirRegex = new Regex(GAMEDIR_EXPRESSION);
	}
}
