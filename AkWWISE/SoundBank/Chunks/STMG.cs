using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class STMG : DataChunk
	{
		public STMG(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.STMG;

		public override void Visit(IReader reader)
		{
		}
	}
}
