using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class PersonalCard : StatelessWidget {
        public PersonalCard(
            PersonalCardItem personalItem,
            Key key = null
        ) : base(key) {
            this.personalItem = personalItem;
        }

        private readonly PersonalCardItem personalItem;

        public override Widget build(BuildContext context) {
            return new GestureDetector(
                onTap: personalItem.onTap,
                child: new Container(
                    padding: EdgeInsets.only(16, right: 16),
                    height: 60,
                    color: CColors.White,
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        crossAxisAlignment: CrossAxisAlignment.center,
                        children: new List<Widget> {
                            new Container(
                                height: 24,
                                child: new Row(
                                    crossAxisAlignment: CrossAxisAlignment.center,
                                    mainAxisAlignment: MainAxisAlignment.start,
                                    children: new List<Widget> {
                                        new Container(
                                            margin: EdgeInsets.only(right: 12),
                                            height: 24,
                                            width: 24,
                                            child: new Icon(personalItem.icon, null, 20, CColors.TextSecondary)
                                        ),
//                                        new Text(widget.personalItem.title,style:CTextStyle.PLarge),
                                        new Text(personalItem.title,
                                            style: new TextStyle(fontSize: 16, height: 1.5f, fontWeight: FontWeight.w400,
                                                color: CColors.TextBody))
                                    }
                                )
                            ),
                            new Icon(Icons.chevron_right, null, 24, Color.fromRGBO(199, 203, 207, 1))
                        }
                    )
                )
            );
        }
    }
}