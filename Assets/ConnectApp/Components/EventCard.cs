using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class EventCard : StatelessWidget {
        public EventCard(
            IEvent model,
            string place = null,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key) {
            this.model = model;
            this.place = place;
            this.onTap = onTap;
        }

        private readonly IEvent model;
        private readonly string place;
        private readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (model == null) return new Container();

            var time = Convert.ToDateTime(model.begin.startTime);
            var hour = $"{time.Hour.ToString().PadLeft(2, '0')}";
            var minute = $"{time.Minute.ToString().PadLeft(2, '0')}";
            var hourMinute = $"{hour}:{minute}";
            var address = place == null ? "" : place;
            var imageUrl = model.avatar != null ? model.avatar : model.background;
            var card = new Container(
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
                                            color: CColors.SecondaryPink
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
                                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                    crossAxisAlignment: CrossAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Container(
                                            margin: EdgeInsets.only(bottom: 8),
                                            child: new Text(
                                                model.title,
                                                style: CTextStyle.PLargeMedium,
                                                maxLines: 2
                                            )
                                        ),
                                        new Text(
                                            model.mode == "online"
                                                ? $"{hourMinute} · {model.participantsCount}人已预订"
                                                : $"{hourMinute}  · {address}",
                                            style: CTextStyle.PSmallBody3
                                        )
                                    }
                                )
                            )
                        ),
                        new Container(
                            width: 114,
                            height: 76,
                            child: new Stack(
                                children: new List<Widget> {
                                    new PlaceholderImage(
                                        imageUrl.EndsWith(".gif")?imageUrl:$"{imageUrl}.300x0x1.jpg",
                                        114,
                                        76,
                                        4,
                                        BoxFit.cover
                                    ),
                                    new Positioned(
                                        bottom: 0,
                                        right: 0,
                                        child: new ClipRRect(
                                            borderRadius: BorderRadius.only(bottomRight: 4),
                                            child: new Container(
                                                width: 41,
                                                height: 24,
                                                color: model.mode == "online"
                                                    ? CColors.SecondaryPink
                                                    : CColors.PrimaryBlue,
                                                alignment: Alignment.center,
                                                child: new Text(
                                                    model.mode == "online" ? "线上" : "线下",
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
                onTap: onTap
            );
        }
    }
}