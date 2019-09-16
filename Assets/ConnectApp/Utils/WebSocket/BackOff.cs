using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace ConnectApp.Utils {
    public class BackOff {
        
        readonly WebSocketHost host;
        readonly int min;
        readonly int max;

        int _current;
        int _callbackId;
        int _fails;


        public BackOff(WebSocketHost host, int min = 500, int? max = null) {
            this.min = min;
            this.max = max != null ? max.Value : min * 10;
            this.host = host;

            this._current = min;
            this._callbackId = -1;
            this._fails = 0;
        }

        public int fail {
            get { return this._fails; }
        }

        public int current {
            get { return this._current; }
        }

        public bool pending {
            get { return this._callbackId != -1; }
        }

        public void OnSucceed() {
            this.Cancel();
            this._fails = 0;
            this._current = this.min;
        }

        public int OnFail(Action callback = null) {
            this._fails += 1;
            int delay = this._current;
            this._current = Mathf.Min(this._current + delay, this.max);
            if (callback != null) {
                if (this._callbackId != -1) {
                    Debug.Assert(false, "callback already pending !");
                    return 0;
                }

                this._callbackId = this.host.DelayCall(this._current / 1000f, () => {
                    try {
                        callback.Invoke();
                    }
                    finally {
                        this._callbackId = -1;
                    }
                });
            }

            return this._current;
        }

        public void Cancel() {
            if (this._callbackId != -1) {
                this.host.CancelDelayCall(this._callbackId);
                this._callbackId = -1;
            }
        }
    }
}