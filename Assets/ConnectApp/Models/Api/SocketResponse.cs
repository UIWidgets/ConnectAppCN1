using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConnectApp.Models.Api {
    
    public static class DispatchMsgType {
        public const string INVALID_LS = "INVALID_LS";
        public const string READY = "READY";
        public const string RESUMED = "RESUMED";
        public const string MESSAGE_CREATE = "MESSAGE_CREATE";
        public const string MESSAGE_UPDATE = "MESSAGE_UPDATE";
        public const string MESSAGE_DELETE = "MESSAGE_DELETE";

        public const string CHANNEL_MEMBER_ADD = null;
        public const string CHANNEL_MEMBER_REMOVE = null;
    }

    public class FrameConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType.IsSubclassOf(typeof(Frame<>));
        }

        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer) {
            //TODO
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer) {
            var jObject = JObject.Load(reader);
            var type = jObject["t"].ToString();
            var dataReader = jObject["d"].CreateReader();
            switch (type) {
                case DispatchMsgType.READY:
                    var sessionData = serializer.Deserialize<SocketResponseSessionData>(dataReader);
                    return new SocketResponseSession {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = sessionData
                    };
                case DispatchMsgType.RESUMED:
                    var resumedData = serializer.Deserialize<SocketResponseNullData>(dataReader);
                    return new SocketResponseNull {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = resumedData
                    };
                case DispatchMsgType.MESSAGE_CREATE:
                case DispatchMsgType.MESSAGE_UPDATE:
                case DispatchMsgType.MESSAGE_DELETE:
                    var createMsgData = serializer.Deserialize<SocketResponseMessageData>(dataReader);
                    return new SocketResponseMessage {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = createMsgData
                    };
                default:
                    var defaultData = serializer.Deserialize<SocketResponseNullData>(dataReader);
                    return new SocketResponseNull {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = defaultData
                    };
            }
        }
    }

    [JsonConverter(typeof(FrameConverter))]
    public interface IFrame {
        [JsonProperty("op")] int opCode { get; set; }
        [JsonProperty("s")] int sequence { get; set; }
        [JsonProperty("t")] string type { get; set; }
    }

    public abstract class Frame<T> : IFrame {
        [JsonProperty("d")] public T data { get; set; }
        public int opCode { get; set; }
        public int sequence { get; set; }
        public string type { get; set; }
    }


    public class SocketGatewayResponse {
        public string url;
        public List<string> urls;
    }
    
    public class SocketResponseDataBase {
    }

    public class SocketResponseNullData : SocketResponseDataBase {
    }
    
    public class SocketResponseNull : Frame<SocketResponseNullData> {
    }

    
    public class SocketResponseSessionData : SocketResponseDataBase {
        public string sessionId;
    }
    
    public class SocketResponseSession : Frame<SocketResponseSessionData> {
    }

    //Copy from and should be consistent with Models/Model/ChannelMessage
    public class SocketResponseMessageData : SocketResponseDataBase {
        public string id;
        public string type;
        public string nonce;
        public string channelId;
        public string content;
        public User author;
        public List<Attachment> attachments;
        public bool mentionEveryone;
        public List<User> mentions;
        public bool starred;
        public List<string> replyMessageIds;
        public List<string> lowerMessageIds;
        public List<User> replyUsers;
        public List<User> lowerUsers;
        public List<Reaction> reactions;
        public List<Embed> embeds;
        public bool pending;
        public string deletedTime;
    }

    public class SocketResponseMessage : Frame<SocketResponseMessageData> {
    }
}