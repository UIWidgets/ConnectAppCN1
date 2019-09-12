using UnityEngine;

namespace ConnectApp.Utils {
    
    public class WebSocketManager {

        static WebSocketManager m_Instance;

        public static WebSocketManager instance {
            get {
                if (m_Instance != null) {
                    return m_Instance;
                }

                Debug.Assert(false, "fatal error: There is no WebSocketManager available now!");
                return null;
            }
        }
        
        
    }
}