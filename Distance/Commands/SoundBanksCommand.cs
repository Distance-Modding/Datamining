using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using AkWWISE.SoundBank;
using CliFx;
using CliFx.Attributes;
using CliFx.Infrastructure;
using Distance.Services.Extractors;

namespace Distance.Commands
{
	public class SoundBanksCommand
	{
		[Command("soundbank events", Description = "Reads WWISE sound event names from xml files in the game's soundbanks folder")]
		public class SoundBanksXmlCommand : ICommand
		{
			[CommandParameter(0, Name = "source", Description = "Source folder containing the WWISE xml soundbank information files")]
			public DirectoryInfo Source { get; set; }

			[CommandParameter(1, Name = "destination", Description = "Destination file (.xlsx)")]
			public FileInfo Destination { get; set; }

			[CommandOption("sub-folders", 'r', Description = "If the scan should look for xml files recursively")]
			public bool Recursive { get; set; }

			public ValueTask ExecuteAsync(IConsole console)
			{
				WwiseXmlEventsExtractor extractor = new WwiseXmlEventsExtractor();
				console.Output.WriteLine($"Locating XML files...");
				foreach (FileInfo file in Source.GetFiles("*.xml", Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
				{
					console.Output.WriteLine($"Loading XML \"{file.FullName}\"");
					extractor.LoadXml(file);
				}
				extractor.ExtractTo(Destination);
				return default;
			}
		}

		[Command("soundbank bnk", Description = "Reads WWISE sound banks (.bnk)")]
		public class SoundBanksWemCommand : ICommand
		{
			[CommandParameter(0, Name = "source", Description = "Source folder containing the WWISE soundbank files")]
			public DirectoryInfo Source { get; set; }

			public ValueTask ExecuteAsync(IConsole console)
			{
				SoundBankHelper helper = new SoundBankHelper();
				foreach (FileInfo bnkFile in Source.GetFiles("*.bnk"))
				{
					console.Output.WriteLine($"=== {bnkFile.Name} ===");

					try
					{
						SoundBank soundbank = helper.LoadFrom(bnkFile);
						foreach (var chunk in from segment in soundbank where segment.Length > 0 select segment)
						{
							console.Output.WriteLine($"{chunk.Header} ({chunk.Length} bytes) - {chunk.Description}");
						}
					}
					catch (Exception any)
					{
						console.Error.WriteLine(any.Message);
						throw;
					}
				}
				return default;
			}
		}
	
	}
}
