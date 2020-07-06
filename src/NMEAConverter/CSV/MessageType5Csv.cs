using NAisParser;
using System;

namespace NMEAConverter.CSV
{
	public class MessageType5Csv
	{
		public double Draught { get; set; }
		public int Minute { get; set; }
		public int Hour { get; set; }
		public int Day { get; set; }
		public int Month { get; set; }
		public EpfdFixType Epfd { get; set; }
		public int ToStarBoard { get; set; }
		public int ToPort { get; set; }
		public int ToStern { get; set; }
		public int ToBow { get; set; }
		public ShipType ShipType { get; set; }
		public string VesselName { get; set; }
		public string Destination { get; set; }
		public string CallSign { get; set; }
		public byte Version { get; set; }
		public int Mmsi { get; set; }
		public byte Repeat { get; set; }
		public int ImoNumber { get; set; }
		public bool Dte { get; set; }
		public DateTime TimestampUtc { get; set; }
	}
}