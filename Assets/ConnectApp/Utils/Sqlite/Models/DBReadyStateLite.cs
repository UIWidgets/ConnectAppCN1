using SQLite4Unity3d;
using UnityEngine.Scripting;

namespace ConnectApp.Utils {
    public class DBReadyStateLite {

        public const int DefaultReadyStateKey = 1;

        [Preserve] [PrimaryKey] [Indexed] 
        public int key { get; set; }

        [Preserve]
        public string readyJson { get; set; }
    }
}