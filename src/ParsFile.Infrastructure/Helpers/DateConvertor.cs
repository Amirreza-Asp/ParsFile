using System.Globalization;

namespace ParsFile.Infrastructure.Helpers
{
	public static class DateConvertor
	{
		public static String ToShamsi(this DateTime dateTime)
		{
			PersianCalendar pc = new PersianCalendar();
			return pc.GetYear(dateTime) + "/" + pc.GetMonth(dateTime).ToString("00") + "/" + pc.GetDayOfMonth(dateTime).ToString("00");
		}
	}
}
