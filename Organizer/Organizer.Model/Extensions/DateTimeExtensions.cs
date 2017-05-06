using System;

namespace Organizer.Model.Extensions
{
    public static class DateTimeExtensions
    {
        const string FORMAT_DATETIME = "dd.MM.yyyy hh:mm";

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfDay(this DateTime dateTime)
        {
            dateTime = dateTime.AddHours(23 - dateTime.Hour);
            dateTime = dateTime.AddMinutes(59 - dateTime.Minute);
            return dateTime;
        }

        public static DateTime StartOfDay(this DateTime dateTime)
        {
            dateTime = dateTime.AddHours(-dateTime.Hour);
            dateTime = dateTime.AddMinutes(-dateTime.Minute);
            return dateTime;
        }

        public static string ToDateString(this DateTime dateTime)
        {
            return dateTime.ToString(FORMAT_DATETIME);
        }
    }
}
