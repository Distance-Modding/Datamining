using AkWWISE.Model;
using Ardalis.SmartEnum;

namespace AkWWISE.SoundBank.Model
{
	public sealed class ChunkType : SmartEnum<ChunkType>
	{
		public static readonly ChunkType 
			AKBK = new ChunkType( 0, nameof(AKBK), "Audiokinetic Bank"),
			BKHD = new ChunkType( 1, nameof(BKHD), "Bank Header"),
			HIRC = new ChunkType( 2, nameof(HIRC), "Hierarchy"),
			DATA = new ChunkType( 3, nameof(DATA), "Data"),
			STMG = new ChunkType( 4, nameof(STMG), "Global Settings"),
			DIDX = new ChunkType( 5, nameof(DIDX), "Media Index"),
			FXPR = new ChunkType( 6, nameof(FXPR), "FX Parameters"),
			ENVS = new ChunkType( 7, nameof(ENVS), "Environment Settings"),
			STID = new ChunkType( 8, nameof(STID), "String Mappings"),
			PLAT = new ChunkType( 9, nameof(PLAT), "Custom Platform"),
			INIT = new ChunkType(10, nameof(INIT), "Plugin");

		public string FriendlyName { get; }

		public FourCC ChunkHeader => new FourCC(Name);

		private ChunkType(int ordinal, string name, string friendlyName)
		: base(name, ordinal)
		=> FriendlyName = friendlyName;
	}
}