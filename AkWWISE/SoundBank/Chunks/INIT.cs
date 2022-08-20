using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class INIT : DataChunk
	{
		public INIT(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.INIT;

		public override void Visit(IReader reader)
		{
		}
	}
}
