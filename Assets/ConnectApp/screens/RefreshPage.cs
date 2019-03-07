using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ConnectApp.components;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using UnityEngine.Networking;

namespace ConnectApp.screens
{
    public class RefreshPage : StatefulWidget
    {
        public override State createState()
        {
            return new _RefreshPageState();
        }

        public RefreshPage(Key key = null) : base(key)
        {
        }
    }

    class  _RefreshPageState : State<RefreshPage> , TickerProvider
    {
        private List<Widget> _list = new List<Widget>
        {
            new ArticleCard(),
            new ArticleCard(),
            new ArticleCard(),
            new ArticleCard(),
        };

        Promise onFooterRefresh()
        {
//            Window.instance.run(new TimeSpan(0, 0, 0, 5), () =>
//            {
//                
//            });
//            setState(() =>
//            {
//                _list.Add(new ArticleCard());
//                _list.Add(new ArticleCard());
//                _list.Add(new ArticleCard());
//                _list.Add(new ArticleCard());
//            });
            var promise = new Promise();
            Window.instance.startCoroutine(_refresh(promise));
            return promise;
        }

        IEnumerator _refresh(Promise promise)
        {
           yield return null;
           setState(() =>
           {
               _list = new List<Widget>
               {
                   new ArticleCard()
               };
           });
           Debug.Log("asdasdasdasd");
           promise.Resolve();
        }
        
       
        Promise onHeaderRefresh()
        {

            return new Promise();
        }
        
        public override Widget build(BuildContext context)
        {
            return new SafeArea(
             child:new Container(
                 margin:EdgeInsets.only(top:50,bottom:50),
                 color:CColors.White,
                 child:new Refresh(
                     onFooterRefresh:onFooterRefresh,
                     onHeaderRefresh:onHeaderRefresh,
                     child:new ListView(
                         children:  _list
                     )))
             
                
            );
        }

        public Ticker createTicker(TickerCallback onTick)
        {
            Ticker _ticker = new Ticker(onTick, debugLabel: $"created by {this}");
            return _ticker;
        }
    }
    
}