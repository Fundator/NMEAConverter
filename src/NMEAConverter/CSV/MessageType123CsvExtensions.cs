using NAisParser;
using System;

namespace NMEAConverter.CSV
{
	public static class MessageType123CsvExtensions
	{
		public static MessageType123Csv MapFrom(this MessageType123Csv self, MessageType123 message, DateTime timestamp)
		{
			self.mmsi = message.Mmsi;
			self.date_time_utc = timestamp;
			self.lon = message.Longitude;
			self.lat = message.Latitude;
			self.sog = message.SpeedOverGround / 10.0;
			self.cog = message.CourseOverGround;
			self.true_heading = message.TrueHeading;
			self.nav_status = (int)message.Status;
			self.message_nr = (int)message.Type;

			return self;
		}
	}
}