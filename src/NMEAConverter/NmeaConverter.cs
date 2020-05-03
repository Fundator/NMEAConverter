using CsvHelper;
using NAisParser;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NMEAConverter
{
	internal partial class Program
	{
		public static class NmeaConverter
		{
			public static void ConvertNmeaToCsv(string inputFilename, string outputFilename)
			{
				var counter = 0.0;
				var success = 0.0;
				var failed = 0.0;
				var filename = Path.GetFileName(inputFilename);
				var messages = new List<MessageType123Csv>();
				using (var reader = new StreamReader(inputFilename))
				using (var writer = new StreamWriter(outputFilename))
				using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
				{
					csvWriter.Configuration.RegisterClassMap<MessageType123CsvMap>();
					csvWriter.Configuration.Delimiter = ";";
					var iterator = File.ReadLines(inputFilename);
					var total = iterator.Count();

					Console.WriteLine("Number of lines: " + total);
					var parser = new NAisParser.Parser();
					csvWriter.WriteHeader<MessageType123Csv>();
					foreach (var line in iterator)
					{
						var timestamp = DateTimeOffset.FromUnixTimeSeconds(int.Parse(line.Split("\\", StringSplitOptions.RemoveEmptyEntries).First().Split(":").Last().Split("*").First())).UtcDateTime;
						var nmeaMessage = line.Split("\\").Last();
						try
						{
							AisResult aisResult = null;
							try
							{
								// NMEA messages can be fragmented over multiple lines
								// so upon failure, try to parse another line of data
								if (!parser.TryParse(nmeaMessage, out aisResult))
								{
									continue;
								}
							}
							catch (Exception e)
							{
								Console.WriteLine("Error in stage 1: " + e.Message);
							}
							if (aisResult != null && (aisResult.Type == 1 || aisResult.Type == 2 || aisResult.Type == 3))
							{
								try
								{
									parser = new NAisParser.Parser();
									parser.TryParse(aisResult, out MessageType123 type123Result);
									if (type123Result != null)
									{
										//messages.Add(new MessageType123Csv().MapFrom(type123Result, timestamp));
										csvWriter.NextRecord();
										csvWriter.WriteRecord(new MessageType123Csv().MapFrom(type123Result, timestamp));
									}
									success++;
								}
								catch (Exception e)
								{
									Console.WriteLine("Error in stage 2: " + e.Message);
								}
							}
							else
							{
								success++;
							}
						}
						catch (ArgumentException e)
						{
							failed++;
							if (e.InnerException != null)
							{
								Console.WriteLine("Inner exception: " + e.InnerException.Message);
							}
							Console.WriteLine("Outer exception: " + e.Message);
						}
						if (counter % 100000 == 0)
						{
							Console.WriteLine($"{filename} progress: {Math.Round((counter / total) * 100.0, 2)}%");
						}
						counter++;
					}
					csvWriter.Flush();
					Console.WriteLine($"Parsing {filename} complete. Succeeded: {(success / counter) * 100} Failed: {(failed / counter) * 100} Processed: {counter}");
					Console.WriteLine($"{filename} completed");
				}
			}
		}
	}

}