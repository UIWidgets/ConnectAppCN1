using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components
{
    public class AcitveCard : StatefulWidget
    {
        public AcitveCard(
            
            Key key =null
            
        ) : base(key)
        {
        }

        public override State createState()
        {
            return new _AcitveCardState();
        }
    }

    internal class _AcitveCardState : State<AcitveCard>
    {
        public override Widget build(BuildContext context)
        {
            return new Container(
                height:108,
                padding:EdgeInsets.only(top:16,bottom:16,right:16),
                child:new Row(
                    children:new List<Widget>
                    {
                        //date
                        new Container(
                            width:58,
                            child:new Column(
                                crossAxisAlignment:CrossAxisAlignment.center,
                                children:new List<Widget>
                                {
                                    new Text("27",style:new TextStyle(height: 1.33f,
                                        fontSize: 24,
                                        fontFamily: "DINPro-Bold",
                                        color: CColors.secondaryPink)),
                                    new Text("2月",style:CTextStyle.PSmall)
                                }    
                            )    
                        ),
                    
                        //content
                        new Container(
                            width:MediaQuery.of(context).size.width-188,
                            margin:EdgeInsets.only(right:8),
                            child:new Column(
                                crossAxisAlignment:CrossAxisAlignment.start,
                                mainAxisAlignment:MainAxisAlignment.spaceBetween,
                                children:new List<Widget>
                                {
                                    new Container(
                                        child:new Text("Unity2018.3全新Prefab预制件系统深入介绍",style:CTextStyle.PLarge,maxLines:2)
                                    ),
                                    
                                    new Text("20:00 · 48人已预订",style:CTextStyle.PSmall)
                                }  
                             )
                            ),
                        //pic
                        new Container(
                            child:new ClipRRect(
                                borderRadius:BorderRadius.all(4),
                                child:new Container(
                                    width:114,
                                    child:Image.asset("pikachu",fit:BoxFit.cover)    
                                    
                                )
                            )
                        )
                    }
                )
                
            );
        }
    }

}