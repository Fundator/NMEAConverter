using NAisParser;
using System;

namespace NMEAConverter.CSV
{
	public class MessageType4Csv
	{
		public EpfdFixType Epfd { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public bool FixQuality { get; set; }
		public int Year { get; set; }
		public int Month { get; set; }
		public int Day { get; set; }
		public int Hour { get; set; }
		public int Minute { get; set; }
		public int Second { get; set; }
		public int Mmsi { get; set; }
		public byte Repeat { get; set; }
		public bool RaimFlag { get; set; }
		public int RadioStatus { get; set; }
		public DateTime TimestampUtc { get; set; }
	}
}