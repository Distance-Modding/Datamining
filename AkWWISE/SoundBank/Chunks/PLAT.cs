using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class PLAT : DataChunk
	{
		public PLAT(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.PLAT;

		public override void Visit(IReader reader)
		{
		}
	}
}
