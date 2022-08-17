using System.IO;

namespace Distance.Services.Extractors
{
	public interface IExtractor<TDest> where TDest : FileSystemInfo
	{
		void ExtractTo(TDest destination);
	}
}
