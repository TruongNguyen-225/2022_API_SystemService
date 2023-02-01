using System;
namespace SystemServiceAPICore3.Utilities
{
	public static class DateTimeHelper
	{
		public static string ConvertDateTimeToString103(this DateTime? dateTime)
		{
			if (dateTime.HasValue)
			{
				return dateTime.Value.ToString("dd/MM/yyyy");
			}

			return String.Empty;
		}
    }
}

