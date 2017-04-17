using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizer.Model.Extensions
{
    public static class ObjectExtensions
    {
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
    }
}
