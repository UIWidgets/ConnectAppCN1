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
            Message message,
            Key key = null
        ) : base(key) {
            this.message = message;
        }

        private readonly Message message;


        public override Widget build(BuildContext context) {

            var author = message.author != null ? message.author : new User();

            var messageContent = MessageUtils.AnalyzeMessage(message.content, message.mentions, message.mentionEveryone);
            return new Container(
                padding: EdgeInsets.symmetric(6),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        Avatar.User(message.author.id,message.author,24),
                        new Expanded(
                            child: new Container(
                                margin: EdgeInsets.only(8),
                                child: new Container(
                                    child: new RichText(
                                        softWrap: true,
                                        text: new TextSpan(
                                            children: new List<TextSpan> {
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