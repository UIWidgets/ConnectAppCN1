using System;
using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Api;
using ConnectApp.Models.Model;
using Newtonsoft.Json;

namespace ConnectApp.Utils {
    public static class MessengerDBApi {

        /*
         *
         * load ReadyState from DB.
         * !!! Note that this will return a new SocketResponseSessionData if nothing is available in the DB
         * 
         */
        public static SocketResponseSessionData SyncLoadReadyState() {
            var readyState = SQLiteDBManager.instance.LoadReadyState();
            if (readyState == null) {
                return new SocketResponseSessionData();
            }

            var readyData = JsonConvert.DeserializeObject<SocketResponseSessionData>(readyState.readyJson);
            return readyData;
        }

        /*
         *
         * save the given ReadyState into DB
         * 
         */
        public static void SyncSaveReadyState(SocketResponseSessionData data) {
            var readyJson = JsonConvert.SerializeObject(data);
            
            SQLiteDBManager.instance.SaveReadyState(
                new DBReadyStateLite {
                    key = DBReadyStateLite.DefaultReadyStateKey ,
                    readyJson = readyJson
                }
            );
        }

        static long MessageIdToKey(string messageId) {
            return string.IsNullOrEmpty(messageId) ? 0 : Convert.ToInt64(messageId, 16);
        }

        /**
         *
         * load messages from DB and convert them into List<ChannelMessageView>. The results are ordered by nonce from new to old
         *
         * channelId: the required channelId
         * 
         * messageNonceFrom: if value = -1, the Api will load the newest $messageCount$ messages; otherwise it will load the newest $messageCount$ messages
         * that are older than $messageNonceFrom$
         * 
         * messageCount: the amount of required messages
         * 
         */
        public static List<ChannelMessageView> SyncLoadMessages(string channelId, long messageNonceFrom = -1, int messageCount = 10) {
            var msgLites = SQLiteDBManager.instance.QueryMessages(channelId, messageNonceFrom, messageCount);

            var msgs = new List<ChannelMessageView>();
            foreach (var msgLite in msgLites) {
                var embedsList = JsonConvert.DeserializeObject<DBEmbedList>(msgLite.embedsJson);
                var mentionList = JsonConvert.DeserializeObject<DBUserList>(msgLite.mentionsJson);
                
                msgs.Add(new ChannelMessageView {
                    id = msgLite.messageId,
                    content = msgLite.content,
                    author = new User {
                        fullName = msgLite.authorName,
                        avatar = msgLite.authorThumb,
                        id = msgLite.authorId
                    },
                    channelId = msgLite.channelId,
                    nonce = msgLite.nonce,
                    time = DateConvert.DateTimeFromNonce(msgLite.nonce),
                    type = (ChannelMessageType) msgLite.type,
                    mentionEveryone = msgLite.mentionEveryone == 1,
                    deleted = msgLite.deleted == 1,
                    embeds = embedsList.embeds,
                    mentions = mentionList.users.Select(user => new User{id = user.id, fullName = user.name, avatar = user.thumb}).ToList()
                });
            }

            return msgs;
        }

        /**
         *
         * save the given message to DB (update its value if it already exists in DB)
         * 
         */
        public static void SyncSaveMessage(ChannelMessageView message) {
            SyncSaveMessages(new List<ChannelMessageView> {message});
        }

        static DBMessageLite ConvertToDbMessageLite(ChannelMessageView message) {
            var embedsList = new DBEmbedList {
                embeds = message.embeds ?? new List<Embed>()
            };

            var embedsJson = JsonConvert.SerializeObject(embedsList);
            
            var mentionsList = new DBUserList {
                users = new List<DBUser>()
            };
            
            if (message.mentions != null) {
                foreach (var metion in message.mentions) {
                    mentionsList.users.Add(new DBUser {
                        id = metion.id,
                        name = metion.fullName,
                        thumb = metion.avatar
                    });
                }
            }

            var metionsJson = JsonConvert.SerializeObject(mentionsList);
            
            return new DBMessageLite {
                messageKey = MessageIdToKey(message.id),
                messageId = message.id,
                content = message.content,
                authorName = message.author.fullName,
                authorThumb = message.author.avatar,
                authorId = message.author.id,
                channelId = message.channelId,
                nonce = message.nonce,
                type = (int) message.type,
                mentionEveryone = message.mentionEveryone ? 1 : 0,
                deleted = message.deleted ? 1 : 0,
                embedsJson = embedsJson,
                mentionsJson = metionsJson
            };
        }

        /**
         *
         * save the given messages to DB (update its value if it already exists in DB)
         * 
         */
        public static void SyncSaveMessages(List<ChannelMessageView> messages) {
            var msgLites = new List<DBMessageLite>();

            foreach (var message in messages) {
                msgLites.Add(ConvertToDbMessageLite(message));
            }
            
            SQLiteDBManager.instance.SaveMessages(msgLites);
        }

        /**
         *
         * save the given messages to DB (update its value if it already exists in DB)
         * 
         */
        public static void SyncSaveMessages(List<ChannelMessage> messages) {
            var msgLites = new List<DBMessageLite>();
            
            foreach (var message in messages) {
                msgLites.Add(ConvertToDbMessageLite(ChannelMessageView.fromChannelMessage(message)));
            }
            
            SQLiteDBManager.instance.SaveMessages(msgLites);
        }
    }
}