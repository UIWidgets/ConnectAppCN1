using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
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
            GestureTapCallback onTap = null,
            Key key = null
            ) : base(key)
        {
            this.title = title;
            this.iconData = iconData;
            this.done = done;
            this.onTap = onTap;
        }

        public readonly IconData iconData;
        public readonly string title;
        public readonly bool done;
        public readonly GestureTapCallback onTap;

        public override Widget build(BuildContext context)
        {
            var iconColor = done ? CColors.PrimaryBlue: new Color(0xFFC7CBCF);
            var textColor = done ? CColors.PrimaryBlue: CColors.TextBody2;
            var child = new Container(
                decoration:new BoxDecoration(
                    color:CColors.Transparent,
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
            return new GestureDetector(
                onTap: onTap,
                child: child);
        }
    }
}