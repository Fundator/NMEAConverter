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
			public static void ConvertNmeaToCsv(string inputFilename, string outputFilename, bool type123 = true, bool type5 = true)
			{
				var counter = 0.0;
				var success = 0.0;
				var failed = 0.0;
				var filename = Path.GetFileName(inputFilename);
				var messages = new List<MessageType123Csv>();
				using (var reader = new StreamReader(inputFilename))
				{

					var iterator = File.ReadLines(inputFilename);
					var total = iterator.Count();

					Console.WriteLine("Number of lines: " + total);
					var parser = new NAisParser.Parser();

					int lineNo = 0;
					var type123Records = new List<MessageType123Csv>();
					var type5Records = new List<MessageType5>();
					foreach (var line in iterator)
					{
						lineNo++;
						try
						{
							AisResult aisResult = null;
							var timestamp = DateTimeOffset.FromUnixTimeSeconds(int.Parse(line.Split("\\", StringSplitOptions.RemoveEmptyEntries).First().Split(":").Last().Split("*").First())).UtcDateTime;
							var nmeaMessage = line.Split("\\").Last();
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
								Console.WriteLine($"Error in stage 1 on line {lineNo} in file {inputFilename}: " + e.Message);
							}
							if (type123 && aisResult != null && (aisResult.Type == 1 || aisResult.Type == 2 || aisResult.Type == 3))
							{
								try
								{
									//parser = new NAisParser.Parser();
									parser.TryParse(aisResult, out MessageType123 type123Result);
									if (type123Result != null)
									{
										//messages.Add(new MessageType123Csv().MapFrom(type123Result, timestamp));
										type123Records.Add(new MessageType123Csv().MapFrom(type123Result, timestamp));
										//csvWriter.NextRecord();
										//csvWriter.WriteRecord(new MessageType123Csv().MapFrom(type123Result, timestamp));
									}
									success++;
								}
								catch (Exception e)
								{
									Console.WriteLine("Error in stage 2: " + e.Message);
								}
							}
							else if (type5 && aisResult != null && aisResult.Type == 5)
							{
								try
								{
									parser.TryParse(aisResult, out MessageType5 type5Result);
									if (type5Result != null)
									{
										type5Records.Add(type5Result);
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
						catch (Exception e)
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
					if (type123)
					{
						Console.WriteLine($"Completed parsing Type 1, 2 and 3 messages from {inputFilename}, writing..");
						var csvWriter = new CsvWriter(new StreamWriter(outputFilename), CultureInfo.InvariantCulture);
						csvWriter.Configuration.RegisterClassMap<MessageType123CsvMap>();
						csvWriter.Configuration.Delimiter = ";";
						csvWriter.WriteHeader<MessageType123Csv>();
						csvWriter.NextRecord();
						csvWriter.WriteRecords(type123Records);
						Console.WriteLine($"Writing to {outputFilename} completed!");
						csvWriter.Flush();
						csvWriter.Dispose();
					}
					if (type5)
					{
						Console.WriteLine($"Completed parsing Type 5 messages from {inputFilename}, writing..");
						var outputFilenameStatic = outputFilename + ".static";
						var csvWriter = new CsvWriter(new StreamWriter(outputFilenameStatic), CultureInfo.InvariantCulture);
						csvWriter.Configuration.RegisterClassMap<MessageType5CsvMap>();
						csvWriter.Configuration.Delimiter = ";";
						csvWriter.WriteHeader<MessageType5>();
						csvWriter.NextRecord();
						csvWriter.WriteRecords(type5Records);
						Console.WriteLine($"Writing to {outputFilenameStatic} completed!");
						csvWriter.Flush();
						csvWriter.Dispose();
					}
					Console.WriteLine($"Parsing {filename} complete. Succeeded: {(success / counter) * 100.0} Failed: {(failed / counter) * 100.0} Processed: {counter}");
					//Console.WriteLine($"{filename} completed");
				}
			}
		}
	}

}