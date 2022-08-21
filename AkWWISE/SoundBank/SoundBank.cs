using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AkWWISE.IO.Interfaces;
using AkWWISE.IO.Model;
using AkWWISE.SoundBank.Chunks;
using AkWWISE.SoundBank.Interfaces;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.SoundBank
{
	public class SoundBank : IVisitor, IEnumerable<DataChunk>
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
			}).ToDictionary(chunk => chunk.Header, chunk => chunk);
		}

		#region Visit
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
			FourCC header = reader.Read4CC(); // Find chunk type

			reader.PushOffset();
			uint length = reader.ReadU32();
			reader.PopOffset();

			long nextChunk = reader.Position + length;

			DataChunk chunk = this[header];
			if (chunk is null)
			{
				reader.Seek(nextChunk);
				return;
			}

			chunk.Visit(reader);
			reader.Seek(nextChunk);
		}
		#endregion

		
		public DataChunk this[FourCC header]
		{
			get
			{
				if (chunks.TryGetValue(header, out var chunk))
				{
					return chunk;
				}
				return null;
			}
		}

		#region IEnumerator
		IEnumerator<DataChunk> IEnumerable<DataChunk>.GetEnumerator()
		=> dataChunks.Cast<DataChunk>().GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => dataChunks.GetEnumerator();
		#endregion
	}
}
