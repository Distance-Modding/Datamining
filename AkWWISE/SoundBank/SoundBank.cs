using AkWWISE.Model;
using System.IO;
using AkWWISE.SoundBank.Chunks;
using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AkWWISE.SoundBank
{
	public class SoundBank : IVisitor
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

		public readonly DataChunk[] dataChunks;

		public readonly Dictionary<FourCC, DataChunk> chunks;
		#endregion
		#endregion
		
		internal SoundBank()
		{
			chunks = (dataChunks = new DataChunk[] {
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
			}).ToDictionary(chunk => chunk.ChunkHeader, chunk => chunk);
		}

		public void Visit(IReader reader)
		{
			VisitFileHeader(reader);
			VisitChunks(reader);
		}

		protected void VisitFileHeader(IReader reader)
		{
			reader.PushOffset();
			FourCC header = reader.Read4CC();
			reader.PopOffset();

			if (ChunkType.BKHD.ChunkHeader != header)
			{
				throw new InvalidDataException("Unable to find BKHD header.");
			}
		}

		protected void VisitChunks(IReader reader)
		{
			while (!reader.IsEOF())
			{
				VisitChunk(reader);
			}
		}

		protected void VisitChunk(IReader reader)
		{
			FourCC header = reader.Read4CC();
			uint length = reader.ReadU32();
			reader.Skip(length);

			DataChunk chunk = this[header];

			Console.WriteLine($"{header} ({length} bytes) - {chunk.Description}");
		}

		public DataChunk this[FourCC header] => chunks[header];
	}
}
