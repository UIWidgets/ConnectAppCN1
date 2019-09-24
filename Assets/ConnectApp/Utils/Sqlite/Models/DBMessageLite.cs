using SQLite4Unity3d;
using UnityEngine.Scripting;

namespace ConnectApp.Utils {
    public class DBMessageLite {    
        [Preserve][PrimaryKey][Indexed]
        public long nonce { get; set; }
        
        [Preserve]
        public string messageId { get; set; }
        [Preserve]
        public string channelId { get; set; }
        
        [Preserve]
        public string content { get; set; }
        [Preserve]
        public string authorName { get; set; }
        [Preserve]
        public string authorThumb { get; set; }
    }
}