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
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class EventCard : StatelessWidget {
        public EventCard(
            IEvent model,
            GestureTapCallback onTap = null,
            Key key = null
        ) : base(key) {
            _model = model;
            this.onTap = onTap;
        }

        private readonly IEvent _model;
        private readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            var time = Convert.ToDateTime(_model.createdTime);
            var card = new Container(
                height: 108,
                padding: EdgeInsets.all(16),
                child: new Row(
                    children: new List<Widget> {
                        //date
                        new Container(
                            width: 32,
                            margin: EdgeInsets.only(right: 10),
                            child: new Column(
                                crossAxisAlignment: CrossAxisAlignment.center,
                                children: new List<Widget> {
                                    new Text(time.Day.ToString(), style: new TextStyle(height: 1.33f,
                                        fontSize: 24,
                                        fontFamily: "DINPro-Bold",
                                        color: CColors.secondaryPink)),
                                    new Text($"{time.Month.ToString()}月", style: CTextStyle.PSmall)
                                }
                            )
                        ),

                        //content
                        new Container(
                            width: MediaQuery.of(context).size.width - 196,
                            margin: EdgeInsets.only(right: 8),
                            child: new Column(
                                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: new List<Widget> {
                                    new Container(
                                        child: new Text(_model.title, style: CTextStyle.PLarge, maxLines: 2)
                                    ),

                                    new Text(
                                        _model.live ? $"20:00 · {_model.participantsCount}人已预订" : "20:00 · 旧金山Unity大厦",
                                        style: CTextStyle.PSmall)
                                }
                            )
                        ),
                        //pic
                        new ClipRRect(
                            borderRadius: BorderRadius.all(0),
                            child: new Container(
                                width: 114,
                                height: 76,
                                child: new Stack(
                                    children: new List<Widget> {
                                        new Container(
                                            width: 114,
                                            height: 76,
                                            child: Image.network(_model.background, fit: BoxFit.fill)
                                        ),
                                        new Positioned(
                                            bottom: 0,
                                            right: 0,
                                            child: new Container(
                                                width: 41,
                                                height: 24,
                                                color: _model.live ? CColors.PrimaryBlue : CColors.secondaryPink,
                                                alignment: Alignment.center,
                                                child: new Text(
                                                    _model.live ? "线上" : "线下",
                                                    style: new TextStyle(
//                                                        height: 1.67f,
                                                        fontSize: 12,
                                                        fontFamily: "PingFang-Regular",
                                                        color: CColors.White
                                                    ),
                                                    textAlign: TextAlign.center
                                                )
                                            )
                                        )
                                    }
                                )
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