using System;
using AkWWISE.IO.Interfaces;

public static class IReaderEx
{
	#region Read Extensions
	public static string ReadS32STR(this IReader reader)
	=> reader.ReadSTR(reader.ReadS32());
	#endregion

	#region Skip Extensions
	public static void SkipD64(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadD64, count);

	public static void SkipF32(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadF32, count);

	public static void SkipS64(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadS64, count);

	public static void SkipU64(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadU64, count);

	public static void SkipS32(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadS32, count);

	public static void SkipU32(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadU32, count);

	public static void SkipS16(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadS16, count);

	public static void SkipU16(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadU16, count);

	public static void SkipS8(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadS8, count);

	public static void SkipU8(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadU8, count);

	public static void Read4CC(this IReader reader, uint count = 1)
	=> SkipRead(reader.Read4CC, count);

	public static void ReadSID(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadSID, count);

	public static void ReadTID(this IReader reader, uint count = 1)
	=> SkipRead(reader.ReadTID, count);

	private static void SkipRead<T>(Func<T> readCallback, uint count = 1)
	{
		for (int i = 0; i < count; ++i)
		{
			readCallback();
		}
	}
	#endregion
}