using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectApp.Utils {
    public class WebSocketHost : MonoBehaviour {
        readonly Queue<Action> _executionQueue = new Queue<Action>();

        public void Update() {
            lock(this._executionQueue) {
                while (this._executionQueue.Count > 0) {
                    this._executionQueue.Dequeue().Invoke();
                }
            }
        }
        
        public void Enqueue(IEnumerator action) {
            lock (this._executionQueue) {
                this._executionQueue.Enqueue (() => {
                    this.StartCoroutine (action);
                });
            }
        }
        
        public void Enqueue(Action action)
        {
            this.Enqueue(this.ActionWrapper(action));
        }
        
        IEnumerator ActionWrapper(Action action)
        {
            action();
            yield return null;
        }

        static bool _singletonChecker = false;

        WebSocketManager _webSocketManager;

        void Awake() {
            if (_singletonChecker) {
                Debug.Assert(false, "fatal error! Cannot initialize two WebSocketHost!");
                return;
            }
            _singletonChecker = true;
            
            this._webSocketManager = new WebSocketManager(this);
            DontDestroyOnLoad(this.gameObject);
        }
    }
}