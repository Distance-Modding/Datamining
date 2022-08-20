using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class AKBK : DataChunk
	{
		public AKBK(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.AKBK;

		public override void Visit(IReader reader)
		{
		}
	}
}
