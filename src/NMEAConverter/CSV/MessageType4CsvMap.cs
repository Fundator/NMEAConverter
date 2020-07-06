using CsvHelper.Configuration;

namespace NMEAConverter.CSV
{
	public class MessageType4CsvMap : ClassMap<MessageType4Csv>
	{
		public MessageType4CsvMap()
		{
			Map(m => m.Epfd).ConvertUsing(m => ((int)m.Epfd).ToString());
			Map(m => m.Mmsi);
			Map(m => m.Latitude);
			Map(m => m.Longitude);
			Map(m => m.FixQuality);
			Map(m => m.Year);
			Map(m => m.Month);
			Map(m => m.Day);
			Map(m => m.Hour);
			Map(m => m.Minute);
			Map(m => m.Second);
			Map(m => m.Repeat).ConvertUsing(m => ((int)m.Repeat).ToString());
			Map(m => m.RaimFlag);
			Map(m => m.RadioStatus);
			Map(m => m.TimestampUtc).ConvertUsing(row => row.TimestampUtc.ToString("yyyy-MM-dd HH:mm:ss"));
		}
	}
}