using AkWWISE.SoundBank.Chunks;

namespace AkWWISE.SoundBank
{
	public class SoundBank
	{
		#region Properties and Fields
		#region Serialization
		public AKBK AKBK { get; }

		public BKHD BKHD { get; }

		public HIRC HIRC { get; }

		public DATA DATA { get; }

		public STMG STMG { get; }

		public DIDX DIDX { get; }

		public FXPR FXPR { get; }

		public ENVS ENVS { get; }

		public STID STID { get; }

		public PLAT PLAT { get; }

		public INIT INIT { get; }

		public DataChunk[] Chunks { get; }
		#endregion
		#endregion
		
		internal SoundBank()
		{
			Chunks = new DataChunk[] {
				AKBK = new AKBK(this),
				BKHD = new BKHD(this),
				HIRC = new HIRC(this),
				DATA = new DATA(this),
				STMG = new STMG(this),
				DIDX = new DIDX(this),
				FXPR = new FXPR(this),
				ENVS = new ENVS(this),
				STID = new STID(this),
				PLAT = new PLAT(this),
				INIT = new INIT(this)
			};
		}
	}
}
