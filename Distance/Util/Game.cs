using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Distance.Data;
using static Distance.Constants.RegularExpressions;

namespace Distance.Util
{
	public static class Game
	{
		public delegate DirectoryInfo DirectoryMapperDelegate(DirectoryInfo directory);

		public const string ASSEMBLY = "Assembly-CSharp.dll";

		public static readonly Dictionary<Platform, DirectoryMapperDelegate> GameFolderToDataPath = new Dictionary<Platform, DirectoryMapperDelegate>()
		{
			{ Platform.Auto, AutoDataPathMapper },
			{ Platform.Windows, dir => new DirectoryInfo(Path.Combine(dir.FullName, $@"Distance_Data\")) },
			{ Platform.Linux, dir => new DirectoryInfo(Path.Combine(dir.FullName, $@"bin\Distance_Data")) },
			{ Platform.Mac, dir => new DirectoryInfo(Path.Combine(dir.FullName, $@"Distance.app\Contents\Resources\Data")) },
		};

		public static bool IsGameDirectory(DirectoryInfo gameBaseDir)
		{
			return DetectGamePlatform(gameBaseDir) != Platform.Auto;
		}

		public static Platform DetectGamePlatform(DirectoryInfo gameBaseDir)
		{
			try
			{
				return GameFolderToDataPath
					.Where(item => item.Key != Platform.Auto)
					.First(kvp => kvp.Value(gameBaseDir).Exists).Key;
			}
			catch (Exception)
			{
				return Platform.Auto;
			}
		}

		private static DirectoryInfo AutoDataPathMapper(DirectoryInfo gameBaseDir)
		{
			try
			{
				return GameFolderToDataPath
					.Where(item => item.Key != Platform.Auto)
					.Select(item => item.Value(gameBaseDir))
					.First(dataDir => dataDir.Exists);
			}
			catch (InvalidOperationException invalidOperationException)
			{
				throw new InvalidOperationException($"Unable to determine Unity Data path for \"{gameBaseDir.FullName}\"", invalidOperationException);
			}
		}

		public static DirectoryInfo GetGameDataDirectory(DirectoryInfo gameBaseDir, Platform platform = Platform.Auto)
		{
			if (GameFolderToDataPath.TryGetValue(platform, out DirectoryMapperDelegate mapper))
			{
				return mapper(gameBaseDir);
			}

			throw new InvalidOperationException($"Platform with ID {(int)platform} does not exist.");
		}

		public static FileInfo GetGameAssemblyPath(DirectoryInfo gameBaseDir, Platform platform = Platform.Auto)
		{
			DirectoryInfo gameDataPath = GetGameDataDirectory(gameBaseDir, platform);
			return new FileInfo(Path.Combine(gameDataPath.FullName, $@"Managed\{ASSEMBLY}"));
		}

		public static string GetBranch(DirectoryInfo gameBaseDir)
		{
			Match match = GameDirRegex.Match(gameBaseDir.Name);
			if (match.Success)
			{
				return match.Groups["branch"].Value;
			}

			foreach(string branch in Resources.KnownSteamBranches.Split('\n'))
			{
				if (gameBaseDir.Name.Contains(branch))
				{
					return branch;
				}
			}

			return "nodrm";
		}
	}
}
