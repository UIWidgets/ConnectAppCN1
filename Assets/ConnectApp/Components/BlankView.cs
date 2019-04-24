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

        public override Widget build(BuildContext context)
        {
            var isNetWorkError = Application.internetReachability == NetworkReachability.NotReachable ? true:false;
            var refreshMessage = isNetWorkError ? "点击重试" : "点击刷新";
            var message = isNetWorkError ? "网络连接失败，" : $"{title}，";
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new GestureDetector(
                            onTap: tapCallback,
                            child:new Container(
                                child:new RichText(
                                    text:new TextSpan(
                                        canRefresh?message:title,
                                        style:CTextStyle.PLargeBody,
                                        children:canRefresh? new List<TextSpan>
                                        {
                                            new TextSpan(text:refreshMessage,style:CTextStyle.PLargeBlue)
                                        }:new List<TextSpan>()
                                    )
                                )
                            ) 
                       )
                        
                    }
                )
            );
        }

        Widget networkErrorWidget(BuildContext context)
        {
            return new Container(
                color: CColors.White,
                width: MediaQuery.of(context).size.width,
                child: new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    crossAxisAlignment: CrossAxisAlignment.center,
                    children: new List<Widget> {
                        new Text("网络连接失败",style:CTextStyle.PLargeBody),
                        new CustomButton(
                            onPressed:tapCallback,
                            child:new Container(
                                padding: EdgeInsets.symmetric(horizontal: 30, vertical: 7),
                                decoration: new BoxDecoration(
                                    border: Border.all(CColors.TextBody),
                                    borderRadius: BorderRadius.all(20)
                                ),
                                child: new Text(
                                    "重新加载",
                                    style: new TextStyle(
                                        fontSize: 16,
                                        fontFamily: "Roboto-Regular",
                                        color: CColors.TextBody
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