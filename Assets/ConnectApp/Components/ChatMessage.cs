using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class ChatMessage : StatelessWidget {
        public ChatMessage(
            Message message,
            Key key = null
        ) : base(key: key) {
            this.message = message;
        }

        readonly Message message;

        public override Widget build(BuildContext context) {
            if (this.message == null) {
                return new Container();
            }

            var author = this.message.author != null ? this.message.author : new User();

            var messageContent =
                MessageUtils.AnalyzeMessage(this.message.content, this.message.mentions, this.message.mentionEveryone);
            return new Container(
                padding: EdgeInsets.symmetric(6),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        Avatar.User(author, 24),
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