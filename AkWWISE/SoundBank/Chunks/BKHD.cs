using System;
using AkWWISE.IO.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank.Chunks
{
	public class BKHD : DataChunk
	{
		#region Initialization
		public BKHD(SoundBank soundBank) : base(soundBank)
		{
		}

		public override ChunkType ChunkType => ChunkType.BKHD;
		#endregion


		#region Data
		#region Backing Fields
		private uint _akVersion;
		#endregion
		public override uint AkVersion => _akVersion;

		public uint AkId { get; protected set; }

		public bool AkInitializationBank { get; protected set; }

		public uint AkBankGeneratorVersion { get; protected set; }

		public uint AkSoundBankId { get; protected set; }

		public uint AkLanguageId { get; protected set; }

		public ulong AkTimeStamp { get; protected set; }

		public bool AkFeedback { get; protected set; }

		public uint AkAltValues { get; protected set; }

		public uint AkAlignment => (AkAltValues << 0x00) & 0xFFFF;

		public uint AkDeviceAllocated => (AkAltValues << 0x10) & 0xFFFF;

		public uint AkProjectId { get; protected set; }

		public uint AkSoundBankType { get; protected set; }

		public byte[] AkBankHash { get; protected set; } = Array.Empty<byte>();
		#endregion

		public override void Visit(IReader reader)
		{
			base.Visit(reader);

			_akVersion = reader.ReadU32();
			AkId = reader.ReadU32();

			if (AkVersion <= 26)
			{
				AkInitializationBank = reader.ReadU32() > 0;
				reader.SkipU32();
				AkBankGeneratorVersion = reader.ReadU32();
			}
			else
			{
				AkBankGeneratorVersion = reader.ReadU32();
				AkSoundBankId = reader.ReadSID();
			}

			if (AkVersion <= 122)
			{
				AkLanguageId = reader.ReadU32();
			}
			else
			{
				AkLanguageId = reader.ReadSID();
			}

			if (AkVersion <= 26)
			{
				AkTimeStamp = reader.ReadU64();
			}
			else if (AkVersion <= 128)
			{
				AkFeedback = reader.ReadU32() > 0;
			}
			else if (AkVersion <= 134)
			{
				AkAltValues = reader.ReadU32();
			}
			else
			{
				AkAltValues = reader.ReadU32();
			}

			if (AkVersion > 76)
			{
				AkProjectId = reader.ReadU32();
			}

			if (AkVersion > 140)
			{
				AkSoundBankType = reader.ReadU32();
				AkBankHash = reader.ReadGAP(0x10);
			}

			if (AkVersion <= 26)
			{
				reader.Skip(Length - 0x18);
			}
			else if (AkVersion <= 76)
			{
				reader.Skip(Length - 0x10);
			}
			else if (AkVersion <= 140)
			{
				reader.Skip(Length - 0x14);
			}
			else
			{
				reader.Skip(Length - 0x28);
			}

			Console.WriteLine($"[BKHD] AkSoundBank SDK v.{AkVersion} | Bank #{AkId} | Project #{AkProjectId}");
		}
	}
}
