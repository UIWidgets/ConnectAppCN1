using SQLite4Unity3d;
using UnityEngine.Scripting;

namespace ConnectApp.Utils {
    public class FileRecordLite {  
        [Preserve][Indexed][PrimaryKey]
        public string url { get; set; }
        [Preserve]
        public string filepath { get; set; }
    }
}