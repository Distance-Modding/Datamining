using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class HIRC : DataChunk
	{
		public HIRC(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.HIRC;

		public override void Visit(IReader reader)
		{
		}
	}
}
