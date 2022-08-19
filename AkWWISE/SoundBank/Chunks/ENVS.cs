using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class ENVS : DataChunk
	{
		public ENVS(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.ENVS;

		public override void Visit(IReader reader)
		{
		}
	}
}
