using AkWWISE.IO.Interfaces;
using AkWWISE.IO.Model;
using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public abstract class DataChunk : IVisitor
	{
		#region Properties
		public SoundBank SoundBank { get; private set; }

		public abstract ChunkType ChunkType { get; }

		public FourCC Header => ChunkType.ChunkHeader;

		public string Name => ChunkType.Name;

		public string Description => ChunkType.FriendlyName;

		public uint Length { get; private set; }
		#endregion

		protected DataChunk(SoundBank soundBank)
		=> SoundBank = soundBank;

		public virtual void Visit(IReader reader)
		{
			Length = reader.ReadU32();
		}
	}
}
