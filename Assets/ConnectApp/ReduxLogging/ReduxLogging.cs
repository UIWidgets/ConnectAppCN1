using ConnectApp.redux;
using Newtonsoft.Json;
using UnityEngine;

namespace ConnectApp.redux_logging {
    public class ReduxLogging {
        public static Middleware<State> Create<State>() {
            return (store) => (next) => (action) => {
                var previousState = store.state;
                var previousStateDump = JsonConvert.SerializeObject(previousState);
                var result = next(action);
                var afterState = store.state;
                var afterStateDump = JsonConvert.SerializeObject(afterState);
                Debug.LogFormat("Action name={0} data={1}", action.GetType().Name, JsonUtility.ToJson(action));
                Debug.LogFormat("previousState=\n{0}", previousStateDump);
                Debug.LogFormat("afterState=\n{0}", afterStateDump);
                return result;
            };
        }
    }
}