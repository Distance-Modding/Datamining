using AkWWISE.Model;

namespace AkWWISE.SoundBank.Interfaces
{
	public interface IReader
	{
		long Position { get; }

		long Length { get; }

		Endianness Endianness { get; set; }

		bool IsEOF();

		void Seek(int offset);

		void Skip(int bytes);
	}
}
