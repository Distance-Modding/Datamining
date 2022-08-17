using System.IO;
using Distance.Data;
using Distance.Util;

namespace Distance.Services.Extractors
{
	public abstract class BaseExtractor : IExtractor<DirectoryInfo>
	{
		public DirectoryInfo GameBaseDir { get; protected set; }

		public DirectoryInfo GameDataDir => Game.GetGameDataDirectory(GameBaseDir, Platform);

		public Platform Platform { get; protected set; }

		public GameInfo GameInfo { get; protected internal set; }

		public BaseExtractor(DirectoryInfo gameBaseDir, Platform platform = Platform.Auto)
		{
			GameBaseDir = gameBaseDir;
			GameInfo = GameInfo.From(GameBaseDir);
			Platform = platform != Platform.Auto ? platform : Game.DetectGamePlatform(gameBaseDir);
		}

		public abstract void ExtractTo(DirectoryInfo extractDir);

		void IExtractor<DirectoryInfo>.ExtractTo(DirectoryInfo extractDir) => ExtractTo(extractDir);

		protected string GetDestinationFolderName() => GameInfo;
	}
}
