using System.Collections.Generic;
using System.Linq;
using ConnectApp.Models.Model;
using ConnectApp.Models.ViewModel;
using Newtonsoft.Json;


namespace ConnectApp.Utils {
    public static class MessengerDBApi {   
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
                        avatar = msgLite.authorThumb
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

        public static void SyncSaveMessage(ChannelMessageView messages) {
            SyncSaveMessages(new List<ChannelMessageView> {messages});
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
                messageId = message.id,
                content = message.content,
                authorName = message.author.fullName,
                authorThumb = message.author.avatar,
                channelId = message.channelId,
                nonce = message.nonce,
                type = (int) message.type,
                mentionEveryone = message.mentionEveryone ? 1 : 0,
                deleted = message.deleted ? 1 : 0,
                embedsJson = embedsJson,
                mentionsJson = metionsJson
            };
        }

        public static void SyncSaveMessages(List<ChannelMessageView> messages) {
            var msgLites = new List<DBMessageLite>();

            foreach (var message in messages) {
                msgLites.Add(ConvertToDbMessageLite(message));
            }
            
            SQLiteDBManager.instance.SaveMessages(msgLites);
        }

        public static void SyncSaveMessages(List<ChannelMessage> messages) {
            var msgLites = new List<DBMessageLite>();
            
            foreach (var message in messages) {
                msgLites.Add(ConvertToDbMessageLite(ChannelMessageView.fromChannelMessage(message)));
            }
            
            SQLiteDBManager.instance.SaveMessages(msgLites);
        }
    }
}