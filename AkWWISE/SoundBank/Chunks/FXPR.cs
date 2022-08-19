using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class FXPR : DataChunk
	{
		public FXPR(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.FXPR;

		public override void Visit(IReader reader)
		{
		}
	}
}
