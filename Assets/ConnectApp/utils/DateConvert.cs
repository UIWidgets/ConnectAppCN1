using System;

namespace ConnectApp.utils {
    public static class DateConvert {
        public static string DateStringFromNow(DateTime dt) {
            TimeSpan span = DateTime.UtcNow - dt;
            if (span.TotalDays > 60)
                return dt.ToShortDateString();
            else if (span.TotalDays > 30)
                return "1个月前";
            else if (span.TotalDays > 14)
                return "2周前";
            else if (span.TotalDays > 7)
                return "1周前";
            else if (span.TotalDays > 1)
                return $"{(int) Math.Floor(span.TotalDays)}天前";
            else if (span.TotalHours > 1)
                return $"{(int) Math.Floor(span.TotalHours)}小时前";
            else if (span.TotalMinutes > 1)
                return $"{(int) Math.Floor(span.TotalMinutes)}分钟前";
            else
                return "刚刚";
        }
        
        public static string DateStringFromNonce(string nonce)
        {
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime( new System.DateTime(2016, 1, 1)); 
            var span = Convert.ToInt64(nonce,16);
            var shifted = (span + 1) >> 22;
            var timespan = (shifted - 1);
            var dt = startTime.AddMilliseconds(timespan);
            return DateStringFromNow(dt) ;
        }

    }
}