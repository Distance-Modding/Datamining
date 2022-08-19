﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkWWISE.Model
{
	public struct FourCC
	{
		#region Constants
		public const int STRUCT_SIZE = 4;
		#endregion

		#region Fields & Properties
		public static readonly Encoding TextEncoding = Encoding.ASCII;

		private readonly byte[] bytes;

		public byte[] Bytes => bytes;

		public char[] Chars => Bytes.Cast<char>().ToArray();

		public string Text => new string(Chars);

		public int Code => ((bytes[0] << 24) | (bytes[1] << 16) | (bytes[2] << 8) | bytes[3]);
		#endregion

		#region Constructors
		public FourCC(string value)
		{
			if (value.Length != STRUCT_SIZE)
			{
				throw new ArgumentException("Text must be of length of 4", nameof(value));
			}
			this = new FourCC(TextEncoding.GetBytes(value));
		}

		public FourCC(byte[] value)
		{
			if (value.Length != STRUCT_SIZE)
			{
				throw new ArgumentException("Byte array must have a length of 4", nameof(value));
			}
			bytes = value.Take(4).ToArray();
		}

		public FourCC(byte a, byte b, byte c, byte d)
		=> bytes = new byte[4] { a, b, c, d };
		#endregion

		#region Override Methods (System.Object)
		public override string ToString() => Text;

		public override bool Equals(object obj)
		=> obj is FourCC cC
		&& EqualityComparer<byte[]>.Default.Equals(bytes, cC.bytes);

		public override int GetHashCode()
		=> Code;
		#endregion

		#region Operators
		#region Type Conversion
		public static implicit operator byte[](FourCC current) => current.Bytes;
		public static implicit operator char[](FourCC current) => current.Chars;
		public static implicit operator string(FourCC current) => current.Text;
		#endregion
		#endregion
	}
}