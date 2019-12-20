using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Scripting;

namespace ConnectApp.Models.Api {
    public static class DispatchMsgType {
        public const string INVALID_LS = "INVALID_LS";
        public const string READY = "READY";
        public const string RESUMED = "RESUMED";
        public const string MESSAGE_CREATE = "MESSAGE_CREATE";
        public const string MESSAGE_UPDATE = "MESSAGE_UPDATE";
        public const string MESSAGE_DELETE = "MESSAGE_DELETE";
        public const string MESSAGE_ACK = "MESSAGE_ACK";
        public const string PRESENCE_UPDATE = "PRESENCE_UPDATE";
        public const string PING = "PING";
        public const string CHANNEL_MEMBER_ADD = "CHANNEL_MEMBER_ADD";
        public const string CHANNEL_MEMBER_REMOVE = "CHANNEL_MEMBER_REMOVE";
        public const string CHANNEL_CREATE = "CHANNEL_CREATE";
        public const string CHANNEL_DELETE = "CHANNEL_DELETE";
        public const string CHANNEL_UPDATE = "CHANNEL_UPDATE";
    }

    public class FrameConverter : JsonConverter {
        [Preserve]
        public FrameConverter() {
        }

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
                case DispatchMsgType.PING:
                    var pingMsgData = serializer.Deserialize<SocketResponsePingData>(dataReader);
                    return new SocketResponsePing {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = pingMsgData
                    };
                case DispatchMsgType.PRESENCE_UPDATE:
                    var presenceUpdateData = serializer.Deserialize<SocketResponsePresentUpdateData>(dataReader);
                    return new SocketResponsePresentUpdate {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = presenceUpdateData
                    };
                case DispatchMsgType.CHANNEL_MEMBER_ADD:
                case DispatchMsgType.CHANNEL_MEMBER_REMOVE:
                    var memberChangeData = serializer.Deserialize<SocketResponseChannelMemberChangeData>(dataReader);
                    return new SocketResponseChannelMemberChange {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = memberChangeData
                    };
                case DispatchMsgType.CHANNEL_CREATE:
                case DispatchMsgType.CHANNEL_DELETE:
                case DispatchMsgType.CHANNEL_UPDATE:
                    var channelDeleteData = serializer.Deserialize<SocketResponseUpdateChannelData>(dataReader);
                    return new SocketResponseUpdateChannel {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = channelDeleteData
                    };
                case DispatchMsgType.MESSAGE_ACK:
                    var messageAckData = serializer.Deserialize<SocketResponseMessageAckData>(dataReader);
                    return new SocketResponseMessageAck {
                        type = type,
                        opCode = int.Parse(jObject["op"].ToString()),
                        sequence = int.Parse(jObject["s"].ToString()),
                        data = messageAckData
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

    [Serializable]
    public class SocketGatewayResponse {
        public string url;
        public List<string> urls;
    }

    [Serializable]
    public class SocketResponseDataBase {
    }

    [Serializable]
    public class SocketResponseNullData : SocketResponseDataBase {
    }

    [Serializable]
    public class SocketResponseNull : Frame<SocketResponseNullData> {
    }

    [Serializable]
    public class SocketResponseSessionData : SocketResponseDataBase {
        public string sessionId;
        public string userId;
        public List<ChannelReadState> readState;
        public List<ChannelMessageLite> lastMessages;
        public List<NormalChannelLite> publicChannels;
        public List<NormalChannelLite> lobbyChannels;
        public List<NormalChannelLite> privateChannels;
        public List<MessageUser> users;
    }

    [Serializable]
    public class ChannelReadState {
        public string channelId;
        public string lastMessageId;
        public string lastMentionId;
        public int mentionCount;
    }

    [Serializable]
    public class NormalChannelLite {
        public string id;
        public string workspaceId;
        public string type;
        public string liveChannelId;
        public bool live;
        public string groupId;
        public string thumbnail;
        public int commentCount;
        public string name;
        public string topic;
        public List<ChannelTag> tags;
        public int memberCount;
        public int onlineMemberCount;
        public bool isArchived;
        public bool isHidden;
        [JsonProperty("readonly")] public bool Readonly;
        public bool isMute;
        public string lastMessageId;
    }

    [Serializable]
    public class ChannelTag {
        public string id;
        public string type;
        public string name;
    }

    [Serializable]
    public class ChannelMessageLite {
        public string id;
        public string type;
        public string channelId;
        public MessageUserLite author;
        public string content;
        public string nonce;
        public bool mentionEveryone;
        public List<MessageUserLite> mentions;
        public List<Attachment> attachments;
        public string deletedTime;
    }

    [Serializable]
    public class MessageUserLite {
        public string id;
    }

    [Serializable]
    public class MessageUser {
        public string id;
        public string username;
        public string fullname;
        public string avatar;
        public string title;
        public string coverImage;
        public string presenceStatus;
        public int likeCount;
        public int followCount;
        public bool isStaff;
        public bool isBot;
    }

    [Serializable]
    public class SocketResponseSession : Frame<SocketResponseSessionData> {
    }

    //Copy from and should be consistent with Models/Model/ChannelMessage
    [Serializable]
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
        public Dictionary<string, int> likeEmojiStats;
        public List<Embed> embeds;
        public bool pending;
        public string deletedTime;
    }

    [Serializable]
    public class SocketResponseMessage : Frame<SocketResponseMessageData> {
    }

    [Serializable]
    public class SocketResponsePresentUpdateData : SocketResponseDataBase {
        public string userId;
        public string channelId;
        public string status;
    }

    [Serializable]
    public class SocketResponsePresentUpdate : Frame<SocketResponsePresentUpdateData> {
    }

    [Serializable]
    public class SocketResponsePingData : SocketResponseDataBase {
        public long ts;
    }

    [Serializable]
    public class SocketResponsePing : Frame<SocketResponsePingData> {
    }

    [Serializable]
    public class SocketResponseChannelMemberChangeData : SocketResponseDataBase {
        public string id;
        public string channelId;
        public string workspaceId;
        public User user;
        public string role;
        public string stickTime;
        public bool isMute;
        public bool isBanned;
        public bool kicked;
        public bool left;
        public bool guideFinished;
        public bool showTerms;
        public int memberCount;
    }

    [Serializable]
    public class SocketResponseChannelMemberChange : Frame<SocketResponseChannelMemberChangeData> {
    }

    [Serializable]
    public class SocketResponseMessageAckData : SocketResponseDataBase {
        public string channelId;
        public string lastMessageId;
        public string lastMentionId;
        public int mentionCount;
    }

    [Serializable]
    public class SocketResponseMessageAck : Frame<SocketResponseMessageAckData> {
    }

    [Serializable]
    public class OnlineMemberCount {
        public int count;
        public int slot;
    }

    [Serializable]
    public class SocketResponseUpdateChannelData : SocketResponseDataBase {
        public string id;
        public string workspaceId;
        public string type;
        public string liveChannelId;
        public string projectId;
        public string ticketId;
        public string proposalId;
        public bool live;
        public string groupId;
        public string thumbnail;
        public int commentCount;
        public string name;
        public string topic;
        public List<ChannelTag> tags;
        public ChannelMessageLite lastMessage;
        public int memberCount;
        public List<OnlineMemberCount> onlineMemberCount;
        public bool isArchived;
        public bool isHidden;
        [JsonProperty("readonly")] public bool Readonly;
        public bool isMute;
    }

    [Serializable]
    public class SocketResponseUpdateChannel : Frame<SocketResponseUpdateChannelData> {
    }
}