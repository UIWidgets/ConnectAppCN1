using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components
{
    public class ActionCard : StatelessWidget
    {
        public ActionCard(
            IconData iconData,
            string title,
            bool done,
            Key key = null
            ) : base(key)
        {
            this.title = title;
            this.iconData = iconData;
        }

        public readonly IconData iconData;
        public readonly string title;
        public readonly bool done;

        public override Widget build(BuildContext context)
        {
            var iconColor = done ? CColors.PrimaryBlue: new Color(0xFFC7CBCF);
            var textColor = done ? CColors.PrimaryBlue: CColors.TextSecondary;
            return new Container(
                decoration:new BoxDecoration(
                    borderRadius:BorderRadius.circular(4),
                    border:Border.all(CColors.Separator)
                ),
                width:100,
                height:40,
                child:new Row(
                    mainAxisAlignment:MainAxisAlignment.center,
                    crossAxisAlignment:CrossAxisAlignment.center,
                    children:new List<Widget>
                    {
                        new Container(
                            margin:EdgeInsets.only(right:10),
                            child:new Icon(iconData,color:iconColor)),
                        new Text(title,style:new TextStyle(
//                            height: 1.5f,
                            fontSize: 16,
                            fontFamily: "PingFang-Regular",
                            color: textColor
                        )
                       )
                    }   
                    
                 )    
            );
        }
    }
}