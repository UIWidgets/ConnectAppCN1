using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components
{
    public class EventDescription : StatelessWidget
    {
        public EventDescription(
            Key key = null,
            string content = null,
            Dictionary<string, object> contentMap = null
        ) : base(key)
        {
            this.content = content;
            this.contentMap = contentMap;
        }

        public string content;
        public Dictionary<string, object> contentMap;


        public override Widget build(BuildContext context)
        {
            if (content == null)
            {
                return new Container();
            }

            Dictionary<string, object> cont = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(content);
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: map(cont)
            );
        }

        private List<Widget> map(Dictionary<string, dynamic> cont)
        {
            List<Widget> widgets = new List<Widget>();
            /*
            var blocks = cont["blocks"];
            for (int i = 0; i < blocks; i++)
            {
                var block = blocks[i];
                string type = (string)block["type"];
                string text = (string)block["text"];
                switch (type)
                {
                    case "header-one":
                        widgets.Add(_H1(text));
                        break;
                    case "header-two":
                        widgets.Add(_H2(text));
                        break;
                    case "blockquote":
                        widgets.Add(_QuoteBlock(text));
                        break;
                    case "code-block":
                        widgets.Add(_CodeBlock(text));
                        break;
                    case "unstyled":
                        if (text != null) {
                            widgets.Add(_Unstyled(text));
                        }
                        break;
                  
                }

            }
            */
            return widgets;
        }

        private Widget _H1(string text)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 16),
                child: new Text(
                    text,
                    style: new TextStyle(
                        color: Color.white,
                        fontSize: 17.0,
                        letterSpacing: 0.3
                    )
                )
            );
        }

        private Widget _H2(string text)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 16),
                child: new Text(
                    text,
                    style: new TextStyle(
                        color: Color.white,
                        fontSize: 15.0,
                        letterSpacing: 0.3
                    )
                )
            );
        }

        private Widget _Unstyled(string text)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 32),
                child: new Text(
                    text,
                    style: new TextStyle(
                        color: new Color(0xffd8d8d8),
                        fontSize: 14.0,
                        height: 1.4
                    )
                )
            );
        }

        private Widget _CodeBlock(string text)
        {
            return new Container(
                width: new MediaQuery().data.size.width - 32.0,
                decoration: new BoxDecoration(
                    color: new Color(0xff141414)
                ),
                margin: EdgeInsets.only(
                    bottom: 32.0
                ),
                child: new Container(
                    margin: EdgeInsets.all(16.0),
                    child: new Text(
                        text,
                        style: new TextStyle(
                            color: new Color(0xffffffff),
                            height: 1.4
                        )
                    )
                )
            );
        }


        private Widget _QuoteBlock(string text)
        {
            return new Container(
                margin: EdgeInsets.only(bottom: 32.0, left: 16.0),
                decoration: new BoxDecoration(
                    border: new Border(
                        left: new BorderSide(
                            color: new Color(0xffe91e63),
                            width: 4.0
                        )
                    )
                ),
                child: new Container(
                    margin: EdgeInsets.only(
                        left: 8.0,
                        bottom: 8.0
                    ),
                    child: new Text(
                        text,
                        style: new TextStyle(
                            color: new Color(0xffffffff),
                            height: 1.4
                        )
                    )
                )
            );
        }
    }
}