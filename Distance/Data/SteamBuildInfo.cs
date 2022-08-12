using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static Distance.Constants.RegularExpressions;

namespace Distance.Data
{
	public class SteamBuildInfo
	{
		public const string DEPOT_DOWNLOADER_METADATA = ".DepotDownloader";
		
		public int Depot { get; protected set; }

		public string Manifest { get; protected set; }

		public SteamBuildInfo(int depot, string manifest)
		{
			Depot = depot;
			Manifest = manifest;
		}

		public static SteamBuildInfo FromGameDir(DirectoryInfo gameBaseDir)
		{
			DirectoryInfo metadataDir = new DirectoryInfo(Path.Combine(gameBaseDir.FullName, DEPOT_DOWNLOADER_METADATA));
			if (!metadataDir.Exists)
			{
				return FromGameDirName(gameBaseDir);
			}

			return FromMetadataDir(metadataDir);
		}

		public static SteamBuildInfo FromGameDirName(DirectoryInfo gameBaseDir)
		{
			Match match = GameDirRegex.Match(gameBaseDir.Name);
			if (!match.Success)
			{
				return null;
			}

			return new SteamBuildInfo(233610, match.Groups["manifest"]?.Value ?? "unknown_manifest_id");
		}

		public static SteamBuildInfo FromMetadataDir(DirectoryInfo metadataDir)
		{
			if (!metadataDir.Exists)
			{
				return null;
			}
			
			FileInfo manifestFile = metadataDir.GetFiles()
				.First(file => ManifestRegex.IsMatch(file.Name));

			Match match = ManifestRegex.Match(manifestFile.Name);

			return new SteamBuildInfo(
				int.Parse(match.Groups["depot"].Value), 
				match.Groups["manifest"].Value
			);
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			if (Depot != 233610)
			{
				sb.Append($"{Depot} ");
			}
			sb.Append(Manifest);
			return sb.ToString();
		}

		public static implicit operator string(SteamBuildInfo info) => info.ToString();
	}
}