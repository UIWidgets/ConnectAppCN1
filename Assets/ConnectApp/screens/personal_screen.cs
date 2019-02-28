using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

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
                            child: new CustomNavigationBar(new Text("这是个昵称", style: CTextStyle.H2White),
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
                                                decoration: new BoxDecoration(),
                                                color: CColors.White,
                                                width: 38,
                                                height: 38,
                                                child: Image.asset(
                                                    "mario", fit: BoxFit.cover
                                                )
                                            )
                                        )
                                    )
                                }, CColors.PrimaryBlue)
                        ),
                        new Center(
                            child: new Text("personal screen", style: new TextStyle(fontSize: 30)))
                    }
                )
            );
        }
    }
}