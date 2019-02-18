using System.Collections.Generic;
using System.Linq;
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

        public string content;
        public Dictionary<string, ContentMap> contentMap;


        public override Widget build(BuildContext context) {
            if (content == null) return new Container();

            var cont = JsonConvert.DeserializeObject<EventContent>(content);
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: map(cont)
            );
        }

        private List<Widget> map(EventContent content) {
            var widgets = new List<Widget>();
            ///*
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

        private Widget _H2(string text) {
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

        private Widget _Unstyled(string text) {
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

        private Widget _CodeBlock(string text) {
            return new Container(
                width: new MediaQuery().data.size.width - 32.0,
                decoration: new BoxDecoration(
                    new Color(0xff141414)
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


        private Widget _QuoteBlock(string text) {
            return new Container(
                margin: EdgeInsets.only(bottom: 32.0, left: 16.0),
                decoration: new BoxDecoration(
                    border: new Border(
                        left: new BorderSide(
                            new Color(0xffe91e63),
                            4.0
                        )
                    )
                ),
                child: new Container(
                    margin: EdgeInsets.only(
                        8.0,
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

        private Widget _Atomic(string type, string title, string url) {
            var nodes = new List<Widget>() {
                Image.network(url)
            };
            if (title != null) {
                var imageTitle = new Container(
                    decoration: new BoxDecoration(
                        border: new Border(
                            bottom: new BorderSide(
                                width: 1.0,
                                color: new Color(0xffd8d8d8)
                            )
                        )
                    ),
                    child: new Container(
                        margin: EdgeInsets.only(4.0, 8.0, 4.0, 4.0),
                        child: new Text(
                            title,
                            style: new TextStyle(
                                color: new Color(0xffd8d8d8),
                                fontSize: 12.0
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
                        new TextStyle(
                            color: new Color(0xffffffff)
                        )
                    ),
                    new TextSpan(
                        items[i],
                        new TextStyle(
                            color: new Color(0xffd8d8d8)
                        )
                    ),
                };
                widgets.Add(
                    new Container(margin: EdgeInsets.only(top: i == 0 ? 0.0 : 8.0),
                        child: new RichText(
                            text: new TextSpan(
                                style: new TextStyle(
                                    height: 1.4
                                ),
                                children: spans
                            )
                        )
                    )
                );
            }


            return new Container(margin: EdgeInsets.only(bottom: 32, left: 16),
                child: new Column(crossAxisAlignment: CrossAxisAlignment.start, children: widgets));
        }

        private Widget _UnorderedList(string[] items) {
            var widgets = new List<Widget>();

            for (var i = 0; i < items.Length; i++) {
                var spans = new List<TextSpan>() {
                    new TextSpan(
                        "\\u{25cf}",
                        new TextStyle(
                            color: new Color(0xffffffff)
                        )
                    ),
                    new TextSpan(
                        items[i],
                        new TextStyle(
                            color: new Color(0xffd8d8d8)
                        )
                    ),
                };
                widgets.Add(
                    new Container(margin: EdgeInsets.only(top: i == 0 ? 0.0 : 8.0),
                        child: new RichText(
                            text: new TextSpan(
                                style: new TextStyle(
                                    height: 1.4
                                ),
                                children: spans
                            )
                        )
                    )
                );
            }


            return new Container(margin: EdgeInsets.only(bottom: 32, left: 16),
                child: new Column(crossAxisAlignment: CrossAxisAlignment.start, children: widgets));
        }
    }
}