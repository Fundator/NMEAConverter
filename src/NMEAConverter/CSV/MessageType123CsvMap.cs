using CsvHelper.Configuration;

namespace NMEAConverter.CSV
{
	public class MessageType123CsvMap : ClassMap<MessageType123Csv>
	{
		public MessageType123CsvMap()
		{
			Map(m => m.mmsi);
			Map(m => m.date_time_utc).ConvertUsing(row => row.date_time_utc.ToString("yyyy-MM-dd HH:mm:ss"));
			Map(m => m.lon);
			Map(m => m.lat);
			Map(m => m.sog);
			Map(m => m.cog);
			Map(m => m.true_heading);
			Map(m => m.nav_status);
			Map(m => m.message_nr);
		}
	}
}