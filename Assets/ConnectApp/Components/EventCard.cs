using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components {
    public class EventCard : StatelessWidget {
        public EventCard(
            IEvent model,
            string place = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) :base(key: key) {
            this.model = model;
            this.place = place;
            this.onTap = onTap;
        }

        readonly IEvent model;
        readonly string place;
        readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (this.model == null) {
                return new Container();
            }

            const float imageWidth = 114;
            const float imageHeight = 76;
            const float borderRadius = 4;

            var time = Convert.ToDateTime(value: this.model.begin.startTime);
            var hour = $"{time.Hour.ToString().PadLeft(2, '0')}";
            var minute = $"{time.Minute.ToString().PadLeft(2, '0')}";
            var hourMinute = $"{hour}:{minute}";
            var address = this.place ?? "";
            var imageUrl = this.model.avatar ?? this.model.background;
            var card = new Container(
                height: 108,
                padding: EdgeInsets.all(16),
                color: CColors.White,
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            width: 32,
                            margin: EdgeInsets.only(right: 10),
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: new List<Widget> {
                                    new Text(
                                        time.Day.ToString(),
                                        style: new TextStyle(
                                            height: 1.33f,
                                            fontSize: 24,
                                            fontFamily: "Roboto-Bold",
                                            color: CColors.Error
                                        )
                                    ),
                                    new Text(
                                        $"{time.Month.ToString()}月",
                                        style: CTextStyle.CaptionBody
                                    )
                                }
                            )
                        ),
                        new Expanded(
                            child: new Container(
                                margin: EdgeInsets.only(right: 8),
                                child: new Column(
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Container(
                                            margin: EdgeInsets.only(bottom: 8),
                                            child: new Text(this.model.title,
                                                style: CTextStyle.PLargeMedium,
                                                maxLines: 2,
                                                overflow: TextOverflow.ellipsis
                                            )
                                        ),
                                        new Text(this.model.mode == "online"
                                                ? $"{hourMinute} · {this.model.participantsCount}人已预订"
                                                : $"{hourMinute}  · {address}",
                                            style: CTextStyle.PSmallBody3
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(
                            child: new Stack(
                                children: new List<Widget> {
                                    new PlaceholderImage(
                                        imageUrl.EndsWith(".gif")
                                            ? imageUrl
                                            : CImageUtils.SuitableSizeImageUrl(imageWidth, imageUrl),
                                        imageWidth,
                                        imageHeight,
                                        borderRadius,
                                        BoxFit.cover,
                                        color: CColorUtils.GetSpecificDarkColorFromId(id: this.model.id)
                                    ),
                                    new Positioned(
                                        bottom: 0,
                                        right: 0,
                                        child: new ClipRRect(
                                            borderRadius: BorderRadius.only(4, bottomRight: 4),
                                            child: new Container(
                                                width: 41,
                                                height: 24,
                                                decoration: new BoxDecoration(
                                                    gradient: new LinearGradient(
                                                        begin: Alignment.centerLeft,
                                                        end: Alignment.centerRight,
                                                        this.model.mode == "online"
                                                            ? new List<Color> {
                                                                Color.fromARGB(255, 250, 120, 102),
                                                                CColors.SecondaryPink
                                                            }
                                                            : new List<Color> {
                                                                Color.fromARGB(255, 69, 199, 250),
                                                                CColors.PrimaryBlue
                                                            }
                                                    )
                                                ),
                                                alignment: Alignment.center,
                                                child: new Text(
                                                    this.model.mode == "online" ? "线上" : "线下",
                                                    style: CTextStyle.CaptionWhite,
                                                    textAlign: TextAlign.center
                                                )
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            );
            return new GestureDetector(
                child: card,
                onTap: this.onTap
            );
        }
    }
}