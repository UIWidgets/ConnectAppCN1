using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.screens {
    public class ChannelScreenConnector : StatelessWidget {
        public ChannelScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            User codeboy = new User {
                id = "codeboy",
                name = "代码小哥",
                avatar =
                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
            };
            User canteen = new User {
                id = "canteen",
                name = "佳能食堂",
                avatar =
                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
            };
            User fish = new User {
                id = "fish",
                name = "海边的孙小鱼",
                avatar =
                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
            };
            User dage = new User {
                id = "dage",
                name = "达哥",
                avatar =
                    "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
            };
            return new StoreConnector<AppState, ChannelScreenViewModel>(
                converter: state => {
                    return new ChannelScreenViewModel {
                        channelInfo = new Channel {
                            imageUrl =
                                "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                            name = "UI Widgets 技术交流",
                            members = new List<User> { },
                            isHot = true,
                            joined = true,
                        },
                        messages = new List<ChannelMessage> {
                            new ChannelMessage {
                                content = "听说Connect App是用UI Widgets做的？",
                                sender = codeboy,
                                time = new DateTime(2019, 9, 9, 7, 30, 10)
                            },
                            new ChannelMessage {
                                content = "是的",
                                sender = canteen,
                                time = new DateTime(2019, 9, 9, 7, 30, 22)
                            },
                            new ChannelMessage {
                                content = "彩蛋这个Demo可以下载吗？看起来很有意思",
                                sender = fish,
                                time = new DateTime(2019, 9, 9, 8, 30, 0)
                            },
                            new ChannelMessage {
                                content = "https://connect-prd-cdn.unity.com/20190830/p/images/9796aa86-b799-4fcc-a2df-ac6d1293ea8e_image1_1_1280x720.jpg",
                                sender = fish,
                                time = new DateTime(2019, 9, 9, 8, 30, 7),
                                type = ChannelMessageType.image
                            },
                            new ChannelMessage {
                                content = "https://connect-prd-cdn.unity.com/20190902/p/images/b961e571-8da0-41aa-9e54-fda0fef95ba8_image2_9.png",
                                sender = dage,
                                time = new DateTime(2019, 9, 9, 8, 30, 8),
                                type = ChannelMessageType.image,
                            },
                            new ChannelMessage {
                                content = "https://connect-prd-cdn.unity.com/20190829/p/images/6a9e4f35-43a2-41ef-bb90-626698ef4876_17.gif",
                                sender = fish,
                                time = new DateTime(2019, 9, 9, 8, 30, 9),
                                type = ChannelMessageType.image,
                            },
                            new ChannelMessage {
                                content = "可以参考这个教程https://unity.com/solutions/game",
                                sender = dage,
                                time = new DateTime(2019, 9, 9, 8, 30, 14)
                            },
                            new ChannelMessage {
                                content = "可以参考这个教程https://unity.com/solutions/game",
                                sender = dage,
                                time = new DateTime(2019, 9, 9, 8, 30, 15)
                            },
                            new ChannelMessage {
                                content = "可以参考这个教程https://unity.com/solutions/game",
                                sender = dage,
                                time = new DateTime(2019, 9, 9, 8, 30, 16)
                            },
                        },
                        me = fish
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new ChannelScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToChannelDetail = () => {
                            dispatcher.dispatch(new MainNavigatorPushToChannelDetailAction());
                        }
                    };
                    return new ChannelScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class ChannelScreen : StatefulWidget {
        public ChannelScreen(
            ChannelScreenViewModel viewModel = null,
            ChannelScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly ChannelScreenViewModel viewModel;
        public readonly ChannelScreenActionModel actionModel;

        public override State createState() {
            return new _ChannelScreenState();
        }
    }

    class _ChannelScreenState : State<ChannelScreen> {
        TextEditingController _fullNameController;
        TextEditingController _titleController;

        Dictionary<string, string> _jobRole;
        float messageBubbleWidth = 0;

        public override void initState() {
            base.initState();
        }

        public override Widget build(BuildContext context) {
            this.messageBubbleWidth = MediaQuery.of(context).size.width * 0.7f;
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(
                                    child: this._buildContent()
                                ),
                                this._buildInputBar(),
                                new Container(height: 34, color: CColors.White)
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                onBack: () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    this.widget.viewModel.channelInfo.name,
                    style: CTextStyle.PXLargeMedium
                ),
                rightWidget: new CustomButton(
                    onPressed: () => { this.widget.actionModel.pushToChannelDetail(); },
                    child: new Container(
                        width: 28,
                        height: 28,
                        child: new Icon(Icons.ellipsis, color: CColors.Icon, size: 28)
                    )
                )
            );
        }

        Widget _buildContent() {
            List<Widget> messages = new List<Widget>();
            for (int i = 0; i < this.widget.viewModel.messages.Count; i++) {
                var message = this.widget.viewModel.messages[i];
                messages.Add(this._buildMessage(
                    message,
                    showTime: i == 0 || (message.time -
                                         this.widget.viewModel.messages[i - 1].time) > TimeSpan.FromMinutes(5),
                    left: message.sender.id != this.widget.viewModel.me.id
                ));
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(top: 16),
                child: new ListView(
                    padding: EdgeInsets.symmetric(16, 0),
                    children: messages
                )
            );
        }

        Widget _buildMessage(ChannelMessage message, bool showTime, bool left) {
            Widget avatar = new Container(
                padding: EdgeInsets.symmetric(0, 10),
                child: Avatar.User(message.sender, size: 40)
            );
            Widget messageContent = new Column(
                crossAxisAlignment: left ? CrossAxisAlignment.start : CrossAxisAlignment.end,
                children: new List<Widget> {
                    new Container(
                        padding: EdgeInsets.only(bottom: 6),
                        child: new Text(message.sender.name, style: CTextStyle.PSmallBody4)
                    ),
                    new Container(
                        constraints: new BoxConstraints(
                            maxWidth: this.messageBubbleWidth
                        ),
                        padding: message.type == ChannelMessageType.text
                            ? EdgeInsets.symmetric(12, 12)
                            : EdgeInsets.zero,
                        decoration: message.type == ChannelMessageType.image
                            ? null
                            : new BoxDecoration(
                                color: left ? CColors.VeryLightPinkThree : CColors.PaleSkyBlue,
                                borderRadius: BorderRadius.all(10)
                            ),
                        child: message.type == ChannelMessageType.text
                            ? new Text(message.content)
                            : message.type == ChannelMessageType.image
                                ? new _ImageMessage(
                                    url: message.content,
                                    size: 140,
                                    ratio: 16.0f / 9.0f
                                )
                                : (Widget) new Container()
                    )
                }
            );
            
            return new Column(
                crossAxisAlignment: left ? CrossAxisAlignment.start : CrossAxisAlignment.end,
                children: new List<Widget> {
                    showTime
                        ? new Container(
                            height: 36,
                            padding: EdgeInsets.only(bottom: 16),
                            child: new Center(
                                child: new Text(
                                    message.time.ToString("HH:mm"),
                                    style: CTextStyle.PSmallBody5
                                )
                            )
                        )
                        : new Container(),
                    new Container(
                        padding: EdgeInsets.only(left: 2, right: 2, bottom: 16),
                        child: new Row(
                            mainAxisAlignment: left ? MainAxisAlignment.start : MainAxisAlignment.end,
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: left
                                ? new List<Widget> {avatar, messageContent}
                                : new List<Widget> {messageContent, avatar}
                        )
                    )
                }
            );
        }

        Widget _buildInputBar() {
            return new Container(
                height: 49,
                padding: EdgeInsets.only(16, right: 10),
                decoration: new BoxDecoration(
                    border: new Border(new BorderSide(CColors.Separator)),
                    color: CColors.White
                ),
                child: new Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Expanded(
                            child: new Container(
                                padding: EdgeInsets.only(16),
                                height: 32,
                                decoration: new BoxDecoration(
                                    CColors.Separator2,
                                    borderRadius: BorderRadius.all(16)
                                ),
                                alignment: Alignment.centerLeft,
                                child: new Container(
                                    child: new Text(
                                        "说点想法...",
                                        style: CTextStyle.PKeyboardTextStyle
                                    )
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => { },
                            child: new Container(
                                width: 44,
                                height: 49,
                                child: new Center(
                                    child: new Icon(Icons.mood, size: 28, color: CColors.Icon)
                                )
                            )
                        ),
                        new CustomButton(
                            padding: EdgeInsets.zero,
                            onPressed: () => { },
                            child: new Container(
                                width: 44,
                                height: 49,
                                child: new Center(
                                    child: new Icon(Icons.outline_event, size: 28, color: CColors.Icon)
                                )
                            )
                        ),
                    }
                )
            );
        }
    }

    class _ImageMessage : StatefulWidget {
        public readonly string url;
        public readonly float size;
        public readonly float ratio;
        public readonly float radius;

        public _ImageMessage(string url, float size, float ratio, float radius = 16) {
            this.url = url;
            this.size = size;
            this.ratio = ratio;
            this.radius = radius;
        }
        public override State createState() {
            return new _ImageMessageState();
        }
    }
    
    class _ImageMessageState : State<_ImageMessage> {
        
        Image image;
        Size size;

        public override void initState() {
            base.initState();
            this.image = Image.network(this.widget.url);
            this.image.image
                .resolve(new ImageConfiguration())
                .addListener((ImageInfo info, bool _) => {
                    if (info.image.width > info.image.height * this.widget.ratio) {
                        this.size = new Size(this.widget.size, this.widget.size / this.widget.ratio);
                    }
                    else if (info.image.width > info.image.height) {
                        this.size = new Size(this.widget.size, 
                            this.widget.size / info.image.width * info.image.height);
                    }
                    else if (info.image.width > info.image.height / this.widget.ratio) {
                        this.size = new Size(this.widget.size / info.image.height * info.image.width,
                            this.widget.size);
                    }
                    else {
                        this.size = new Size(this.widget.size / this.widget.ratio, this.widget.size);
                    }
                    this.setState(() => {});
                });
        }
        
        public override Widget build(BuildContext context) {
            return this.size == null
                ? new Container(width: this.widget.size, height: this.widget.size, decoration: new BoxDecoration(
                    color: CColors.Disable,
                    borderRadius: BorderRadius.all(this.widget.radius)
                ))
                : (Widget) new ClipRRect(
                    borderRadius: BorderRadius.all(this.widget.radius),
                    child: new Container(
                        width: this.size.width,
                        height: this.size.height,
                        child: Image.network(this.widget.url,
                            fit: BoxFit.cover))
                );
        }
    }
}