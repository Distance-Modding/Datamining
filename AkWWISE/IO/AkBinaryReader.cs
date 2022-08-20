using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AkWWISE.IO.Interfaces;
using AkWWISE.IO.Model;
using AkWWISE.SoundBank.Model;

namespace AkWWISE.IO
{
	public class AkBinaryReader : BinaryReader, IReader
	{
		#region Fields
		protected static readonly Encoding TextEncoding = Encoding.UTF8;

		protected readonly BinaryHelper Converter = new BinaryHelper();

		protected readonly Stream stream;

		private readonly Stack<long> seekStack = new Stack<long>();
		#endregion

		#region Properties
		public long Position
		{
			get => stream.Position;
			set => stream.Position = value;
		}

		public long Length => stream.Length;

		public Endianness Endianness
		{
			get => Converter.Endianness;
			set => Converter.Endianness = value;
		}

		public byte[] Xorpad { get; set; } = null;
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

		#region Stream Methods
		public bool IsEOF() => Position >= Length;

		public long Seek(long offset) => Seek(offset, false);

		public long Seek(long offset, bool push) => Move(offset, push, SeekOrigin.Begin);

		public long Skip(long bytes) => Skip(bytes, false);

		public long Skip(long offset, bool push) => Move(offset, push, SeekOrigin.Current);

		public long Move(long offset, bool push, SeekOrigin origin)
		{
			if (push)
			{
				PushOffset();
			}
			return stream.Seek(offset, origin);
		}

		public void PushOffset()
		=> seekStack.Push(Position);

		public long PopOffset()
		=> Position = seekStack.Any() ? seekStack.Pop() : Position;
		#endregion

		#region Read Methods
		public void ReadGAP(int bytes)
		{
			long offsetBefore = Position;
			Skip(bytes);
			long offsetAfter = Position;
			if (offsetBefore + bytes != offsetAfter || offsetAfter > Length)
			{
				throw new InvalidOperationException("Can't skip requested bytes (corrupted file?)");
			}
		}

		public string ReadSTR(int size)
		{
			byte[] data = base.ReadBytes(size);

			if (data.Length != size)
			{
				throw new InvalidOperationException("Can't read requested bytes (corrupted file?)");
			}

			return TextEncoding.GetString(data);
		}

		public string ReadSTZ() => base.ReadString();

		public double ReadD64()
		=> Converter.ToDouble(ReadBytes(DataType.TYPE_D64));

		public float ReadF32()
		=> Converter.ToSingle(ReadBytes(DataType.TYPE_F32));

		public long ReadS64()
		=> Converter.ToInt64(ReadBytes(DataType.TYPE_S64));

		public ulong ReadU64()
		=> Converter.ToUInt64(ReadBytes(DataType.TYPE_U64));

		public int ReadS32()
		=> Converter.ToInt32(ReadBytes(DataType.TYPE_S32));

		public uint ReadU32()
		=> Converter.ToUInt32(ReadBytes(DataType.TYPE_U32));

		public short ReadS16()
		=> Converter.ToInt16(ReadBytes(DataType.TYPE_S16));

		public ushort ReadU16()
		=> Converter.ToUInt16(ReadBytes(DataType.TYPE_U16));

		public byte ReadS8()
		=> Converter.ToByte(ReadBytes(DataType.TYPE_S8));

		public sbyte ReadU8()
		=> Converter.ToSByte(ReadBytes(DataType.TYPE_U8));

		public FourCC Read4CC()
		=> new FourCC(ReadBytes(DataType.TYPE_4CC));

		public uint ReadSID() => ReadU32();

		public uint ReadTID() => ReadU32();
		#endregion

		#region Utilities
		public Endianness GuessEndianness()
		=> Endianness = Converter.GuessEndiannessInt32(PeekBytes(4));

		protected byte[] PeekBytes(int count)
		{
			long offsetBefore = Position;
			byte[] data = ReadBytes(count);

			if (data == null || data.Length != count)
			{
				throw new InvalidOperationException("Can't read requested bytes (corrupted file?)");
			}

			Seek(offsetBefore);
			return data;
		}

		public byte[] ReadBytes(DataType type)
		{
			byte[] data = ReadBytes(type.ByteLength);

			if (data == null || data.Length != type.ByteLength)
			{
				throw new InvalidOperationException("Can't read requested bytes (corrupted file?)");
			}

			return data;
		}
		
		protected byte[] Xor(byte[] data)
		{
			if (Xorpad is null || Xorpad.Length == 0)
			{
				return data;
			}

			long offset = Position - data.Length;
			if (offset >= Xorpad.Length)
			{
				return data;
			}

			long max = offset + data.Length;
			if (Position > Xorpad.Length)
			{
				max = Xorpad.Length;
			}

			byte[] result = new byte[data.Length];
			for (long xorOffset = offset, index = 0; xorOffset < max; ++xorOffset, ++index)
			{
				result[index] = (byte)(data[index] ^ Xorpad[xorOffset]);
			}

			return result;
		}
		#endregion

		#region Override Methods
		public override byte[] ReadBytes(int count)
		=> Xor(base.ReadBytes(count));
		#endregion
	}
}
