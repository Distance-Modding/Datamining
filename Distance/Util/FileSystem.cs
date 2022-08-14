using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Distance.Util
{
	public static class FileSystem
	{
		public static readonly string IllegalChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
		public static readonly Regex IllegalCharsRegex = new Regex($"[{Regex.Escape(IllegalChars)}]");
		
		public static string RemoveIllegalPathChars(this string input)
		{
			return IllegalCharsRegex.Replace(input, string.Empty);
		}

		public static string RemoveUnwantedChars(this string input, string unwantedChars = "+-*=.!{}")
		{
			StringBuilder sb = new StringBuilder();
			foreach (char c in input)
			{
				if (!unwantedChars.Contains(c.ToString()))
				{
					sb.Append(c);
				}
			}
			return sb.ToString();

		}
	}
}
