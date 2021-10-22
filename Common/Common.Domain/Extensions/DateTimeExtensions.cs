using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Domain
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTimeSeconds(this DateTime now)
        {

            var dateTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, DateTimeKind.Local);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixDateTime = (dateTime.ToUniversalTime() - epoch).TotalSeconds;
            return unixDateTime;
        }

        public static DateTime ToTimeZone(this DateTime now)
        {
            return TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }

        public static DateTime TodayZeroHours(this DateTime date)
        {
            return Convert.ToDateTime(date.ToString("dd/MM/yyyy"));
        }

        public static DateTime TodayHoursZeroMs(this DateTime date)
        {
            return Convert.ToDateTime(date.ToString("dd/MM/yyyy HH:mm"));
        }

        public static DateTime TomorrowZeroHours(this DateTime date)
        {
            return date.TodayZeroHours().AddDays(1);
        }

        public static string FormatTempoEmTimeSheet(int totalHours, int totalMinutes)
        {
            return string.Format("{0}:{1}", totalHours.ToString().PadLeft(2, '0'), totalMinutes.ToString().PadLeft(2, '0'));
        }

        public static int MonthDays(this DateTime data)
        {
            int month = data.Month;
            int year = data.Year;
            return DateTime.DaysInMonth(year, month);
        }
        public static DateTime EndDay(this DateTime data)
        {
            return data.AddDays(1).AddMilliseconds(-1);
        }

        public static DateTime FirstDayInMonth(this DateTime data)
        {
            int dayToday = data.Day - 1;
            return data.AddDays(-dayToday);
        }

        public static DateTime LastDayInMonth(this DateTime data)
        {
            int dayToday = data.Day;
            int lastDayOfMonth = data.MonthDays();
            int daysToAdd = lastDayOfMonth - dayToday;

            return data.AddDays(daysToAdd);
        }

        public static string ToYearSemester(this DateTime? data, string valueIfNull = null)
        {
            if (data == null) return valueIfNull;
            return data.Value.ToYearSemester();
        }

        public static string ToYearSemester(this DateTime data)
        {
            int year = data.Year;
            int semester = data.ToSemester();
            return string.Format("{0}.{1}", year, semester);
        }

        public static int ToSemester(this DateTime data)
        {
            int year = data.Year;
            int month = data.Month;
            int semester = 1;
            if (month > 6) semester = 2;
            return semester;
        }

    }
}
