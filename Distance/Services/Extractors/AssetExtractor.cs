using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetStudio;
using Distance.Data;
using static Distance.Assets.AssetExporter;

namespace Distance.Services.Extractors
{
	public class AssetExtractor : BaseExtractor
	{
		public static readonly string[] RESOURCE_SEARCH_PATTERNS =
		{
			"mainData",
			"globalgamemanagers",
			"*.assets",
			"level*"
		};

		protected AssetsManager assetsManager;
		protected AssetInfoDatabase assetInfoDatabase;

		public DirectoryInfo UnityResourcesDir => new DirectoryInfo(Path.Combine(GameDataDir.FullName, "Resources"));

		public AssetExtractor(DirectoryInfo gameBaseDir, Platform platform = Platform.Auto)
		: base(gameBaseDir, platform) => LoadAssets();

		protected void LoadAssets()
		{
			assetsManager = new AssetsManager();

			List<FileInfo> assetBundles = new List<FileInfo>();

			foreach (string pattern in RESOURCE_SEARCH_PATTERNS)
			{
				assetBundles.AddRange(GameDataDir.GetFiles(pattern));
			}

			if (UnityResourcesDir.Exists)
			{
				assetBundles.AddRange(UnityResourcesDir.GetFiles("unity_*"));
			}

			Console.WriteLine($"Loading assets in memory...");
			assetsManager.LoadFiles(assetBundles.Select(File => File.FullName).ToArray());

			Console.WriteLine($"Processing assets hierarchy...");
			assetInfoDatabase = AssetInfoDatabase.From(assetsManager);
		}

		public static void Extract(DirectoryInfo gameBaseDir, DirectoryInfo destination, Platform platform = Platform.Auto)
			=> new AssetExtractor(gameBaseDir, platform)
			.ExtractTo(destination);

		public override void ExtractTo(DirectoryInfo extractDir)
		{
			string destinationName = GetDestinationFolderName();
			Console.WriteLine(destinationName);
			DirectoryInfo destination = extractDir.CreateSubdirectory(destinationName);

			foreach (GameAsset asset in assetInfoDatabase.Assets)
			{
				ExtractTo(destination, asset);
			}
		}

		protected void ExtractTo(DirectoryInfo extractDir, GameAsset gameAsset)
		{
			try
			{
				FileInfo destination;
				string extension;
				switch (gameAsset.asset)
				{
					case Texture2D m_Texture2D:
						destination = AssetFileInfo(extractDir, gameAsset, ".png");
						ExportTexture2D(destination, m_Texture2D);
						break;
					case Sprite m_Sprite:
						destination = AssetFileInfo(extractDir, gameAsset, ".png");
						ExportSprite(destination, m_Sprite);
						break;
					case TextAsset m_TextAsset:
						extension = ExtensionMapper.GetTextAssetFileExtension(gameAsset);
						destination = AssetFileInfo(extractDir, gameAsset, extension);
						ExportTextAsset(destination, m_TextAsset);
						break;
					case Font m_Font:
						extension = ExtensionMapper.GetFontFileExtension(m_Font);
						destination = AssetFileInfo(extractDir, gameAsset, extension);
						ExportFont(destination, m_Font);
						break;
					case AudioClip m_AudioClip:
						destination = AssetFileInfo(extractDir, gameAsset, ".wav");
						ExportAudioClip(destination, m_AudioClip);
						break;
					case VideoClip m_VideoClip:
						destination = AssetFileInfo(extractDir, gameAsset, Path.GetExtension(m_VideoClip.m_OriginalPath));
						ExportVideoClip(destination, m_VideoClip);
						break;
					case MovieTexture m_MovieTexture:
						destination = AssetFileInfo(extractDir, gameAsset, ".ogv");
						ExportMovieTexture(destination, m_MovieTexture);
						break;
					case Mesh m_Mesh:
						destination = AssetFileInfo(extractDir, gameAsset, ".obj");
						ExportMesh(destination, m_Mesh);
						break;

					default: return;
				}
				Console.WriteLine($"Extracted \"{gameAsset}\"...");
			}
			catch (Exception exception)
			{
				Console.WriteLine($"An error occured while extracting {gameAsset} {exception.Message}");
			}
		}

		protected FileInfo AssetFileInfo(DirectoryInfo extractBaseDir, GameAsset gameAsset, string fileExtension = null, bool createDirectory = true)
		{
			FileInfo result = new FileInfo(Path.Combine(extractBaseDir.FullName, $"{gameAsset.AssetPath}{(string.IsNullOrEmpty(fileExtension) ? "" : fileExtension)}"));
			if (createDirectory)
			{
				result.Directory.Create();
			}
			return result;
		}

		~AssetExtractor()
		{
			assetsManager.Clear();
			GC.Collect();
			GC.WaitForFullGCComplete();
		}

		public static  class ExtensionMapper
		{
			// TODO: Externalize this into a json resource file
			public const string DEFAULT_EXTASSET_EXTENSION = ".txt";
			public static readonly List<(string[], string)> TextAssetPathsToExtension = new List<(string[], string)>()
			{
				(new[] 
				{ 
					"splineroadtemplates/",
					"splinetunneltemplates/",
					"levels/",
					"levelbackups/" 
				}, ".bytes"),
				(new[] 
				{ 
					"levelplaylists/",
					"blueprints/",
					"editorconfig/"
				}, ".xml")
			};

			public static readonly Dictionary<string, string> TextAssetNameToExtension = new Dictionary<string, string>()
			{
				{ "LevelEditorPrefabDirectoryInfo", ".xml" },
				{ "LevelInfos", ".bytes" },
				{ "Localization", ".csv" },
				{ "ResourceList", ".xml" }
			};

			public static readonly byte[] OTFFontHeader = { 79, 84, 84, 79 };

			public static string GetTextAssetFileExtension(GameAsset asset)
			{
				if (TextAssetNameToExtension.TryGetValue(asset.AssetPath, out string result))
				{
					return result;
				}

				foreach ((string[] paths, string ext) in TextAssetPathsToExtension)
				{
					foreach(string path in paths)
					{
						if (asset.Container.StartsWith(path, StringComparison.InvariantCultureIgnoreCase))
						{
							return ext;
						}
					}
				}

				return DEFAULT_EXTASSET_EXTENSION;
			}

			public static string GetFontFileExtension(Font m_Font)
			{
				byte[] m_FontData = m_Font.m_FontData;

				if (m_FontData.Take(4).SequenceEqual(OTFFontHeader))
				{
					return ".otf";
				}

				return ".ttf";
			}
		}
	}
}
