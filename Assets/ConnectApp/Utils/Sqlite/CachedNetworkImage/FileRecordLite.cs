using SQLite4Unity3d;

namespace System {
    public class FileRecordLite {  
        [PrimaryKey][Indexed]
        public string url { get; set; }
        
        public string filepath { get; set; }
    }
}