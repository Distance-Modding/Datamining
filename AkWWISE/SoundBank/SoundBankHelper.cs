using System.IO;

namespace AkWWISE.SoundBank
{
	public class SoundBankHelper
	{
		public SoundBank LoadFrom(FileInfo file)
		{
			if (!file.Exists)
			{
				throw new FileNotFoundException("The SoundBank file does not exist.", file.FullName);
			}

			using (Stream stream = File.OpenRead(file.FullName))
			{
				return LoadFrom(stream);
			}
		}

		public SoundBank LoadFrom(Stream stream)
		{
			SoundBank result = new SoundBank();
			using (AkBinaryReader reader = new AkBinaryReader(stream))
			{
				result.Visit(reader);
			}
			return result;
		}
	}
}
