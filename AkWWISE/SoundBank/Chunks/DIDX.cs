using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class DIDX : DataChunk
	{
		public DIDX(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.DIDX;

		public override void Visit(IReader reader)
		{
		}
	}
}
