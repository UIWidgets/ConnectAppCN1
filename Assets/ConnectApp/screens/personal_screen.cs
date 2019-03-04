using System;
using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;
using Icons = ConnectApp.constants.Icons;

namespace ConnectApp.screens {
    public class PersonalScreen : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new Stack(
                    children: new List<Widget> {
                        new Positioned(
                            top: 0,
                            left: 0,
                            right: 0,
                            child: new CustomNavigationBar(new Text("这是个昵称", style: CTextStyle.H2),
                                new List<Widget> {
                                    new Container(
                                        decoration: new BoxDecoration(
                                            borderRadius: BorderRadius.all(20),
                                            border: Border.all(Colors.white, 1.0f),
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
                                }, CColors.White,0)
                        ),
                        new Container(
                            padding:EdgeInsets.only(0,140,0,49),
                            child:new ListView(
                                scrollDirection:Axis.vertical,
                                children:_buildItems()
                            )
                        )
                    }
                )
            );
        }

        private List<Widget> _buildItems()
        {
            List <PersonalCardItem> personalCardItems = new List<PersonalCardItem>
            {
                new PersonalCardItem(Icons.book,"我的收藏"),
                new PersonalCardItem(Icons.ievent,"我的活动"),
                new PersonalCardItem(Icons.eye,"浏览历史"),
                new PersonalCardItem(Icons.settings,"设置"),

            };
            List<Widget> widgets = new List<Widget>();
            personalCardItems.ForEach((item) =>
            {
                  widgets.Add(new PersonalCard(item));  
            });
            return widgets;
        }
    }
}