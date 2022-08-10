using CliFx;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;

namespace Distance
{
	internal class Program
	{
        public static async Task<int> Main()
        {
            SetCulture();
            return await new CliApplicationBuilder()
                .SetExecutableName("distance")
                .SetTitle("Directive Relay for Distance")
                .SetDescription("A command-line utility to perform various datamining operations on the game Distance")
                .AddCommandsFromThisAssembly()
                .Build()
                .RunAsync();
        }

        internal static void SetCulture()
        {
            Thread thread = Thread.CurrentThread;
            thread.CurrentCulture = CultureInfo.InvariantCulture;
            thread.CurrentUICulture = thread.CurrentCulture;
        }
    }
}