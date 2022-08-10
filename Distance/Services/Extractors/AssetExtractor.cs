using System;
using System.IO;
using Distance.Data;

namespace Distance.Services.Extractors
{
	public class AssetExtractor : BaseExtractor
	{
		public AssetExtractor(DirectoryInfo gameBaseDir, Platform platform = Platform.Auto)
		: base(gameBaseDir, platform)
		{ }

		public static void Extract(DirectoryInfo gameBaseDir, DirectoryInfo destination, Platform platform = Platform.Auto)
			=> new AssetExtractor(gameBaseDir, platform)
			.ExtractTo(destination);

		public override void ExtractTo(DirectoryInfo extractDir)
		{
			string destinationName = GetDestinationFolderName();
			Console.WriteLine(destinationName);
			DirectoryInfo destination = extractDir.CreateSubdirectory(destinationName);
			foreach (FileInfo assetsFile in GameBaseDir.GetFiles("*.assets"))
			{
				ExtractTo(destination, assetsFile);
			}
		}

		protected void ExtractTo(DirectoryInfo destination, FileInfo assetsFile)
		{

		}
	}
}
