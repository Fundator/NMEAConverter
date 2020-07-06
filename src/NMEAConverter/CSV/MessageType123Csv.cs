using System;

namespace NMEAConverter.CSV
{
	public class MessageType123Csv
	{
		public int mmsi;
		public DateTime date_time_utc;
		public double lon;
		public double lat;
		public double sog;
		public double cog;
		public int true_heading;
		public int nav_status;
		public int message_nr;
	}
}