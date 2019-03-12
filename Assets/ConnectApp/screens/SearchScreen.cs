using ConnectApp.constants;
using ConnectApp.components;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;
using System.Collections.Generic;
using Unity.UIWidgets.painting;
using TextStyle = Unity.UIWidgets.painting.TextStyle;
using Unity.UIWidgets.rendering;

namespace ConnectApp.screens {
    public class SearchScreen : StatefulWidget {

        public SearchScreen(
            Key key = null
        ) : base(key) {
            
        }
        public override State createState() => new _SearchScreenState();
    }

    internal class _SearchScreenState : State<SearchScreen> {
        
        public override Widget build(BuildContext context){
            return new SafeArea(
                child: new Container(
                    color: CColors.White,
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            _buildSearchBar(context),
                            _buildHotSearch(context),
                            _buildSearchHistory(context)
                        }
                    )
                )
            );
        }

        private static Widget _buildSearchBar(BuildContext context) {
            return new Container(
                padding: EdgeInsets.only(16, 40, 16, 16),
                color: CColors.White,
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            child: new Container(
                                decoration: new BoxDecoration(
                                    CColors.White,
                                    border: Border.all(CColors.Separator2),
                                    borderRadius: BorderRadius.circular(22)
                                ),
                                height: 44,
                                child: new InputField(
                                    style: new TextStyle(
                                        fontSize: 25,
                                        color: CColors.TextThird
                                    ),
                                    autofocus: true,
                                    hintText: "热门搜索",
                                    hintStyle: new TextStyle(
                                        fontSize: 20,
                                        color: CColors.TextThird
                                    ),
                                    cursorColor: CColors.primary,
                                    onChanged: text => {
                                        //
                                    },
                                    onSubmitted: text => {
                                        // 
                                    }
                                )
                            )
                        ),
                        new Container(width: 8),
                        new CustomButton(
                            onPressed: () => { Navigator.pop(context); },
                            child: new Text("取消")
                        )
                    }
                )
            );
        }
        
        private static Widget _buildHotSearch(BuildContext context) {
            List<string> hotSearch = new List<string> {
                "Unity", "Animation", "AR", "Icon", "Component", "Flutter", "C#"
            };
            List<Widget> widgets = new List<Widget>();
            hotSearch.ForEach(item => {
                Widget widget = new Container(
                    decoration: new BoxDecoration(
                        CColors.background2,
                        borderRadius: BorderRadius.circular(11)
                    ),
                    height: 22,
                    padding: EdgeInsets.symmetric(horizontal: 5, vertical: 3),
                    child: new Text(
                        item, 
                        style: new TextStyle(
                            fontSize: 16,
                            color: CColors.White
                        )
                    )
                );
                widgets.Add(widget);
            });
            return new Container(
                padding: EdgeInsets.only(16, 0, 16, 16),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            margin: EdgeInsets.only(top: 16),
                            child: new Text(
                                "热门搜索",
                                style: new TextStyle(
                                    fontSize: 20
                                )
                            )
                        ),
                        new Wrap(
                            spacing: 10,
                            runSpacing: 20,
                            children: widgets
                        )
                    }
                )
            );
        }
        
        private static Widget _buildSearchHistory(BuildContext context) {
            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                color: CColors.White,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        new Container(
                            child: new Text(
                                "搜索历史",
                                style: new TextStyle(
                                    fontSize: 20
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}