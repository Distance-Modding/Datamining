using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class STID : DataChunk
	{
		public STID(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.STID;

		public override void Visit(IReader reader)
		{
		}
	}
}
