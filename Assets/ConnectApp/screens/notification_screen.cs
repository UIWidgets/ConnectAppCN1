using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens
{
    public class NotificationScreen : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return  new Container(
                color:CColors.White,
                child:new Stack(
                    children:new List<Widget>
                    {
                        new Positioned(
                            top:0,
                            left:0,
                            right:0,
                            child:new CustomNavigationBar(new Text("通知",style:CTextStyle.H2White),new List<Widget>
                            {},CColors.PrimaryBlue)
                        ),
                        new Center(
                            child:new Text("notification screen",style:new TextStyle(fontSize:30)))
                    
                    }
                )
            );
        }
    }
}