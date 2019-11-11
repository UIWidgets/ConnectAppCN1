using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using SQLite4Unity3d;
using UnityEngine.Scripting;

namespace ConnectApp.Utils {

    [Serializable]
    public class DBEmbedList {
        [Preserve]
        public List<Embed> embeds;
    }

    [Serializable]
    public class DBUser {
        [Preserve] 
        public string id;
        
        [Preserve] 
        public string name;
        
        [Preserve] 
        public string thumb;
    }

    [Serializable]
    public class DBUserList {
        [Preserve] 
        public List<DBUser> users;
    }

    public class DBMessageLite {
        [Preserve][PrimaryKey][Indexed]
        public long messageKey { get; set; }
        
        [Preserve]
        public string messageId { get; set; }
        
        [Preserve]
        public long nonce { get; set; }

        [Preserve]
        public string channelId { get; set; }
        
        [Preserve]
        public int type { get; set; }
        
        [Preserve]
        public int mentionEveryone { get; set; }
        
        [Preserve]
        public int deleted { get; set; }

        [Preserve]
        public string content { get; set; }
        [Preserve]
        public string authorName { get; set; }
        [Preserve]
        public string authorThumb { get; set; }
        [Preserve]
        public string authorId { get; set; }
        
        [Preserve]
        public string embedsJson { get; set; }
        
        [Preserve]
        public string mentionsJson { get; set; }
    }
}