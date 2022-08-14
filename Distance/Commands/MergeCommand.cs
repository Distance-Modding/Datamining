using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Distance.Services;

namespace Distance.Commands
{
	[Command("merge", Description = "Merges a set of folders into a single one")]
	public class MergeCommand : ICommand
	{
		[CommandParameter(0, Name = "source", Description = "Folder containing a set of subfolders to merge")]
		public DirectoryInfo Source { get; set; }

		[CommandParameter(1, Name = "destination", Description = "Destination of the merged results")]
		public DirectoryInfo Destination { get; set; }

		[CommandOption("sub-folder", 'f', Description = "Specifies a subfolder of each folder to merge to use as the root point")]
		public string SubFolder { get; set; } = string.Empty;

		public ValueTask ExecuteAsync(IConsole console)
		{
			new DirectoryMerger(Source, SubFolder).MergeTo(Destination);
			return default;
		}
	}
}
