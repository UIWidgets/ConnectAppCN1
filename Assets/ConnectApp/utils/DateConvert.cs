using System;

namespace ConnectApp.utils {
    public static class DateConvert {
        public static string DateStringFromNow(DateTime dt) {
            TimeSpan span = DateTime.UtcNow - dt;
            if (span.TotalDays > 3)
                return dt.ToString("yyyy-MM-dd");
            else if (span.TotalDays > 1)
                return $"{(int) Math.Floor(span.TotalDays)}天前";
            else if (span.TotalHours > 1)
                return $"{(int) Math.Floor(span.TotalHours)}小时前";
            else if (span.TotalMinutes > 1)
                return $"{(int) Math.Floor(span.TotalMinutes)}分钟前";
            else
                return "刚刚";
        }

        public static string DateStringFromNonce(string nonce) {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(2016, 1, 1));
            var span = Convert.ToInt64(nonce, 16);
            var shifted = (span + 1) >> 22;
            var timespan = (shifted - 1);
            var dt = startTime.AddMilliseconds(timespan);
            return DateStringFromNow(dt);
        }
    }
}