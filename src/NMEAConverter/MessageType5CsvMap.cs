using CsvHelper.Configuration;
using NAisParser;

namespace NMEAConverter
{
	public class MessageType5CsvMap : ClassMap<MessageType5>
	{
		public MessageType5CsvMap()
		{
			Map(m => m.ImoNumber);
			Map(m => m.Mmsi);
			Map(m => m.CallSign);
			Map(m => m.Month);
			Map(m => m.Day);
			Map(m => m.Hour);
			Map(m => m.Minute);
			Map(m => m.Destination);
			Map(m => m.Draught);
			Map(m => m.Dte);
			Map(m => m.Epfd).ConvertUsing(m => ((int)m.Epfd).ToString());
			Map(m => m.Repeat).ConvertUsing(m => ((int)m.Repeat).ToString());
			Map(m => m.ShipType).ConvertUsing(m => ((int)m.ShipType).ToString());
			Map(m => m.ToBow);
			Map(m => m.ToPort);
			Map(m => m.ToStarBoard);
			Map(m => m.ToStern);
			Map(m => m.Version).ConvertUsing(m => ((int)m.Version).ToString());
			Map(m => m.VesselName);
		}
	}
}