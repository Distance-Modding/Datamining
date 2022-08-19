using System;
using System.IO;
using System.Text;
using AkWWISE.Model;
using AkWWISE.SoundBank.Interfaces;

namespace AkWWISE.SoundBank
{
	public class AkBinaryReader : BinaryReader, IReader
	{
		protected readonly Stream stream;

		protected readonly BinaryHelper Converter = new BinaryHelper();

		#region Properties
		public long Position => stream.Position;

		public long Length => stream.Length;

		public Endianness Endianness
		{
			get => Converter.Endianness;
			set => Converter.Endianness = value;
		}
		#endregion

		#region Constructors
		public AkBinaryReader(Stream input)
		: base(input)
		=> stream = input;

		public AkBinaryReader(Stream input, Encoding encoding)
		: base(input, encoding)
		=> stream = input;


		public AkBinaryReader(Stream input, Encoding encoding, bool leaveOpen)
		: base(input, encoding, leaveOpen)
		=> stream = input;
		#endregion

		public bool IsEOF() => Position >= Length;

		public void Seek(int offset) => stream.Seek(offset, SeekOrigin.Begin);
		
		public void Skip(int bytes) => stream.Seek(bytes, SeekOrigin.Current);
	}
}
