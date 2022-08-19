using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class DATA : DataChunk
	{
		public DATA(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.DATA;

		public override void Visit(IReader reader)
		{
		}
	}
}
