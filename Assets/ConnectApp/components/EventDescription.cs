using System.Collections.Generic;
using System.Linq;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class EventDescription : StatelessWidget {
        public EventDescription(
            Key key = null,
            string content = null,
            Dictionary<string, ContentMap> contentMap = null
        ) : base(key) {
            this.content = content;
            this.contentMap = contentMap;
        }

        public readonly string content;
        public readonly Dictionary<string, ContentMap> contentMap;


        public override Widget build(BuildContext context) {
            if (content == null) return new Container();

            var cont = JsonConvert.DeserializeObject<EventContent>(content);
            return new Column(
                crossAxisAlignment:CrossAxisAlignment.start,
                children: map(cont)
            );
        }

        private List<Widget> map(EventContent content) {
            var widgets = new List<Widget>();
            
            var blocks = content.blocks;
            for (var i = 0; i < blocks.Count; i++) {
                var block = blocks[i];
                var type = block.type;
                var text = block.text;
                switch (type) {
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
                        if (text != null) widgets.Add(_Unstyled(text));
                        break;
                    case "unordered-list-item": {
                        string[] items = {block.text};
                        while (i + 1 < blocks.Count &&
                               blocks[i + 1].type == "unordered-list-item") {
                            items.Append(blocks[i + 1].text);
                            i++;
                        }

                        widgets.Add(_UnorderedList(items));
                    }
                        break;
                    case "ordered-list-item": {
                        string[] items = {block.text};
                        while (i + 1 < blocks.Count &&
                               blocks[i + 1].type == "ordered-list-item") {
                            items.Append(blocks[i + 1].text);
                            i++;
                        }

                        widgets.Add(_OrderedList(items));
                    }
                        break;
                    case "atomic": {
                        var range = block.entityRanges.first();
                        var rangeKey = range.key.ToString();
                        var data = content.entityMap[rangeKey].data;
                        var map = contentMap[data.contentId];
                        widgets.Add(_Atomic(block.type, data.title, map.originalImage.url));
                    }
                        break;
                }
            }

            //*/
            return widgets;
        }

        private Widget _H1(string text) {
            return new Container(
                padding:EdgeInsets.only(top:16),
                margin: EdgeInsets.only(bottom: 24),
                child: new Text(
                    text,
                    style: new TextStyle(
                        color: CColors.TextTitle,
                        fontSize: 24,
                        letterSpacing: 0.0f,
                        height: 1.33f
                    )
                )
            );
        }

        private Widget _H2(string text) {
            return new Container(
                padding:EdgeInsets.only(top:16),
                margin: EdgeInsets.only(bottom: 24),
                child: new Text(
                    text,
                    style: new TextStyle(
                        color: CColors.TextTitle,
                        fontSize: 20,
                        height: 1.4f
                    )
                )
            );
        }

        private Widget _Unstyled(string text) {
            return new Container(
                margin: EdgeInsets.only(bottom: 24),
                child: new Text(
                    text,
                    style: CTextStyle.TextBody1
                )
            );
        }

        private Widget _CodeBlock(string text) {
            return new Container(
                width: new MediaQuery().data.size.width,
                decoration: new BoxDecoration(
                    color:Color.fromRGBO(110,198,255,0.12f)
                ),
                margin: EdgeInsets.only(
                    bottom: 24
                ),
                padding:EdgeInsets.all(16),
                child: new Container(
                    child: new Text(
                        text,
                        style: CTextStyle.PRegular
                    )
                )
            );
        }


        private Widget _QuoteBlock(string text) {
            return new Container(
                margin: EdgeInsets.only(bottom: 24, left: 8),
                decoration: new BoxDecoration(
                    border: new Border(
                        left: new BorderSide(
                            Color.fromRGBO(60,131,212,0.3f),
                            16
                        )
                    )
                ),
                child: new Container(
                    child: new Text(
                        text,
                        style: CTextStyle.TextBody4
                    )
                )
            );
        }

        private Widget _Atomic(string type, string title, string url) {
            var nodes = new List<Widget>() {
                Image.network(url)
            };
            if (title != null) {
                var imageTitle = new Container(
                    decoration: new BoxDecoration(
                        border: new Border(
                            bottom: new BorderSide(
                                width: 1,
                                color: CColors.TextBody
                            )
                        )
                    ),
                    child: new Container(
                        margin: EdgeInsets.only(4, 8, 4, 4),
                        child: new Text(
                            title,
                            style: new TextStyle(
                                color: CColors.TextBody,
                                fontSize: 12
                            )
                        )
                    )
                );
                nodes.Add(imageTitle);
            }

            return new Container(
                margin: EdgeInsets.only(bottom: 32),
                child: new Column(children: nodes)
            );
        }


        private Widget _OrderedList(string[] items) {
            var widgets = new List<Widget>();

            for (var i = 0; i < items.Length; i++) {
                var spans = new List<TextSpan>() {
                    new TextSpan(
                        $"i+1",
                        CTextStyle.TextBody1
                    ),
                    new TextSpan(
                        items[i],
                        CTextStyle.TextBody1
                    ),
                };
                widgets.Add(
                    new Container(margin: EdgeInsets.only(top: i == 0 ? 0 : 8),
                        child: new RichText(
                            text: new TextSpan(
                                style: CTextStyle.TextBody1,
                                children: spans
                            )
                        )
                    )
                );
            }


            return new Container(margin: EdgeInsets.only(bottom: 24, left: 16),
                child: new Column(crossAxisAlignment: CrossAxisAlignment.start, children: widgets));
        }

        private Widget _UnorderedList(string[] items) {
            var widgets = new List<Widget>();

            for (var i = 0; i < items.Length; i++) {
                var spans = new List<TextSpan>() {
                    new TextSpan(
                        "\\u{25cf}",
                        CTextStyle.TextBody1
                    ),
                    new TextSpan(
                        items[i],
                        CTextStyle.TextBody1
                    ),
                };
                widgets.Add(
                    new Container(margin: EdgeInsets.only(top: i == 0 ? 0 : 8),
                        child: new RichText(
                            text: new TextSpan(
                                style: CTextStyle.TextBody1,
                                children: spans
                            )
                        )
                    )
                );
            }


            return new Container(margin: EdgeInsets.only(bottom: 24, left: 16),
                child: new Column(crossAxisAlignment: CrossAxisAlignment.start, children: widgets));
        }
    }
}