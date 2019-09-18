using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectApp.Utils {

    class DelayCall {
        public Action callback;
        public float remainInterval;
        public int id;
    }
    
    public class WebSocketHost : MonoBehaviour {
        readonly Queue<Action> _executionQueue = new Queue<Action>();
        readonly List<DelayCall> _delayCalls = new List<DelayCall>();
        int _delayCallId = 0;

        public void Update() {
            lock(this._executionQueue) {
                while (this._executionQueue.Count > 0) {
                    this._executionQueue.Dequeue().Invoke();
                }
            }

            var delta = Time.deltaTime;
            while (this._delayCalls.Count > 0) {
                var call = this._delayCalls[0];
                var delay = call.remainInterval;
                if (delay > delta) {
                    call.remainInterval -= delta;
                    break;
                }

                delta -= call.remainInterval;
                var callback = call.callback;
                this._delayCalls.RemoveAt(0);
                callback?.Invoke();
            }
        }

        public void CancelDelayCall(int callId) {
            var callIndex = -1;
            for (var i = 0; i < this._delayCalls.Count; i++) {
                if (this._delayCalls[i].id == callId) {
                    if (i != this._delayCalls.Count - 1) {
                        this._delayCalls[i + 1].remainInterval += this._delayCalls[i].remainInterval;
                    }

                    callIndex = i;
                    break;
                }
            }

            if (callIndex != -1) {
                this._delayCalls.RemoveAt(callIndex);
            }
        }

        public int DelayCall(float duration, Action callback) {
            var preInterval = 0.0f;
            for (var i = 0; i < this._delayCalls.Count; i++) {
                if (this._delayCalls[i].remainInterval > duration) {
                    this._delayCalls.Insert(i, new DelayCall {
                        id = this._delayCallId++,
                        callback = callback,
                        remainInterval = duration - preInterval
                    });
                    return this._delayCallId - 1;
                }

                preInterval = this._delayCalls[i].remainInterval;
            }
            
            this._delayCalls.Add(new DelayCall {
                id = this._delayCallId++,
                callback = callback,
                remainInterval = duration - preInterval
            });
            return this._delayCallId - 1;
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

        SocketGateway _socketGateway;

        void Start() {
            if (_singletonChecker) {
                Debug.Assert(false, "fatal error! Cannot initialize two WebSocketHost!");
                return;
            }
            _singletonChecker = true;
            
            this._socketGateway = new SocketGateway(this);
        }
    }
}