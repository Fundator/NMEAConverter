using CommandLine;

namespace NMEAConverter
{
	internal partial class Program
	{
		public class CommandLineOptions
		{
			[Option('i', "input-directory", Required = false, HelpText = "Path to a directory containing NMEA files to be converted")]
			public string InputDirectory { get; set; }

			[Option('o', "output-directory", Required = false, HelpText = "Path to a directory where output files will be stored")]
			public string OutputDirectory { get; set; }
		}
	}
}