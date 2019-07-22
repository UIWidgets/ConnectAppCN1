using System;
using ConnectApp.Models.Model;

namespace ConnectApp.Utils {
    public enum EventStatus {
        future,
        countDown,
        live,
        past
    }

    public static class DateConvert {
        public static string DateStringFromNow(DateTime dt, bool isLocal = false) {
            TimeSpan span = isLocal ? DateTime.Now - dt : DateTime.UtcNow - dt;
            if (span.TotalDays > 3) {
                return dt.ToString("yyyy-MM-dd");
            }
            else if (span.TotalDays > 1) {
                return $"{(int) Math.Floor(span.TotalDays)}天前";
            }
            else if (span.TotalHours > 1) {
                return $"{(int) Math.Floor(span.TotalHours)}小时前";
            }
            else if (span.TotalMinutes > 1) {
                return $"{(int) Math.Floor(span.TotalMinutes)}分钟前";
            }
            else {
                return "刚刚";
            }
        }

        public static string GetFutureTimeFromNow(string formattedString) {
            if (formattedString == null || formattedString.Length <= 0) {
                return "";
            }

            var date = DateTime.Parse(formattedString);
            var timeSpan = date - DateTime.UtcNow;
            var days = timeSpan.Days;
            var hours = timeSpan.Hours;
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;
            if (days > 0) {
                return $"{days}天{hours}小时";
            }

            if (hours > 0) {
                return $"{hours}小时{minutes}分钟";
            }

            if (minutes > 0) {
                return $"{minutes}分钟{seconds}秒";
            }

            if (seconds > 0) {
                return $"{seconds}秒";
            }

            return "刚刚";
        }

        public static string DateStringFromNonce(string nonce) {
            var startTime = TimeZoneInfo.ConvertTime(new DateTime(2016, 1, 1), TimeZoneInfo.Local);
            var span = Convert.ToInt64(nonce, 16);
            var shifted = (span + 1) >> 22;
            var timespan = (shifted - 1);
            var dt = startTime.AddMilliseconds(timespan);
            return DateStringFromNow(dt, true);
        }

        public static EventStatus GetEventStatus(TimeMap begin) {
            if (begin == null) {
                return EventStatus.future;
            }

            var startDateTime = DateTime.Parse(begin.startTime);
            var endDateTime = DateTime.Parse(begin.endTime);
            var subStartTime = (startDateTime - DateTime.Now).TotalHours;
            var subEndTime = (DateTime.Now - endDateTime).TotalHours;
            if (subStartTime > 1) {
                return EventStatus.future;
            }

            if (subStartTime <= 1 && subStartTime >= 0.0) {
                return EventStatus.countDown;
            }

            if (subStartTime < 0.0 && subEndTime <= 0.0) {
                return EventStatus.live;
            }

            if (subEndTime > 0.0) {
                return EventStatus.past;
            }

            return EventStatus.future;
        }


        public static string formatTime(float time) {
            if (time < 0) {
                return "00:00";
            }

            if (time > 3600) {
                var currentHour = (int) time / 3600;
                var currentMinute = (int) (time - currentHour * 3600) / 60;
                var currentSecond = (int) (time - currentHour * 3600 - currentMinute * 60);
                currentHour = currentHour < 0 ? 0 : currentHour;
                currentMinute = currentMinute < 0 ? 0 : currentMinute;
                currentSecond = currentSecond < 0 ? 0 : currentSecond;
                return string.Format("{0:00}:{1:00}:{2:00}",
                    currentHour, currentMinute, currentSecond);
            }

            var min = (int) time / 60;
            var sec = (int) time % 60;
            min = min < 0 ? 0 : min;
            sec = sec < 0 ? 0 : sec;
            return string.Format("{0:00}:{1:00}", min, sec);
        }
    }
}