using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public abstract class DataChunk : IVisitor
	{
		public SoundBank SoundBank { get; private set; }

		public abstract ChunkType ChunkType { get; }

		public string Name => ChunkType.Name;

		public abstract void Visit(IReader reader);

		protected DataChunk(SoundBank soundBank)
		{
			SoundBank = soundBank;
		}
	}
}
