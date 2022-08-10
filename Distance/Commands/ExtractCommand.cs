using System;
using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Distance.Data;
using Distance.Services.Extractors;
using Distance.Util;

namespace Distance.Commands
{
	[Command("extract", Description = "Extract resources from the game to a specified location")]
    public class ExtractCommand : ICommand
    {
        [CommandParameter(0, Name = "source", Description = "Path of the folder containing the game")]
        public DirectoryInfo Source { get; set; }

        [CommandParameter(1, Name = "destination", Description = "Path where the extracted resources will be written")]
        public DirectoryInfo Destination { get; set; }

        [CommandOption("platform", 'p', Description = "The targetted OS platform")]
        public Platform Platform { get; set; } = Platform.Auto;

        [CommandOption("scan-subdirectories", 'r', Description = "When specified the extractor will scan the source folder subfolders one by one")]
        public bool Recursive { get; set; }

        public ValueTask ExecuteAsync(IConsole console)
            => Recursive
            ? ExtractRecursive(console, Source)
            : ExtractDirectory(console, Source);

        public async ValueTask ExtractRecursive(IConsole console, DirectoryInfo gamesDirectory)
        {
            foreach (DirectoryInfo gameDir in gamesDirectory.GetDirectories())
			{
                if (Game.IsGameDirectory(gameDir)) {
                    await ExtractDirectory(console, gameDir);
				}
			}
        }

        public ValueTask ExtractDirectory(IConsole console, DirectoryInfo gameDir)
		{
            try
			{
                AssetExtractor.Extract(gameDir, Destination, Platform);
			}
            catch (Exception anyException)
			{
                console.Output.WriteLine(anyException.ToString());
			}
            return default;
		}
    }
}
