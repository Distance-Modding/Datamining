using Ardalis.SmartEnum;

namespace AkWWISE.SoundBank.Model
{
	public sealed class DataFormat : SmartEnum<DataFormat>
	{
		private const int VARIABLE_SIZE = -1;
		private const int VARIABLE_MIN1 = 1;

		public static readonly DataFormat
			TYPE_4CC = new DataFormat( 0, "4cc",			 4, "FourCC"),
			TYPE_D64 = new DataFormat( 1, "d64",			 8, "double"),
			TYPE_S64 = new DataFormat( 2, "s64",			 8, "int64_t"),
			TYPE_U64 = new DataFormat( 3, "u64",			 8, "uint64_t"),
			TYPE_S32 = new DataFormat( 4, "s32",			 4, "int32_t"),
			TYPE_U32 = new DataFormat( 5, "u32",			 4, "uint32_t"),
			TYPE_F32 = new DataFormat( 6, "f32",			 4, "float"),
			TYPE_SID = new DataFormat( 7, "sid",			 4, "ShortID (uint32_t)"),
			TYPE_TID = new DataFormat( 8, "tid",			 4, "Target ShortID (uint32_t)"),
			TYPE_UNI = new DataFormat( 9, "uni",			 4, "union (float / int32_t)"),
			TYPE_S16 = new DataFormat(10, "s16",			 2, "int16_t"),
			TYPE_U16 = new DataFormat(11, "u16",			 2, "uint16_t"),
			TYPE_S8  = new DataFormat(12, "s8" ,			 1, "int8_t"),
			TYPE_U8  = new DataFormat(13, "u8" ,			 1, "uint8_t"),
			TYPE_VAR = new DataFormat(14, "var", VARIABLE_MIN1, "Variable Size"),
			TYPE_GAP = new DataFormat(15, "gap", VARIABLE_SIZE, "Byte Gap"),
			TYPE_STR = new DataFormat(16, "str", VARIABLE_SIZE, "String"),
			TYPE_STZ = new DataFormat(17, "stz", VARIABLE_SIZE, "String (null-terminated)");
		
		public int ByteLength { get; }

		public string FriendlyName { get; }

		private DataFormat(int ordinal, string name, int byteLength, string friendlyName)
		: base(name, ordinal)
		{
			FriendlyName = friendlyName;
			ByteLength = byteLength;
		}
	}
}
