using CsvHelper;
using NAisParser;
using NMEAConverter.CSV;
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
			public static void ConvertNmeaToCsv(string inputFilename, string outputFilename, bool type123 = true, bool type5 = false, bool type4 = false)
			{
				var counter = 0.0;
				var success = 0.0;
				var failed = 0.0;
				var filename = Path.GetFileName(inputFilename);
				using (var reader = new StreamReader(inputFilename))
				{

					var iterator = File.ReadLines(inputFilename);
					var total = iterator.Count();

					var parser = new NAisParser.Parser();

					var lineNo = 0;
					var type123Records = new List<MessageType123Csv>();
					var type5Records = new List<MessageType5Csv>();
					var type4Records = new List<MessageType4Csv>();
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
									Console.WriteLine("Error parsing type 1/2/3 message: " + e.Message);
								}
							}
							else if (type5 && aisResult != null && aisResult.Type == 5)
							{
								try
								{
									parser.TryParse(aisResult, out MessageType5 type5Result);
									if (type5Result != null)
									{
										type5Records.Add(new MessageType5Csv().MapFrom(type5Result, timestamp));
									}
									success++;
								}
								catch (Exception e)
								{
									Console.WriteLine("Error parsing type 5 message: " + e.Message);
								}
							}
							else if (type4 && aisResult != null && aisResult.Type == 4)
							{
								try
								{
									parser.TryParse(aisResult, out MessageType4 type4Result);
									if (type4Result != null)
									{
										type4Records.Add(new MessageType4Csv().MapFrom(type4Result, timestamp));
									}
									success++;
								}
								catch (Exception e)
								{
									Console.WriteLine("Error parsing type 4 message: " + e.Message);
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
						Console.WriteLine($"Completed parsing Type 1, 2 and 3 messages from {Path.GetFileName(inputFilename)}, writing..");
						var csvWriter = new CsvWriter(new StreamWriter(outputFilename), CultureInfo.InvariantCulture);
						csvWriter.Configuration.RegisterClassMap<MessageType123CsvMap>();
						csvWriter.Configuration.Delimiter = ";";
						csvWriter.WriteHeader<MessageType123Csv>();
						csvWriter.NextRecord();
						csvWriter.WriteRecords(type123Records);
						Console.WriteLine($"Writing to {Path.GetFileName(outputFilename)} completed!");
						csvWriter.Flush();
						csvWriter.Dispose();
					}
					if (type4)
					{
						Console.WriteLine($"Completed parsing Type 4 messages from {Path.GetFileName(inputFilename)}, writing..");
						var outputFilenameStatic = outputFilename + ".type4.static";
						var csvWriter = new CsvWriter(new StreamWriter(outputFilenameStatic), CultureInfo.InvariantCulture);
						csvWriter.Configuration.RegisterClassMap<MessageType4CsvMap>();
						csvWriter.Configuration.Delimiter = ";";
						csvWriter.WriteHeader<MessageType4Csv>();
						csvWriter.NextRecord();
						csvWriter.WriteRecords(type4Records);
						Console.WriteLine($"Writing to {Path.GetFileName(outputFilenameStatic)} completed!");
						csvWriter.Flush();
						csvWriter.Dispose();
					}
					if (type5)
					{
						Console.WriteLine($"Completed parsing Type 5 messages from {Path.GetFileName(inputFilename)}, writing..");
						var outputFilenameStatic = outputFilename + ".type5.static";
						var csvWriter = new CsvWriter(new StreamWriter(outputFilenameStatic), CultureInfo.InvariantCulture);
						csvWriter.Configuration.RegisterClassMap<MessageType5CsvMap>();
						csvWriter.Configuration.Delimiter = ";";
						csvWriter.WriteHeader<MessageType5Csv>();
						csvWriter.NextRecord();
						csvWriter.WriteRecords(type5Records);
						Console.WriteLine($"Writing to {Path.GetFileName(outputFilenameStatic)} completed!");
						csvWriter.Flush();
						csvWriter.Dispose();
					}

					//Console.WriteLine($"Parsing {filename} complete. Succeeded: {(success / counter) * 100.0} Failed: {(failed / counter) * 100.0} Processed: {counter}");
					////Console.WriteLine($"{filename} completed");
				}
			}
		}
	}

}