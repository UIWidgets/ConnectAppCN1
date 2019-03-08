using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Icons = ConnectApp.constants.Icons;
using UnityEngine;

namespace ConnectApp.screens {
    public class PersonalScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Column(
                    children: new List<Widget> {
                        new CustomNavigationBar(
                            new Text("这是个昵称", style: CTextStyle.H2),
                            new List<Widget> {
                                new Container(
                                    decoration: new BoxDecoration(
                                        borderRadius: BorderRadius.all(20),
                                        border: Border.all(Colors.white),
                                        color: CColors.White
                                    ),
                                    child: new ClipRRect(
                                        borderRadius: BorderRadius.circular(19),
                                        child: new Container(
                                            color: CColors.White,
                                            width: 38,
                                            height: 38,
                                            child: Image.asset(
                                                "mario", fit: BoxFit.cover
                                            )
                                        )
                                    )
                                )
                            },
                            CColors.White,
                            0
                        ),
                        new CustomDivider(
                            color: CColors.Separator2,
                            height: 1
                        ),
                        new Flexible(
                            child: new Container(
                                padding: EdgeInsets.only(bottom: 49),
                                child: new ListView(
                                    children: _buildItems(context)
                                )
                            )
                        )
                    }
                )
            );
        }

        private static List<Widget> _buildItems(BuildContext context) {
            List<PersonalCardItem> personalCardItems = new List<PersonalCardItem> {
                new PersonalCardItem(Icons.book, "我的收藏", () => { Debug.Log("我的收藏"); }),
                new PersonalCardItem(Icons.ievent, "我的活动", () => { Debug.Log("我的活动"); }),
                new PersonalCardItem(Icons.eye, "浏览历史", () => { Navigator.pushNamed(context,"/login"); }),
                new PersonalCardItem(Icons.settings, "设置", () => { Navigator.pushNamed(context,"/setting"); })
            };
            List<Widget> widgets = new List<Widget>();
            personalCardItems.ForEach(item => { widgets.Add(new PersonalCard(item)); });
            return widgets;
        }
    }
}