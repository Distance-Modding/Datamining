using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class BKHD : DataChunk
	{
		public BKHD(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.BKHD;

		public override void Visit(IReader reader)
		{
		}
	}
}
