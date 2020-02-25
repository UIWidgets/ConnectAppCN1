using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace html {
    public class HtmlOldParser : StatelessWidget {
        public HtmlOldParser(
            float? width = null,
            OnLinkTap onLinkTap = null,
            bool renderNewlines = false,
            CustomRender customRender = null,
            float blockSpacing = 0.0f,
            string html = null,
            ImageErrorListener onImageError = null,
            TextStyle linkStyle = null,
            bool showImages = true
        ) {
            D.assert(width != null);
            this.width = width.Value;
            this.onLinkTap = onLinkTap;
            this.renderNewlines = renderNewlines;
            this.customRender = customRender;
            this.blockSpacing = blockSpacing;
            this.html = html;
            this.onImageError = onImageError;
            this.linkStyle = linkStyle;
            this.showImages = showImages;
            this.linkStyle = linkStyle ?? new TextStyle(
                decoration: TextDecoration.underline,
                color: Colors.blueAccent,
                decorationColor: Colors.blueAccent);
        }

        public readonly float width;
        public readonly OnLinkTap onLinkTap;
        public readonly bool renderNewlines;
        public readonly CustomRender customRender;
        public readonly float blockSpacing;
        public readonly string html;
        public readonly ImageErrorListener onImageError;
        public readonly TextStyle linkStyle;
        public readonly bool showImages;

        static List<string> _supportedElements = new List<string> {
            "a",
            "abbr",
            "acronym",
            "address",
            "article",
            "aside",
            "b",
            "bdi",
            "bdo",
            "big",
            "blockquote",
            "body",
            "br",
            "caption",
            "cite",
            "center",
            "code",
            "data",
            "dd",
            "del",
            "dfn",
            "div",
            "dl",
            "dt",
            "em",
            "figcaption",
            "figure",
            "font",
            "footer",
            "h1",
            "h2",
            "h3",
            "h4",
            "h5",
            "h6",
            "header",
            "hr",
            "i",
            "img",
            "ins",
            "kbd",
            "li",
            "main",
            "mark",
            "nav",
            "noscript",
            "ol", //partial
            "p",
            "pre",
            "q",
            "rp",
            "rt",
            "ruby",
            "s",
            "samp",
            "section",
            "small",
            "span",
            "strike",
            "strong",
            "sub",
            "sup",
            "table",
            "tbody",
            "td",
            "template",
            "tfoot",
            "th",
            "thead",
            "time",
            "tr",
            "tt",
            "u",
            "ul", //partial
            "var",
        };


        public override Widget build(BuildContext context) {
            return new Wrap(
                alignment: WrapAlignment.start,
                children: this.parse(this.html)
            );
        }

        ///Parses an html string and returns a list of widgets that represent the body of your html document.
        public List<Widget> parse(string data) {
            List<Widget> widgetList = new List<Widget>();

            if (this.renderNewlines) {
                data = data.Replace("\n", "<br />");
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(data);
            widgetList.Add(this._parseNode(document.DocumentNode));
            return widgetList;
        }

        Widget _parseNode(HtmlNode node) {
            if (this.customRender != null) {
                Widget customWidget = this.customRender(node, this._parseNodeList(node.ChildNodes));
                if (customWidget != null) {
                    return customWidget;
                }
            }

            if (node.NodeType is HtmlNodeType.Element) {
                if (!_supportedElements.Contains(node.Name)) {
                    return new Container();
                }

                switch (node.Name) {
                    case "a":
                        return new GestureDetector(
                            child: DefaultTextStyle.merge(
                                child: new Wrap(
                                    children: this._parseNodeList(node.ChildNodes)
                                ),
                                style: this.linkStyle
                            ),
                            onTap: () => {
                                if (node.Attributes.Contains("href") && this.onLinkTap != null) {
                                    string url = node.Attributes["href"].Value;
                                    this.onLinkTap(url);
                                }
                            });
                    case "abbr":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.underline,
                                decorationStyle: TextDecorationStyle.solid
                            )
                        );
                    case "acronym":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.underline,
                                decorationStyle: TextDecorationStyle.solid
                            )
                        );
                    case "address":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                    case "article":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "aside":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "b":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "bdi":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "bdo":
                        if (node.Attributes["dir"] != null) {
                            return new Directionality(
                                child: new Wrap(
                                    children: this._parseNodeList(node.ChildNodes)
                                ),
                                textDirection: node.Attributes["dir"].Value == "rtl"
                                    ? TextDirection.rtl
                                    : TextDirection.ltr
                            );
                        }

                        //Direction attribute is required, just render the text normally now.
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "big":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontSize: 20.0f
                            )
                        );
                    case "blockquote":
                        return new Padding(
                            padding:
                            EdgeInsets.fromLTRB(40.0f, this.blockSpacing, 40.0f, this.blockSpacing),
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            )
                        );
                    case "body":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "br":
                        if (this._isNotFirstBreakTag(node)) {
                            return new Container(width: this.width, height: this.blockSpacing);
                        }

                        return new Container(width: this.width);
                    case "caption":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                alignment: WrapAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "center":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes),
                                alignment: WrapAlignment.center
                            ));
                    case "cite":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                    case "code":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontFamily: "monospace"
                            )
                        );
                    case "data":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "dd":
                        return new Padding(
                            padding: EdgeInsets.only(left: 40.0f),
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ));
                    case "del":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.lineThrough
                            )
                        );
                    case "dfn":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                    case "div":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "dl":
                        return new Padding(
                            padding: EdgeInsets.only(top: this.blockSpacing, bottom: this.blockSpacing),
                            child: new Column(
                                children: this._parseNodeList(node.ChildNodes),
                                crossAxisAlignment: CrossAxisAlignment.start
                            ));
                    case "dt":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "em":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                    case "figcaption":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "figure":
                        return new Padding(
                            padding:
                            EdgeInsets.fromLTRB(40.0f, this.blockSpacing, 40.0f, this.blockSpacing),
                            child: new Column(
                                children: this._parseNodeList(node.ChildNodes),
                                crossAxisAlignment: CrossAxisAlignment.center
                            ));
                    case "font":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "footer":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "h1":
                        return DefaultTextStyle.merge(
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontSize: 28.0f,
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "h2":
                        return DefaultTextStyle.merge(
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontSize: 21.0f,
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "h3":
                        return DefaultTextStyle.merge(
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontSize: 16.0f,
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "h4":
                        return DefaultTextStyle.merge(
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontSize: 14.0f,
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "h5":
                        return DefaultTextStyle.merge(
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontSize: 12.0f,
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "h6":
                        return DefaultTextStyle.merge(
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontSize: 10.0f,
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "header":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "hr":
                        return new Padding(
                            padding: EdgeInsets.only(top: 7.0f, bottom: 7.0f),
                            child: new Divider(height: 1.0f, color: Colors.black38)
                        );
                    case "i":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                    case "img":
                        return new Builder(
                            builder: (BuildContext context) => {
                                if (this.showImages) {
                                    if (node.Attributes["src"] != null) {
                                        if (node.Attributes["src"].Value.StartsWith("data:image") &&
                                            node.Attributes["src"].Value.Contains("base64,")) {
                                            return Image.memory(Convert.FromBase64String(
                                                node.Attributes["src"].Value.Split(new string[] {"base64,"},
                                                    StringSplitOptions.None)[1].Trim()));
                                        }

                                        return Image.network(node.Attributes["src"].Value);
                                    }
                                    else if (node.Attributes["alt"] != null) {
                                        //Temp fix for https://github.com/flutter/flutter/issues/736
                                        if (node.Attributes["alt"].Value.EndsWith(" ")) {
                                            return new Container(
                                                padding: EdgeInsets.only(right: 2.0f),
                                                child: new Text(node.Attributes["alt"].Value));
                                        }
                                        else {
                                            return new Text(node.Attributes["alt"].Value);
                                        }
                                    }
                                }

                                return new Container();
                            }
                        );
                    case "ins":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.underline
                            )
                        );
                    case "kbd":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontFamily: "monospace"
                            )
                        );
                    case "li":
                        string type = node.ParentNode.Name; // Parent type; usually ol or ul
                        EdgeInsets markPadding = EdgeInsets.symmetric(horizontal: 4.0f);
                        Widget mark;
                        switch (type) {
                            case "ul":
                                mark = new Container(child: new Text("•"), padding: markPadding);
                                break;
                            case "ol":
                                int index = node.ParentNode.ChildNodes.IndexOf(node) + 1;
                                mark = new Container(child: new Text($"{index}."), padding: markPadding);
                                break;
                            default: //Fallback to middle dot
                                mark = new Container(width: 0.0f, height: 0.0f);
                                break;
                        }

                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: new List<Widget> {
                                    mark,
                                    new Wrap(
                                        crossAxisAlignment: WrapCrossAlignment.center,
                                        children: this._parseNodeList(node.ChildNodes))
                                }
                            )
                        );
                    case "main":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "mark":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                color: Colors.black,
                                background: this._getPaint(Colors.yellow)
                            )
                        );
                    case "nav":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "noscript":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                alignment: WrapAlignment.start,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "ol":
                        return new Column(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.start
                        );
                    case "p":
                        return new Padding(
                            padding: EdgeInsets.only(top: this.blockSpacing, bottom: this.blockSpacing),
                            child: new Container(
                                width: this.width,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    alignment: WrapAlignment.start,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            )
                        );
                    case "pre":
                        return new Padding(
                            padding: EdgeInsets.all(this.blockSpacing),
                            child: DefaultTextStyle.merge(
                                child: new Text(node.InnerHtml),
                                style: new TextStyle(
                                    fontFamily: "monospace"
                                )
                            )
                        );
                    case "q":
                        List<Widget> children = new List<Widget>();
                        children.Add(new Text("\""));
                        children.AddRange(this._parseNodeList(node.ChildNodes));
                        children.Add(new Text("\""));
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: children
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                    case "rp":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "rt":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "ruby":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "s":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.lineThrough
                            )
                        );
                    case "samp":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontFamily: "monospace"
                            )
                        );
                    case "section":
                        return new Container(
                            width: this.width,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "small":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontSize: 10.0f
                            )
                        );
                    case "span":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "strike":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.lineThrough
                            )
                        );
                    case "strong":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "sub":
                    case "sup":
                        //Use builder to capture the parent font to inherit the font styles
                        return new Builder(builder: (BuildContext context) => {
                            DefaultTextStyle parent = DefaultTextStyle.of(context);
                            TextStyle parentStyle = parent.style;

                            var painter = new TextPainter(
                                text: new TextSpan(
                                    text: node.InnerText,
                                    style: parentStyle
                                ),
                                textDirection: TextDirection.ltr);
                            painter.layout();
                            //print(painter.size);

                            //Get the height from the default text
                            var height = painter.size.height *
                                         1.35f; //compute a higher height for the text to increase the offset of the Positioned text

                            painter = new TextPainter(
                                text: new TextSpan(
                                    text: node.InnerText,
                                    style: parentStyle.merge(new TextStyle(
                                        fontSize:
                                        parentStyle.fontSize * RichTextParserUtils.OFFSET_TAGS_FONT_SIZE_FACTOR))
                                ),
                                textDirection: TextDirection.ltr);
                            painter.layout();
                            //print(painter.size);

                            //Get the width from the reduced/positioned text
                            var width = painter.size.width;

                            //print("Width: $width, Height: $height");

                            return DefaultTextStyle.merge(
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    children: new List<Widget> {
                                        new Stack(
                                            fit: StackFit.loose,
                                            children: new List<Widget> {
                                                //The Stack needs a non-positioned object for the next widget to respect the space so we create
                                                //a sized box to fill the required space
                                                new SizedBox(
                                                    width: width,
                                                    height: height
                                                ),
                                                DefaultTextStyle.merge(
                                                    child: new Positioned(
                                                        child: new Wrap(children: this._parseNodeList(node.ChildNodes)),
                                                        bottom: node.Name == "sub" ? 0 : (int?) null,
                                                        top: node.Name == "sub" ? (int?) null : 0
                                                    ),
                                                    style: new TextStyle(
                                                        fontSize: parentStyle.fontSize *
                                                                  RichTextParserUtils.OFFSET_TAGS_FONT_SIZE_FACTOR)
                                                )
                                            }
                                        )
                                    }
                                )
                            );
                        });
                    case "table":
                        return new Column(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.start
                        );
                    case "tbody":
                        return new Column(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.start
                        );
                    case "td":
                        int colspan = 1;
                        if (node.Attributes["colspan"] != null) {
                            int.TryParse(node.Attributes["colspan"].Value, out colspan);
                        }

                        return new Expanded(
                            flex: colspan,
                            child: new Wrap(
                                crossAxisAlignment: WrapCrossAlignment.center,
                                children: this._parseNodeList(node.ChildNodes)
                            )
                        );
                    case "template":
                        //Not usually displayed in HTML
                        return new Container();
                    case "tfoot":
                        return new Column(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.start
                        );
                    case "th":
                        int _colspan = 1;
                        if (node.Attributes["colspan"] != null) {
                            int.TryParse(node.Attributes["colspan"].Value, out _colspan);
                        }

                        return DefaultTextStyle.merge(
                            child: new Expanded(
                                flex: _colspan,
                                child: new Wrap(
                                    crossAxisAlignment: WrapCrossAlignment.center,
                                    alignment: WrapAlignment.center,
                                    children: this._parseNodeList(node.ChildNodes)
                                )
                            ),
                            style: new TextStyle(
                                fontWeight: FontWeight.bold
                            )
                        );
                    case "thead":
                        return new Column(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.start
                        );
                    case "time":
                        return new Wrap(
                            children: this._parseNodeList(node.ChildNodes)
                        );
                    case "tr":
                        return new Row(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.center
                        );
                    case "tt":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontFamily: "monospace"
                            )
                        );
                    case "u":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                decoration: TextDecoration.underline
                            )
                        );
                    case "ul":
                        return new Column(
                            children: this._parseNodeList(node.ChildNodes),
                            crossAxisAlignment: CrossAxisAlignment.start
                        );
                    case "var":
                        return DefaultTextStyle.merge(
                            child: new Wrap(
                                children: this._parseNodeList(node.ChildNodes)
                            ),
                            style: new TextStyle(
                                fontStyle: FontStyle.italic
                            )
                        );
                }
            }
            else if (node.NodeType is HtmlNodeType.Text) {
                //We don't need to worry about rendering extra whitespace
                if (node.InnerText.Trim() == "" && node.InnerText.IndexOf(" ") == -1) {
                    return new Wrap();
                }

                if (node.InnerText.Trim() == "" && node.InnerText.IndexOf(" ") != -1) {
                    (node as HtmlTextNode).Text = " ";
                }

                string finalText = this.trimStringHtml(node.InnerText);
                //Temp fix for https://github.com/flutter/flutter/issues/736
                if (finalText.EndsWith(" ")) {
                    return new Container(
                        padding: EdgeInsets.only(right: 2.0f), child: new Text(finalText));
                }
                else {
                    return new Text(finalText);
                }
            }

            return new Wrap();
        }

        List<Widget> _parseNodeList(HtmlNodeCollection nodeList) {
            return nodeList.Select((node) => { return this._parseNode(node); }).ToList();
        }

        Paint _getPaint(Color color) {
            Paint paint = new Paint();
            paint.color = color;
            return paint;
        }

        public string trimStringHtml(string stringToTrim) {
            stringToTrim = stringToTrim.Replace("\n", "");
            while (stringToTrim.IndexOf("  ") != -1) {
                stringToTrim = stringToTrim.Replace("  ", " ");
            }

            return stringToTrim;
        }

        bool _isNotFirstBreakTag(HtmlNode node) {
            int index = node.ParentNode.ChildNodes.IndexOf(node);
            if (index == 0) {
                if (node.ParentNode == null) {
                    return false;
                }

                return this._isNotFirstBreakTag(node.ParentNode);
            }
            else if (node.ParentNode.ChildNodes[index - 1] is HtmlNode) {
                if ((node.ParentNode.ChildNodes[index - 1]).Name == "br") {
                    return true;
                }

                return false;
            }
            else if (node.ParentNode.ChildNodes[index - 1].NodeType == HtmlNodeType.Text) {
                if ((node.ParentNode.ChildNodes[index - 1]).InnerText.Trim() == "") {
                    return this._isNotFirstBreakTag(node.ParentNode.ChildNodes[index - 1]);
                }
                else {
                    return false;
                }
            }

            return false;
        }
    }
}