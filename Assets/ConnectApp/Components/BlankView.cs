using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.components {
    public class BlankView : StatelessWidget {
        public BlankView(
            string title,
            bool canRefresh = false,
            GestureTapCallback tapCallback = null,
            Key key = null
        ) : base(key) {
            this.title = title;
            this.canRefresh = canRefresh;
            this.tapCallback = tapCallback;
        }
        
        private readonly bool canRefresh;
        private readonly string title;
        private readonly GestureTapCallback tapCallback;

        public override Widget build(BuildContext context) {
            var isNetWorkError = Application.internetReachability == NetworkReachability.NotReachable;
            var refreshMessage = isNetWorkError ? "点击重试" : "点击刷新";
            var message = isNetWorkError ? "网络连接失败，" : $"{title}，";
            var recognizer = new TapGestureRecognizer {
                onTap = tapCallback
            };
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new RichText(
                            text: new TextSpan(
                                canRefresh ? message : title,
                                CTextStyle.PLargeBody,
                                canRefresh 
                                    ? new List<TextSpan> {
                                        new TextSpan(
                                            refreshMessage,
                                            CTextStyle.PLargeBlue,
                                            recognizer: recognizer
                                        )
                                      }
                                    : new List<TextSpan>()
                            )
                        )
                    }
                )
            );
        }
    }
}