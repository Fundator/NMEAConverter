using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NMEAConverter
{
	internal partial class Program
	{
		private static object syncRoot = new object();
		private static void Main(string[] args)
		{
			CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args)
			.WithParsed(o =>
			{
				if (!string.IsNullOrWhiteSpace(o.InputDirectory))
				{
					Console.WriteLine($"Trying to convert everything in {o.InputDirectory}");
					var inputDirectory = o.InputDirectory;
					var outputDirectory = string.IsNullOrWhiteSpace(o.OutputDirectory)
							? inputDirectory
							: o.OutputDirectory;

					var filenames = Directory.GetFiles(inputDirectory, "*", SearchOption.AllDirectories);
					var totalCount = filenames.Count();
					var progressCount = 0;
					var completedFiles = new List<string>();
					var completedFilesRegistry = Path.Combine(outputDirectory, "completedfiles.json");
					if (File.Exists(completedFilesRegistry))
					{
						completedFiles = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(completedFilesRegistry));
					}

					Parallel.ForEach(filenames, new ParallelOptions { MaxDegreeOfParallelism = o.Parallellism }, inputFilename =>
					{
						inputFilename = Path.GetFullPath(inputFilename);


						var outputFilename = Path.GetFileNameWithoutExtension(inputFilename) + ".converted.csv";
						var outputFilenameFullPath = Path.Combine(outputDirectory,
															outputFilename);

						if (completedFiles.Contains(outputFilename))
						{
							Console.WriteLine($"Skipping {outputFilename}, already processed");
						}
						else
						{
							NmeaConverter.ConvertNmeaToCsv(inputFilename, outputFilenameFullPath, false, true);
							progressCount++;
							lock (syncRoot)
							{
								Console.WriteLine($"Completed {inputFilename}. Progress: {Math.Round((progressCount / totalCount) * 100.0, 2)}%");
								completedFiles.Add(outputFilename);
								var serialized = JsonSerializer.Serialize(completedFiles);
								File.WriteAllText(completedFilesRegistry, serialized);
							}
						}
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