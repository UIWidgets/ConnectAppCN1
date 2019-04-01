using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class ChatMessage : StatelessWidget {
        public ChatMessage(
            string channelId,
            string messageId,
            Key key = null
        ) : base(key) {
            this.channelId = channelId;
            this.messageId = messageId;
        }

        private readonly string channelId;
        private readonly string messageId;

        public override Widget build(BuildContext context) {
            if (channelId == null || channelId.Length < 0) return new Container();
            if (messageId == null || messageId.Length < 0) return new Container();
                
            var channelMessageDict = StoreProvider.store.state.messageState.channelMessageDict;
            var messageDict = new Dictionary<string, Message>();
            if (channelMessageDict.ContainsKey(channelId))
                messageDict = channelMessageDict[channelId];
            var message = new Message();
            if (channelMessageDict.ContainsKey(channelId))
                message = messageDict[messageId];

            var author = message.author != null ? message.author : new User();
            
            var messageContent = MessageUtil.AnalyzeMessage(message.content, message.mentions, message.mentionEveryone);
            return new Container(
                padding: EdgeInsets.symmetric(6),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget>{
                        new Avatar(
                            author.id,
                            24
                        ),
                        new Expanded(
                            child: new Container(
                                margin: EdgeInsets.only(8),
                                child: new Container(
                                    child: new RichText(
                                        softWrap: true,
                                        text: new TextSpan(
                                            children: new List <TextSpan>{
                                                new TextSpan(
                                                    $"{author.fullName}:".Replace(' ', '\u00a0'),
                                                    CTextStyle.PMediumBlue
                                                ),
                                                new TextSpan(
                                                    $" {messageContent}",
                                                    CTextStyle.PRegularBody
                                                )
                                            }
                                        )
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}