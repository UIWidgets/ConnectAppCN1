using System.Collections.Generic;
using ConnectApp.Plugins;

namespace ConnectApp.Utils {
    public class AnalyticsManager {
        // tab点击统计
        public static void TabClick(int fromIndex, int toIndex) {
            List<string> tabs = new List<string> {
                "Article", "Event", "Notification", "Mine"
            };
            List<string> entries = new List<string> {
                "Article_EnterArticle", "Event_EnterEvent", "Notification_EnterNotification", "Mine_EnterMine"
            };
            var eventId = $"Click_Tab_{entries[toIndex]}";
            Dictionary<string, string> extras = new Dictionary<string, string>();
            extras.Add("from", tabs[fromIndex]);
            extras.Add("to", tabs[toIndex]);
            JAnalyticsPlugin.CountEvent(eventId, extras);
        }
    }
}