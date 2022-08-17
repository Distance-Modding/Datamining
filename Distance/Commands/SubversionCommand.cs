using System.IO;
using System.Threading.Tasks;
using CliFx;
using CliFx.Attributes;
using CliFx.Exceptions;
using CliFx.Infrastructure;
using Distance.Data;
using Distance.Util;

namespace Distance.Commands
{
    public class SubversionCommand
    {
        [Command("subversion assembly", Description = "Read the game version number from the specified .NET Assembly")]
        public class AssemblySubversionCommand : ICommand
        {
            [CommandParameter(0, Name = "path", Description = "Path to the Assembly-CSharp.dll file containing the game version number")]
            public FileInfo AssemblyPath { get; set; }

            public ValueTask ExecuteAsync(IConsole console)
            {
                if (!AssemblyPath.Exists)
                {
                    throw new CommandException("The specified file does not exist");
                }

                string version = Subversion.GetVersion(AssemblyPath);
                console.Output.WriteLine(version);

                return default;
            }
        }

        [Command("subversion game", Description = "Get the version number of the specified game folder")]
        public class FolderSubversionCommand : ICommand
        {
            [CommandParameter(0, Name = "path", Description = "Path to the root Distance directory")]
            public DirectoryInfo GamePath { get; set; }

            [CommandOption("platform", 'p', Description = "The targetted OS platform")]
            public Platform Platform { get; set; } = Platform.Auto;

            public ValueTask ExecuteAsync(IConsole console)
            {
                if (!GamePath.Exists)
                {
                    throw new CommandException("The specified folder does not exist");
                }

                string version = Subversion.GetVersion(GamePath);
                console.Output.WriteLine(version);

                return default;
            }
        }
    }
}
