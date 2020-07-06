using NAisParser;
using System;

namespace NMEAConverter.CSV
{
	public static class MessageType4CsvExtensions
	{
		public static MessageType4Csv MapFrom(this MessageType4Csv self, MessageType4 message, DateTime timestamp)
		{
			self.Epfd = message.Epfd;
			self.Mmsi = message.Mmsi;
			self.Latitude = message.Latitude;
			self.Longitude = message.Longitude;
			self.FixQuality = message.FixQuality;
			self.Year = message.Year;
			self.Month = message.Month;
			self.Day = message.Day;
			self.Hour = message.Hour;
			self.Minute = message.Minute;
			self.Second = message.Second;
			self.Repeat = message.Repeat;
			self.RaimFlag = message.RaimFlag;
			self.RadioStatus = message.RadioStatus;
			self.TimestampUtc = timestamp;
			return self;
		}
	}
}