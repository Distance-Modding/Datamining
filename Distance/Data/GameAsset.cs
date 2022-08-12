using AssetStudio;
using System.Linq;

namespace Distance.Data
{
	public class GameAsset
	{
		public readonly Object asset;
		public readonly SerializedFile source;
		public readonly ClassIDType type;
		public readonly long pathID;

		public string Name { get; protected internal set; }

		public string Container { get; protected internal set; } = string.Empty;

		public long FullSize { get; protected internal set; }

		public string AssetPath
		{
			get
			{
				if (string.IsNullOrEmpty(Container))
				{
					return Name;
				}

				string[] segments = Container.Split('/');
				return string.Join("/", segments.Take(segments.Length - 1).Concat(new string[] { Name }));
			}
		}

		public GameAsset(Object assetObj)
		{
			asset = assetObj;
			source = asset.assetsFile;
			type = asset.type;
			FullSize = asset.byteSize;
			pathID = asset.m_PathID;
		}

		public override string ToString() => ToString();

		public string ToString(string separator = "\t")
		{
			return $"{type}{separator}{AssetPath}";
		}

		public static implicit operator Object(GameAsset instance) => instance.asset;
	}
}
