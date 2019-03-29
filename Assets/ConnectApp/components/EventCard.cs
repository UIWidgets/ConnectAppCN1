using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
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
            this._model = model;
            this.onTap = onTap;
        }

        private readonly IEvent _model;
        private readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context) {
            if (_model == null) return new Container();
            
            var time = Convert.ToDateTime(_model.createdTime);
            var placeId = _model.placeId;
            var address = "";
            if (placeId.isNotEmpty()) {
                var placeDict = StoreProvider.store.state.placeState.placeDict;
                if (placeDict.ContainsKey(placeId)) {
                    var place = placeDict[placeId];
                    address = place.name;
                }
            }
            var imageUrl = _model.avatar != null ? _model.avatar : _model.background;
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
                                                _model.title,
                                                style: CTextStyle.PLargeMedium,
                                                maxLines: 2
                                            )
                                        ),
                                        new Text(
                                            _model.mode == "online"
                                                ? $"{time.Hour}:{time.Minute} · {_model.participantsCount}人已预订"
                                                : $"{time.Hour}:{time.Minute} · {address}",
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
                                    new ClipRRect(
                                        borderRadius: BorderRadius.all(4),
                                        child: new Container(
                                            width: 114,
                                            height: 76,
                                            color: new Color(0xFFD8D8D8),
                                            child: Image.network(imageUrl, fit: BoxFit.fill)
                                        )
                                    ),
                                    new Positioned(
                                        bottom: 0,
                                        right: 0,
                                        child: new ClipRRect(
                                            borderRadius: BorderRadius.only(bottomRight: 4),
                                            child: new Container(
                                                width: 41,
                                                height: 24,
                                                color: _model.mode == "online" ? CColors.PrimaryBlue : CColors.SecondaryPink,
                                                alignment: Alignment.center,
                                                child: new Text(
                                                    _model.mode == "online" ? "线上" : "线下",
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