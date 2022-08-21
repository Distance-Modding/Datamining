using System;
using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class PLAT : DataChunk
	{
		#region Initialization

		public PLAT(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.PLAT;
		#endregion

		#region Data
		public string AkCustomPlatformName { get; private set; }
		#endregion

		public override void Visit(IReader reader)
		{
			base.Visit(reader);

			if (AkVersion <= 136)
			{
				AkCustomPlatformName = reader.ReadS32STR();
			}
			else
			{
				AkCustomPlatformName = reader.ReadSTZ();
			}

			Console.WriteLine($"[PLAT] Platform {AkCustomPlatformName}");
		}
	}
}
