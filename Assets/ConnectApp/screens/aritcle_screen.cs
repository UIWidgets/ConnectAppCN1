using System.Collections.Generic;
using ConnectApp.components;
using ConnectApp.constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.screens
{
    public class ArticleScreen : StatelessWidget
    {
        public override Widget build(BuildContext context)
        {
            return  new Container(
                color:CColors.White,
                child:new Stack(
                    children:new List<Widget>
                    {
                        
                        new Container(
                            padding:EdgeInsets.only(0,140,0,49),
                            child:new ListView(
                                scrollDirection:Axis.vertical,
                                children:new List<Widget>
                                {
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard(),
                                   new ArticleCard()
                                }
                            )
                        ),
                        new Positioned(
                            top:0,
                            left:0,
                            right:0,
                            child:new CustomNavigationBar(new Text("文章",style:CTextStyle.H2White),new List<Widget>
                            {
                                new Container(child:new Icon(Icons.search,null,28,Color.fromRGBO(255,255,255,0.8f))) 
                            },CColors.PrimaryBlue)
                        )
                    
                    }
                )
            );
                
        }
    }
}