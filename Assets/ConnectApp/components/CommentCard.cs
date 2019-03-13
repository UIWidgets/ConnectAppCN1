using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components
{
    public class CommentCard : StatelessWidget
    {
        public CommentCard(
            Key key = null
        ) : base(key)
        {
            
        }

        public override Widget build(BuildContext context)
        {
            return new Container(
                padding:EdgeInsets.only(bottom:16),
                child:new Row(
                    crossAxisAlignment:CrossAxisAlignment.start,
                    children:new List<Widget>
                    {
                        new Container(
                            margin:EdgeInsets.only(right:8,top:2),
                            child:new ClipRRect(
                                borderRadius:BorderRadius.circular(12),
                                child:new Container(
                                    color:CColors.Red,
                                    width:24,
                                    height:24,
                                    child:Image.network("",fit:BoxFit.fill)
                                )
                            )
                        ),
                        
                        new Expanded(
                            child:new Container(
                                child:new Column(
                                children:new List<Widget>
                                {
                                    new Row(
                                        children:new List<Widget>
                                        {
                                            new Expanded(
                                                child:new Text("码农小哥",style:new TextStyle(
                                                    height: 1.57f,
                                                    fontSize: 14,
                                                    fontFamily: "PingFangSC-Regular",
                                                    color: CColors.TextThird))),
                                            new GestureDetector(
                                                child:new Icon(Icons.ellipsis,size:20,color:CColors.BrownGrey)
                                            )
                                        }
                                    ),
                                    new Text("现在仅需启用自定义顶点流，添加Center流，和之前一样，请无视警告信息，一旦我们使用新的着色器分配新材质，警告会自动消失。",style:CTextStyle.PLarge),
                                    new Row(
                                        mainAxisAlignment:MainAxisAlignment.spaceBetween,
                                        children:new List<Widget>
                                        {
                                            new Text("2天前",style:CTextStyle.TextBody4),
                                            new Container(
                                               child: new Row(
                                                    children:new List<Widget>
                                                    {
                                                        new GestureDetector(
                                                            child:new Text("点赞 10",style:CTextStyle.TextBody4)
                                                        ),
                                                        new Container(width:10),
                                                        new GestureDetector(
                                                            child:new Text("回复 24",style:CTextStyle.TextBody4)
                                                        ),
                                                    }
                                                )
                                            )
                                        }
                                    ),
                                    new Container(height:1,color:CColors.Separator2)
                                    
                                }
                            )
                         )
                            
                        )
                    }
                )
            );
        }
    }
}