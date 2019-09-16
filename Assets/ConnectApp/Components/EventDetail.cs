using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components {
    public class EventDetail : StatelessWidget {
        public EventDetail(
            bool isShowImage,
            IEvent eventObj = null,
            Action<string> openUrl = null,
            Action<string, bool> playVideo = null,
            Action<string> pushToUserDetail = null,
            Widget topWidget = null,
            Key titleKey = null,
            Key key = null
        ) : base(key: key) {
            this.eventObj = eventObj;
            this.openUrl = openUrl;
            this.playVideo = playVideo;
            this.pushToUserDetail = pushToUserDetail;
            this.isShowImage = isShowImage;
            this.topWidget = topWidget;
            this.titleKey = titleKey;
        }

        readonly IEvent eventObj;
        readonly bool isShowImage;
        readonly Action<string> openUrl;
        readonly Action<string, bool> playVideo;
        readonly Action<string> pushToUserDetail;
        readonly Widget topWidget;
        readonly Key titleKey;

        public override Widget build(BuildContext context) {
            return new Container(child: this._buildContent(context));
        }

        Widget _buildContent(BuildContext context) {
            var items = new List<Widget> {
                this._buildHeadImage(context), this._buildContentHead()
            };
            if (this.topWidget != null) {
                items.Insert(0, this.topWidget);
            }

            items.AddRange(ContentDescription.map(context, this.eventObj.content, this.eventObj.contentMap, null,
                this.openUrl, this.playVideo
            ));
            items.Add(this._buildContentLecturerList());
            return new CustomScrollbar(
                ListView.builder(
                    physics: new AlwaysScrollableScrollPhysics(),
                    itemCount: items.Count,
                    itemBuilder: (cxt, index) => items[index]
                )
            );
        }

        Widget _buildHeadImage(BuildContext context) {
            if (!this.isShowImage) {
                return new Container();
            }

            var imageUrl = this.eventObj.avatar ?? "";
            var aspectRatio = 16.0f / 9;
            if (Application.platform != RuntimePlatform.Android) {
                aspectRatio = 3f / 2;
            }

            return new Container(
                child: new AspectRatio(
                    aspectRatio: aspectRatio,
                    child: new PlaceholderImage(
                        imageUrl.EndsWith(".gif")
                            ? imageUrl
                            : CImageUtils.SuitableSizeImageUrl(MediaQuery.of(context).size.width, imageUrl),
                        fit: BoxFit.cover
                    )
                )
            );
        }

        Widget _buildContentHead() {
            var user = this.eventObj.user ?? new User();
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 16, bottom: 20),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Text(
                            this.eventObj.title ?? "",
                            this.titleKey,
                            style: CTextStyle.H4
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 20),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: new GestureDetector(
                                            onTap: () => this.pushToUserDetail(user.id),
                                            child: Avatar.User(user, 32)
                                        )
                                    ),
                                    new Column(
                                        mainAxisAlignment: MainAxisAlignment.center,
                                        crossAxisAlignment: CrossAxisAlignment.start,
                                        children: new List<Widget> {
                                            new GestureDetector(
                                                onTap: () => this.pushToUserDetail(user.id),
                                                child: new Text(
                                                    user.fullName ?? "",
                                                    style: CTextStyle.PMediumBody
                                                )
                                            ),
                                            new Text(
                                                $"{DateConvert.DateStringFromNow(this.eventObj.createdTime ?? DateTime.Now)}发布",
                                                style: CTextStyle.PSmallBody3
                                            )
                                        }
                                    )
                                }
                            )
                        )
                    }
                )
            );
        }

        Widget _buildContentLecturerList() {
            var hosts = this.eventObj.hosts;
            if (hosts == null || hosts.Count == 0) {
                return new Container();
            }

            var hostItems = new List<Widget> {
                new Container(
                    margin: EdgeInsets.symmetric(horizontal: 16, vertical: 40),
                    height: 1,
                    color: CColors.Separator2
                ),
                new Padding(
                    padding: EdgeInsets.fromLTRB(16, 0, 16, 24),
                    child: new Text(
                        "讲师",
                        style: CTextStyle.H4
                    )
                )
            };
            hosts.ForEach(host => hostItems.Add(this._buildLecture(host: host)));
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: hostItems
            );
        }

        Widget _buildLecture(User host) {
            if (host == null) {
                return new Container();
            }

            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(bottom: 24),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: () => this.pushToUserDetail(obj: host.id),
                            child: new Row(
                                children: new List<Widget> {
                                    new Container(
                                        margin: EdgeInsets.only(right: 8),
                                        child: Avatar.User(user: host, 48)
                                    ),
                                    new Flexible(
                                        child: new Column(
                                            mainAxisAlignment: MainAxisAlignment.center,
                                            crossAxisAlignment: CrossAxisAlignment.start,
                                            children: new List<Widget> {
                                                new Container(
                                                    child: new Text(
                                                        host.fullName ?? host.name ?? "",
                                                        maxLines: 1,
                                                        overflow: TextOverflow.ellipsis,
                                                        style: new TextStyle(
                                                            color: CColors.TextBody,
                                                            fontFamily: "Roboto-Medium",
                                                            fontSize: 16
                                                        )
                                                    )
                                                ),
                                                host.title.isNotEmpty()
                                                    ? new Container(
                                                        child: new Text(
                                                            data: host.title,
                                                            maxLines: 1,
                                                            overflow: TextOverflow.ellipsis,
                                                            style: CTextStyle.PRegularBody3
                                                        )
                                                    )
                                                    : new Container()
                                            }
                                        )
                                    )
                                }
                            )
                        ),
                        new Container(
                            margin: EdgeInsets.only(top: 8),
                            child: new Text(
                                data: host.description,
                                style: new TextStyle(
                                    color: CColors.TextBody3,
                                    fontFamily: "Roboto-Regular",
                                    fontSize: 16
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}