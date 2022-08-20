using AkWWISE.IO.Model;
using System;
using System.Linq;
using System.Text;

namespace AkWWISE.IO
{
    public class BinaryHelper
    {
        public static readonly BinaryHelper Instance = new BinaryHelper();

        public Endianness Endianness { get; set; }

        public Encoding TextEncoding { get; set; } = Encoding.UTF8;

        public bool IsLittleEndian => Endianness == Endianness.LittleEndian;

        protected byte[] GetBytes(byte[] data)
        {
            if (BitConverter.IsLittleEndian == IsLittleEndian)
            {
                return data;
            }
            return data.Reverse().ToArray();
        }

        #region Value to Byte[]
        public byte[] GetBytes(byte value)
        => new byte[] { value };

        public byte[] GetBytes(sbyte value)
        => GetBytes(Convert.ToByte(value));

        public byte[] GetBytes(bool value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(char value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(short value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(int value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(long value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(ushort value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(uint value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(ulong value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(float value)
        => GetBytes(BitConverter.GetBytes(value));

        public byte[] GetBytes(double value)
        => GetBytes(BitConverter.GetBytes(value));
        #endregion

        #region Byte[] to Value
        public byte ToByte(byte[] data, int startIndex = 0)
        => data[startIndex];

        public byte ToByte(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToByte, endianness, data, startIndex);

        public sbyte ToSByte(byte[] data, int startIndex = 0)
        => Convert.ToSByte(data[startIndex]);

        public sbyte ToSByte(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToSByte, endianness, data, startIndex);

        public char ToChar(byte[] data, int startIndex = 0)
        => BitConverter.ToChar(GetBytes(data), startIndex);

        public char ToChar(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToChar, endianness, data, startIndex);

        public short ToInt16(byte[] data, int startIndex = 0)
        => BitConverter.ToInt16(GetBytes(data), startIndex);

        public short ToInt16(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToInt16, endianness, data, startIndex);

        public int ToInt32(byte[] data, int startIndex = 0)
        => BitConverter.ToInt32(GetBytes(data), startIndex);

        public int ToInt32(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToInt32, endianness, data, startIndex);

        public long ToInt64(byte[] data, int startIndex = 0)
        => BitConverter.ToInt64(GetBytes(data), startIndex);

        public long ToInt64(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToInt64, endianness, data, startIndex);

        public ushort ToUInt16(byte[] data, int startIndex = 0)
        => BitConverter.ToUInt16(GetBytes(data), startIndex);

        public ushort ToUInt16(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToUInt16, endianness, data, startIndex);

        public uint ToUInt32(byte[] data, int startIndex = 0)
        => BitConverter.ToUInt32(GetBytes(data), startIndex);

        public uint ToUInt32(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToUInt32, endianness, data, startIndex);

        public ulong ToUInt64(byte[] data, int startIndex = 0)
        => BitConverter.ToUInt64(GetBytes(data), startIndex);

        public ulong ToUInt64(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToUInt64, endianness, data, startIndex);

        public float ToSingle(byte[] data, int startIndex = 0)
        => BitConverter.ToSingle(GetBytes(data), startIndex);

        public float ToSingle(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToSingle, endianness, data, startIndex);

        public double ToDouble(byte[] data, int startIndex = 0)
        => BitConverter.ToDouble(GetBytes(data), startIndex);

        public double ToDouble(byte[] data, Endianness endianness, int startIndex = 0)
        => Get(ToDouble, endianness, data, startIndex);
        #endregion

        #region Helper
        public Endianness GuessEndiannessInt32(byte[] data)
        {
            int bigEndianInt = ToInt32(data, Endianness.BigEndian);
            int littleEndianInt = ToInt32(data, Endianness.LittleEndian);

            return littleEndianInt > bigEndianInt
                ? Endianness.BigEndian
                : Endianness.LittleEndian;
        }

        public T Get<T>(Func<byte[], int, T> mapper, Endianness endianness, byte[] data, int startIndex)
        {
            Endianness oldEndian = Endianness;
            Endianness = endianness;
            T result = mapper(data, startIndex);
            Endianness = oldEndian;
            return result;
        }
        #endregion
    }
}
