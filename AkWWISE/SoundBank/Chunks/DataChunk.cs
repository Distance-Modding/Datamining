using AkWWISE.Model;
using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public abstract class DataChunk : IVisitor
	{
		public SoundBank SoundBank { get; private set; }

		public abstract ChunkType ChunkType { get; }

		public string Name => ChunkType.Name;

		public string Description => ChunkType.FriendlyName;

		public FourCC ChunkHeader => ChunkType.ChunkHeader;

		protected DataChunk(SoundBank soundBank)
		=> SoundBank = soundBank;

		public abstract void Visit(IReader reader);
	}
}
