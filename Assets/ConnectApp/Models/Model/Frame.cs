using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ConnectApp.Models.Model {
    public class OpCode {
        public const int DISPATCH = 0;
        public const int IDENTIFY = 1;
        public const int RESUME = 2;
        public const int STATUS_UPDATE = 3;
        public const int PING = 9;
    }

    public class Frame {
        Frame fromJson(Dictionary<string, object> json) {
            var op = json["op"];
            switch (op) {
                case OpCode.DISPATCH:
                    return DispatchFrame.fromJson(json);
            }

            return null;
        }
    }

    [Serializable]
    public class FrameUser {
//        [JsonProperty("fullname")]
        public string fullname;
        public string id;
        public bool isStaff;
        public string username;
    }

    [Serializable]
    public class IdentifyFrame : Frame {
        [JsonProperty("op")] public int op = OpCode.IDENTIFY;
        [JsonProperty("d")] public _IdentifyFrameData date;
    }

    [Serializable]
    public class _IdentifyFrameData {
        [JsonProperty("ls")] public string sessionId;
        [JsonProperty("commitId")] public string commitId;
        [JsonProperty("properties")] public Dictionary<string, object> properties;
    }

    [Serializable]
    public class PingFrame : Frame {
        public PingFrame() {
        }

        [JsonProperty("op")] public int op = OpCode.PING;
        [JsonProperty("d")] public _PingFrameData data;
    }

    [Serializable]
    public class _PingFrameData {
        public _PingFrameData(int timestamp, string channelId) {
        }

        [JsonProperty("ts")] public int timestamp;
        [JsonProperty("cid")] public string channelId;
    }

    [Serializable]
    public class DispatchFrame : Frame {
        [JsonProperty("op")] public int op = OpCode.DISPATCH;
        [JsonProperty("t")] public string type;

        public static DispatchFrame fromJson(Dictionary<string, object> json) {
            var frame = fromJson(json);
            return null;
        }
    }
}