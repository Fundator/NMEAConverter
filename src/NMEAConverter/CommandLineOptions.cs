using CommandLine;
using System.Collections.Generic;

namespace NMEAConverter
{
	internal partial class Program
	{
		public class CommandLineOptions
		{
			[Option('i', "input-directory", Required = true, HelpText = "Path to a directory containing NMEA files to be converted")]
			public string InputDirectory { get; set; }

			[Option('o', "output-directory", Required = true, HelpText = "Path to a directory where output files will be stored")]
			public string OutputDirectory { get; set; }

			[Option('p', "parallellism", Required = true, HelpText = "Maximum parallellism")]
			public int Parallellism { get; set; }

			[Option('t', "types", Separator = ',', Required = true, HelpText = "Message types")]
			public IEnumerable<int> Types { get; set; }


		}
	}
}