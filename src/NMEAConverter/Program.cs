using CommandLine;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NMEAConverter
{
	internal partial class Program
	{
		private static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
			.WithParsed(o =>
			{
				if (!string.IsNullOrWhiteSpace(o.InputDirectory))
				{
					Console.WriteLine($"Trying to convert everything in {o.InputDirectory}");
					var inputDirectory = o.InputDirectory;
					var filenames = Directory.GetFiles(inputDirectory);
					Parallel.ForEach(filenames, inputFilename =>
					{
						inputFilename = Path.GetFullPath(inputFilename);
						var outputDirectory = string.IsNullOrWhiteSpace(o.OutputDirectory)
													? Path.GetDirectoryName(inputFilename)
													: o.OutputDirectory;

						var outputFilename = Path.Combine(outputDirectory,
															Path.GetFileNameWithoutExtension(inputFilename) + ".converted.csv");

						NmeaConverter.ConvertNmeaToCsv(inputFilename, outputFilename);
					});
				}
				else
				{
					Console.WriteLine($"Please specify an input directory using the -i option");
				}
			});
		}
	}

}