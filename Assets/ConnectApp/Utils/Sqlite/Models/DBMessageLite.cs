using SQLite4Unity3d;

namespace ConnectApp.Utils {
    public class DBMessageLite {    
        [PrimaryKey]
        public int nonce { get; set; }
        public string messageId { get; set; }
        public string channelId { get; set; }
        
        public string content { get; set; }
        public string authorName { get; set; }
        public string authorThumb { get; set; }
    }
}