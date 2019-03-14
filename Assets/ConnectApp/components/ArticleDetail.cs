using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using ConnectApp.api;
using ConnectApp.components.refresh;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class ArticleDetail : StatelessWidget {
        public ArticleDetail(
            Project articleDetail,
            Key key = null
        ) : base(key) {
            this.articleDetail = articleDetail;
        }

        public readonly Project articleDetail;
        private Article _article;
        private List<Article> _relArticles;
        private User _user;
        
        public override Widget build(BuildContext context) {
            _article = articleDetail.projectData;
            _relArticles = articleDetail.projects;
            _user = StoreProvider.store.state.userState.UserDict[_article.userId];
            return new Container(child: _content(context));
        }

        private Widget _content(BuildContext context) {
            return new Flexible(
//                child:new Refresh(
////                    onFooterRefresh:onFooterRefresh,
////                    child: new ListView(
////                        physics: new AlwaysScrollableScrollPhysics(),
////                        children: new List<Widget> {
////                            _contentHead(context),
////                            _subTitle(context),
////                            _contentDetail(context),
////                            _actionCards(context),
////                            _relatedArticles(context),
////                            _comments(context)
////                        }
////                    )
//                )
                
            );
        }

        

    }
}