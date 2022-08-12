using System.IO;
using System.Text;
using Distance.Util;

namespace Distance.Data
{
	public class GameInfo
	{
		public string Version { get; protected internal set; }

		public Platform Platform { get; protected internal set; }
		
		public string Branch { get; protected internal set; }
		
		public SteamBuildInfo BuildInfo { get; protected internal set; }

		public int? Depot => BuildInfo?.Depot;

		public string Manifest => BuildInfo?.Manifest;

		public string ShortName => $"Distance v{Version} {Platform} {Branch}";

		public string FullName => this;

		protected GameInfo() { }

		public static GameInfo From(DirectoryInfo gameBaseDir)
		{
			return new GameInfo()
			{
				Version = Subversion.GetVersion(gameBaseDir),
				Branch = Game.GetBranch(gameBaseDir),
				Platform = Game.DetectGamePlatform(gameBaseDir),
				BuildInfo = SteamBuildInfo.FromGameDir(gameBaseDir)
			};
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder(ShortName);

			if (BuildInfo != null && !string.IsNullOrEmpty(BuildInfo))
			{
				sb.Append($" ({BuildInfo})");
			}

			return sb.ToString();
		}

		public static implicit operator string(GameInfo instance) => instance.ToString();
	}
}
