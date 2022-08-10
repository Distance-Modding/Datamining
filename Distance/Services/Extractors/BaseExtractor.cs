using Distance.Data;
using Distance.Util;
using System.IO;
using System.Text;

namespace Distance.Services.Extractors
{
	public abstract class BaseExtractor
	{
		public DirectoryInfo GameBaseDir { get; protected set; }

		public DirectoryInfo GameDataDir => Game.GetGameDataDirectory(GameBaseDir, Platform);

		public Platform Platform { get; protected set; }

		public BaseExtractor(DirectoryInfo gameBaseDir, Platform platform = Platform.Auto)
		{
			GameBaseDir = gameBaseDir;
			Platform = platform != Platform.Auto ? platform : Game.DetectGamePlatform(gameBaseDir);
		}

		public abstract void ExtractTo(DirectoryInfo extractDir);

		protected string GetDestinationFolderName()
		{
			string version = Subversion.GetVersion(GameBaseDir);
			string branch = Game.GetBranch(GameBaseDir);

			StringBuilder sb = new StringBuilder($"Distance v{version} {Platform} {branch}");

			BuildInfo info = BuildInfo.FromGameDir(GameBaseDir);
			if (info != null && !string.IsNullOrEmpty(info))
			{
				sb.Append($" ({info})");
			}

			return sb.ToString();
		}
	}
}
