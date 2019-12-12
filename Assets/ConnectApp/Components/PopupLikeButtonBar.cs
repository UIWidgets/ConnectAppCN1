using System;
using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {

    public class PopupLikeButtonItem {
        public string type;
        public string content;
        public bool selected;
    }
    
    public class PopupLikeButtonBar : StatefulWidget {

        public delegate void OnTapPopupLikeButtonCallback(string type);

        public PopupLikeButtonBar(
            OnTapPopupLikeButtonCallback onTap,
            List<PopupLikeButtonItem> popupLikeButtonData,
            Key key = null
        ) : base(key: key) {
            this.onTap = onTap;
            this.popupLikeButtonData = popupLikeButtonData;
        }

        public readonly OnTapPopupLikeButtonCallback onTap;
        public readonly List<PopupLikeButtonItem> popupLikeButtonData;
        
        public override State createState() {
            return new _PopupLikeButtonBarState();
        }
    }

    class _PopupLikeButtonBarState : TickerProviderStateMixin<PopupLikeButtonBar> {

        AnimationController _barSizeController;
        Animation<float> _barSize;
        AnimationController _buttonSizeController;
        Animation<float> _buttonSize;

        public override void initState() {
            base.initState();
            this._barSizeController = new AnimationController(duration: TimeSpan.FromMilliseconds(250), vsync: this);
            this._barSize = new FloatTween(0.7f, 1.0f).animate(this._barSizeController);
            this._barSize.addListener(() => this.setState());
            
            this._buttonSizeController = new AnimationController(duration: TimeSpan.FromMilliseconds(500), vsync: this);
            this._buttonSize = new FloatTween(0, 1.0f).animate(this._buttonSizeController);
            this._buttonSize.addListener(() => this.setState());
            
            this._barSizeController.animateTo(1, curve: Curves.easeOutQuart).whenCompleteOrCancel(() => {
                this._buttonSizeController.animateTo(1, curve: Curves.easeOutQuart);
            });
        }

        public override void dispose() {
            this._barSizeController.dispose();
            this._buttonSizeController.dispose();
            base.dispose();
        }

        public override Widget build(BuildContext context) {
            List<Widget> popupLikeButtons = new List<Widget>();
            for (int i = 0; i < this.widget.popupLikeButtonData.Count; i++) {
                var popupLikeButtonItem = this.widget.popupLikeButtonData[i];
                popupLikeButtons.Add(this._buildPopupLikeButton(
                    popupLikeButtonItem.content,
                    popupLikeButtonItem.selected,
                    i == 0,
                    i == this.widget.popupLikeButtonData.Count - 1,
                    () => { this.widget.onTap(popupLikeButtonItem.type);}
                ));
                
            }
            Widget result = new Container(
                height: 52,
                decoration: new BoxDecoration(
                    borderRadius: BorderRadius.all(26),
                    color: CColors.White
                ),
                child: new Container(
                    decoration: new BoxDecoration(
                        borderRadius: BorderRadius.all(26),
                        color: CColors.White.withOpacity(0.25f)
                    ),
                    padding: EdgeInsets.symmetric(4, 12),
                    child: new Row(
                        mainAxisSize: MainAxisSize.min,
                        children: popupLikeButtons))
            );
            
            // These two fractional translation exist to make the size animation
            // pivoted at the center
            result = new FractionalTranslation(
                translation: new Offset(0.5f, 0.5f),
                child: new Transform(
                    transform: Matrix3.makeScale(this._barSize.value), 
                    child: new FractionalTranslation(
                        translation: new Offset(-0.5f, -0.5f),
                        child: result
                    )
                )
            );
            return result;
        }

        float _buttonSizeValue {
            get {
                return 36 * (this._buttonSize.value < 0.8f
                    ? this._buttonSize.value / 0.8f * 1.1f
                    : 1.1f - (this._buttonSize.value - 0.8f) * 0.5f);
            }
        }
        
        Widget _buildPopupLikeButton(
            string content,
            bool selected,
            bool leftMost,
            bool rightMost,
            GestureTapCallback onTap) {
            return new GestureDetector(
                onTap: onTap,
                child: new Container(
                    padding: EdgeInsets.only(left: leftMost ? 0 : 2, right: rightMost ? 0 : 2),
                    child: new Container(
                        width: 44,
                        height: 44,
                        decoration: new BoxDecoration(
                            borderRadius: BorderRadius.all(12),
                            color: selected ? CColors.Separator2 : CColors.Transparent),
                        child: new Center(
                            child: Image.asset(
                                content,
                                width: this._buttonSizeValue,
                                height: this._buttonSizeValue)
                        )
                    )
                )
            );
        }
    }
}