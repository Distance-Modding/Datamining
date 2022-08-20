using Ardalis.SmartEnum;

namespace AkWWISE.SoundBank.Model
{
	public sealed class DataType : SmartEnum<DataType>
	{
		private const int VARIABLE_SIZE = -1,
						  VARIABLE_MIN1 = 1;

		public static readonly DataType
			TYPE_4CC = new DataType( 0, "4cc",			   4, "FourCC"),
			TYPE_D64 = new DataType( 1, "d64",			   8, "double"),
			TYPE_S64 = new DataType( 2, "s64",			   8, "int64_t"),
			TYPE_U64 = new DataType( 3, "u64",			   8, "uint64_t"),
			TYPE_S32 = new DataType( 4, "s32",			   4, "int32_t"),
			TYPE_U32 = new DataType( 5, "u32",			   4, "uint32_t"),
			TYPE_F32 = new DataType( 6, "f32",			   4, "float"),
			TYPE_SID = new DataType( 7, "sid",			   4, "ShortID (uint32_t)"),
			TYPE_TID = new DataType( 8, "tid",			   4, "Target ShortID (uint32_t)"),
			TYPE_UNI = new DataType( 9, "uni",			   4, "union (float / int32_t)"),
			TYPE_S16 = new DataType(10, "s16",			   2, "int16_t"),
			TYPE_U16 = new DataType(11, "u16",			   2, "uint16_t"),
			TYPE_S8  = new DataType(12, "s8" ,			   1, "int8_t"),
			TYPE_U8  = new DataType(13, "u8" ,			   1, "uint8_t"),
			TYPE_VAR = new DataType(14, "var", VARIABLE_MIN1, "Variable Size"),
			TYPE_GAP = new DataType(15, "gap", VARIABLE_SIZE, "Byte Gap"),
			TYPE_STR = new DataType(16, "str", VARIABLE_SIZE, "String"),
			TYPE_STZ = new DataType(17, "stz", VARIABLE_SIZE, "String (null-terminated)");
		
		public int ByteLength { get; }

		public string FriendlyName { get; }

		private DataType(int ordinal, string name, int byteLength, string friendlyName)
		: base(name, ordinal)
		{
			FriendlyName = friendlyName;
			ByteLength = byteLength;
		}
	}
}
