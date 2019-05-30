using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class PersonalCard : StatelessWidget {
        public PersonalCard(
            PersonalCardItem personalItem,
            Key key = null
        ) : base(key) {
            this.personalItem = personalItem;
        }

        readonly PersonalCardItem personalItem;

        public override Widget build(BuildContext context) {
            if (this.personalItem == null) {
                return new Container();
            }

            return new GestureDetector(
                onTap: () => {
                    if (this.personalItem.onTap != null) {
                        this.personalItem.onTap();
                    }
                },
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
                                            child: new Icon(this.personalItem.icon, size: 24, color: CColors.TextBody2)
                                        ),
                                        new Text(this.personalItem.title ?? "",
                                            style: CTextStyle.PLargeBody
                                        )
                                    }
                                )
                            ),
                            new Icon(Icons.chevron_right, size: 24, color: Color.fromRGBO(199, 203, 207, 1))
                        }
                    )
                )
            );
        }
    }
}