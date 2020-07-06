using NAisParser;
using System;

namespace NMEAConverter.CSV
{
	public static class MessageType5CsvExtensions
	{
		public static MessageType5Csv MapFrom(this MessageType5Csv self, MessageType5 message, DateTime timestamp)
		{
			self.Draught = message.Draught;
			self.Minute = message.Minute;
			self.Hour = message.Hour;
			self.Day = message.Day;
			self.Month = message.Month;
			self.Epfd = message.Epfd;
			self.ToStarBoard = message.ToStarBoard;
			self.ToPort = message.ToPort;
			self.ToStern = message.ToStern;
			self.ToBow = message.ToBow;
			self.ShipType = message.ShipType;
			self.VesselName = message.VesselName;
			self.Destination = message.Destination;
			self.CallSign = message.CallSign;
			self.Version = message.Version;
			self.Mmsi = message.Mmsi;
			self.ImoNumber = message.ImoNumber;
			self.Dte = message.Dte;
			self.TimestampUtc = timestamp;

			return self;
		}
	}
}