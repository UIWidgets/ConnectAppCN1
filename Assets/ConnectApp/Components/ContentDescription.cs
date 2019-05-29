using System;
using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public static class ContentDescription {
        public static List<Widget> map(BuildContext context, string cont, Dictionary<string, ContentMap> contentMap,
            Action<string> openUrl, Action<string> playVideo) {
            if (cont == null) {
                return new List<Widget>();
            }

            var content = JsonConvert.DeserializeObject<EventContent>(cont);
            var widgets = new List<Widget>();

            var orderedIndex = 1;
            var blocks = content.blocks;
            for (var i = 0; i < blocks.Count; i++) {
                var block = blocks[i];
                var type = block.type;
                var text = block.text;
                switch (type) {
                    case "header-one": {
                        var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                            block.inlineStyleRanges, openUrl, CTextStyle.H4);
                        widgets.Add(_H1(text, inlineSpans));
                    }
                        break;
                    case "header-two": {
                        var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                            block.inlineStyleRanges, openUrl, CTextStyle.H5);
                        widgets.Add(_H2(text, inlineSpans));
                    }
                        break;
                    case "blockquote": {
                        var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                            block.inlineStyleRanges, openUrl, CTextStyle.PXLargeBody4);
                        widgets.Add(_QuoteBlock(text, inlineSpans));
                    }
                        break;
                    case "code-block": {
                        var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                            block.inlineStyleRanges, openUrl, CTextStyle.PCodeStyle);
                        widgets.Add(_CodeBlock(context, text, inlineSpans));
                    }
                        break;
                    case "unstyled": {
                        if (text.isNotEmpty()) {
                            var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                                block.inlineStyleRanges, openUrl);
                            widgets.Add(_Unstyled(text, inlineSpans));
                        }
                    }
                        break;
                    case "unordered-list-item": {
                        var isFirst = true;
                        if (i > 0) {
                            var beforeBlock = blocks[i - 1];
                            isFirst = beforeBlock.type != "unordered-list-item";
                        }

                        var isLast = true;
                        if (i < blocks.Count - 1) {
                            var afterBlock = blocks[i + 1];
                             isLast= afterBlock.type != "unordered-list-item";
                        }
                        var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                            block.inlineStyleRanges, openUrl);
                        widgets.Add(
                            _UnorderedList(
                                block.text,
                                isFirst,
                                isLast,
                                inlineSpans
                            )
                        );
                    }
                        break;
                    case "ordered-list-item": {
                        var isFirst = true;
                        if (i > 0) {
                            var beforeBlock = blocks[i - 1];
                            isFirst = beforeBlock.type != "ordered-list-item";
                        }
                        if (isFirst) {
                            orderedIndex = 1;
                        }
                        else {
                            orderedIndex++;
                        }
                        var afterBlock = blocks[i + 1];
                        var isLast = afterBlock.type != "ordered-list-item";

                        var inlineSpans = _RichStyle(text, content.entityMap, block.entityRanges,
                            block.inlineStyleRanges, openUrl);
                        widgets.Add(_OrderedList(text, orderedIndex, isLast, inlineSpans));
                    }
                        break;
                    case "atomic": {
                        var key = block.entityRanges.first().key.ToString();
                        if (content.entityMap.ContainsKey(key)) {
                            var dataMap = content.entityMap[key];
                            var data = dataMap.data;
                            if (data.contentId.isNotEmpty()) {
                                if (contentMap.ContainsKey(data.contentId)) {
                                    var map = contentMap[data.contentId];
                                    var url = map.url;
                                    var downloadUrl = map.downloadUrl ?? "";
                                    var contentType = map.contentType ?? "";
                                    var originalImage = map.originalImage == null
                                        ? map.thumbnail
                                        : map.originalImage;
                                    widgets.Add(_Atomic(context, dataMap.type, contentType, data.title, originalImage,
                                        url, downloadUrl,
                                        openUrl, playVideo));
                                }
                            }
                        }
                    }
                        break;
                }
            }

            return widgets;
        }

        static Widget _H1(string text, List<TextSpan> inlineSpans) {
            if (text == null) {
                return new Container();
            }

            Widget child = new Text(
                text,
                style: CTextStyle.H4
            );
            if (inlineSpans != null) {
                child = new RichText(
                    text: new TextSpan(
                        children: inlineSpans
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16, 24),
                child: child
            );
        }

        static Widget _H2(string text, List<TextSpan> inlineSpans) {
            if (text == null) {
                return new Container();
            }

            Widget child = new Text(
                text,
                style: CTextStyle.H5
            );
            if (inlineSpans != null) {
                child = new RichText(
                    text: new TextSpan(
                        children: inlineSpans
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16, 24),
                child: child
            );
        }

        static Widget _Unstyled(string text, List<TextSpan> inlineSpans) {
            if (text == null) {
                return new Container();
            }

            Widget child = new Text(
                text,
                style: CTextStyle.PXLarge
            );
            if (inlineSpans != null) {
                child = new RichText(
                    text: new TextSpan(
                        children: inlineSpans
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, bottom: 24),
                child: child
            );
        }

        static Widget _CodeBlock(BuildContext context, string text, List<TextSpan> inlineSpans) {
            if (text == null) {
                return new Container();
            }

            Widget child = new Text(
                text,
                style: CTextStyle.PCodeStyle
            );
            if (inlineSpans != null) {
                child = new RichText(
                    text: new TextSpan(
                        children: inlineSpans
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 24),
                child: new Container(
                    color: Color.fromRGBO(110, 198, 255, 0.12f),
                    width: MediaQuery.of(context).size.width,
                    child: new Container(
                        padding: EdgeInsets.all(16),
                        child: child
                    )
                )
            );
        }


        static Widget _QuoteBlock(string text, List<TextSpan> inlineSpans) {
            Widget child = new Text(
                text,
                style: CTextStyle.PXLargeBody4
            );
            if (inlineSpans != null) {
                child = new RichText(
                    text: new TextSpan(
                        children: inlineSpans
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, bottom: 24),
                child: new Container(
                    decoration: new BoxDecoration(
                        CColors.White,
                        border: new Border(
                            left: new BorderSide(
                                CColors.Separator,
                                8
                            )
                        )
                    ),
                    padding: EdgeInsets.only(16),
                    child: child
                )
            );
        }

        static Widget _Atomic(BuildContext context, string type, string contentType, string title,
            _OriginalImage originalImage,
            string url, string downloadUrl, Action<string> openUrl, Action<string> playVideo) {
            if (type == "ATTACHMENT" && contentType != "video/mp4") {
                return new Container();
            }

            var playButton = Positioned.fill(
                new Container()
            );
            if (type == "VIDEO" || type == "ATTACHMENT") {
                playButton = Positioned.fill(
                    new Center(
                        child: new CustomButton(
                            onPressed: () => {
                                if (type == "ATTACHMENT") {
                                    playVideo($"{downloadUrl}?noLoginRequired=true");
                                }
                                else {
                                    if (url == null || url.Length <= 0) {
                                        return;
                                    }

                                    openUrl(url);
                                }
                            },
                            child: new Container(
                                width: 60,
                                height: 60,
                                decoration: new BoxDecoration(
                                    CColors.H5White,
                                    borderRadius: BorderRadius.all(30)
                                ),
                                child: new Icon(
                                    Icons.play_arrow,
                                    size: 45,
                                    color: CColors.icon3
                                )
                            )
                        )
                    )
                );
            }

            var attachWidth = MediaQuery.of(context).size.width - 32;
            var attachHeight = attachWidth * 9 / 16;
            if (type == "ATTACHMENT") {
                return new Container(
                    color: CColors.White,
                    padding: EdgeInsets.only(bottom: 32),
                    alignment: Alignment.center,
                    child: new Container(
                        padding: EdgeInsets.only(16, right: 16),
                        child: new Column(
                            children: new List<Widget> {
                                new Stack(
                                    children: new List<Widget> {
                                        new Container(
                                            width: attachWidth,
                                            height: attachHeight,
                                            color: CColors.Black
                                        ),
                                        playButton
                                    }
                                )
                            }
                        )
                    )
                );
            }

            var width = originalImage.width < MediaQuery.of(context).size.width - 32
                ? originalImage.width
                : MediaQuery.of(context).size.width - 32;
            var height = width * originalImage.height / originalImage.width;
            var imageUrl = originalImage.url;
            if (imageUrl.isNotEmpty()) {
                imageUrl = imageUrl.EndsWith(".gif") ? imageUrl : $"{originalImage.url}.600x0x1.jpg";
            }

            var nodes = new List<Widget> {
                new Stack(
                    children: new List<Widget> {
                        new PlaceholderImage(
                            imageUrl,
                            width,
                            height,
                            fit: BoxFit.cover
                        ),
                        playButton
                    }
                )
            };
            if (title != null) {
                var imageTitle = new Container(
                    decoration: new BoxDecoration(
                        border: new Border(
                            bottom: new BorderSide(
                                CColors.Separator,
                                2
                            )
                        )
                    ),
                    child: new Container(
                        margin: EdgeInsets.only(4, 8, 4, 4),
                        child: new Text(
                            title,
                            style: CTextStyle.PRegularBody4
                        )
                    )
                );
                nodes.Add(imageTitle);
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 32),
                alignment: Alignment.center,
                child: new Container(
                    padding: EdgeInsets.only(16, right: 16),
                    child: new Column(
                        children: nodes
                    )
                )
            );
        }


        static Widget _OrderedList(
            string text,
            int index,
            bool isLast,
            List<TextSpan> inlineSpans
        ) {
            var spans = new List<TextSpan> {
                new TextSpan(
                    $"{index}. ",
                    CTextStyle.PXLarge
                )
            };
            if (inlineSpans != null) {
                spans.AddRange(inlineSpans);
            }
            else {
                spans.Add(new TextSpan(text, CTextStyle.PXLarge));
            }

            return new Container(
                padding: EdgeInsets.only(16, right: 16),
                margin: EdgeInsets.only(top: index == 1 ? 0 : 4, bottom: isLast ? 24 : 0),
                child: new RichText(
                    text: new TextSpan(
                        style: CTextStyle.PXLarge,
                        children: spans
                    )
                )
            );
        }

        static Widget _UnorderedList(
            string text,
            bool isFirst,
            bool isLast,
            List<TextSpan> inlineSpans
        ) {
            Widget child = new Text(
                text,
                style: CTextStyle.PXLarge
            );
            if (inlineSpans != null) {
                child = new RichText(
                    text: new TextSpan(
                        children: inlineSpans
                    )
                );
            }

            var spans = new List<Widget> {
                new Container(
                    width: 8,
                    height: 8,
                    margin: EdgeInsets.only(top: 14, right: 8),
                    decoration: new BoxDecoration(
                        CColors.Black,
                        borderRadius: BorderRadius.all(4)
                    )
                ),
                new Expanded(
                    child: child
                )
            };

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, isFirst ? 0 : 4, 16, isLast ? 24 : 0),
                child: new Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: spans
                )
            );
        }

        static List<TextSpan> _RichStyle(
            string text,
            Dictionary<string, _EventContentEntity> entityMap,
            List<_EntityRange> entityRanges,
            List<_InlineStyleRange> inlineStyleRanges,
            Action<string> openUrl,
            TextStyle style = null
        ) {
            if (text == null) {
                return null;
            }

            if (entityRanges == null
                && entityRanges.Count <= 0
                && inlineStyleRanges == null
                && inlineStyleRanges.Count <= 0) {
                return null;
            }

            var newStyle = style != null ? style : CTextStyle.PXLarge;
            if (inlineStyleRanges.Count <= 0) {
                return _LinkSpans(text, entityMap, entityRanges, openUrl, newStyle);
            }

            if (entityRanges.Count <= 0) {
                return _InlineStyle(text, inlineStyleRanges, newStyle);
            }

            var inlineStyleRange = inlineStyleRanges.first();
            var inlineOffset = inlineStyleRange.offset;
            var inlineLength = inlineStyleRange.length;
            var inlineStyle = inlineStyleRange.style;

            var entityRange = entityRanges.first();
            var key = entityRange.key.ToString();
            var data = entityMap[key];
            var entityOffset = entityRange.offset;
            var entityLength = entityRange.length;
            var textStyle = newStyle.merge(
                new TextStyle(
                    fontWeight: inlineStyle == "BOLD" ? FontWeight.bold : FontWeight.normal,
                    fontStyle: inlineStyle == "ITALIC" ? FontStyle.italic : FontStyle.normal,
                    decoration: inlineStyle == "UNDERLINE" ? TextDecoration.underline : TextDecoration.none
                )
            );
            var textBlueStyle = newStyle.merge(
                new TextStyle(
                    fontWeight: inlineStyle == "BOLD" ? FontWeight.bold : FontWeight.normal,
                    fontStyle: inlineStyle == "ITALIC" ? FontStyle.italic : FontStyle.normal,
                    decoration: inlineStyle == "UNDERLINE" ? TextDecoration.underline : TextDecoration.none,
                    color: CColors.PrimaryBlue
                )
            );
            var recognizer = new TapGestureRecognizer {
                onTap = () => openUrl(data.data.url)
            };
            if (entityOffset >= inlineOffset + inlineLength) {
                var spans = new List<TextSpan> {
                    new TextSpan(text.Substring(0, inlineOffset), newStyle),
                    new TextSpan(text.Substring(inlineOffset, inlineLength), textStyle),
                    new TextSpan(
                        text.Substring(inlineOffset + inlineLength, entityOffset - inlineOffset - inlineLength),
                        newStyle),
                    new TextSpan(text.Substring(entityOffset, entityLength), newStyle.copyWith(CColors.PrimaryBlue),
                        recognizer: recognizer),
                    new TextSpan(text.Substring(entityOffset + entityLength, text.Length - entityOffset - entityLength),
                        newStyle)
                };
                return spans;
            }

            if (inlineOffset >= entityOffset + entityLength) {
                var spans = new List<TextSpan> {
                    new TextSpan(text.Substring(0, entityOffset), newStyle),
                    new TextSpan(text.Substring(entityOffset, entityLength), newStyle.copyWith(CColors.PrimaryBlue),
                        recognizer: recognizer),
                    new TextSpan(
                        text.Substring(entityOffset + entityLength, inlineOffset - entityOffset - entityLength),
                        newStyle),
                    new TextSpan(text.Substring(inlineOffset, inlineLength), textStyle),
                    new TextSpan(text.Substring(inlineOffset + inlineLength, text.Length - inlineOffset - inlineLength),
                        newStyle)
                };
                return spans;
            }

            if (entityOffset >= inlineOffset) {
                if (entityOffset + entityLength <= inlineOffset + inlineLength) {
                    var lastLength = inlineOffset + inlineLength - entityOffset - entityLength;
                    var spans = new List<TextSpan> {
                        new TextSpan(text.Substring(0, inlineOffset), newStyle),
                        new TextSpan(text.Substring(inlineOffset, entityOffset - inlineOffset), textStyle),
                        new TextSpan(text.Substring(entityOffset, entityLength), textBlueStyle, recognizer: recognizer),
                        new TextSpan(text.Substring(entityOffset + entityLength, lastLength), textStyle),
                        new TextSpan(
                            text.Substring(inlineOffset + inlineLength, text.Length - inlineOffset - inlineLength),
                            newStyle)
                    };
                    return spans;
                }
            }

            return null;
        }

        static List<TextSpan> _InlineStyle(string text, List<_InlineStyleRange> inlineStyleRanges, TextStyle style) {
            if (inlineStyleRanges == null && inlineStyleRanges.Count <= 0) {
                return null;
            }

            var inlineStyleRange = inlineStyleRanges.first();
            var offset = inlineStyleRange.offset;
            var length = inlineStyleRange.length;
            var inlineStyle = inlineStyleRange.style;
            var leftText = text.Substring(0, offset);
            var currentText = text.Substring(offset, length);
            var rightText = text.Substring(length + offset, text.Length - length - offset);
            var textStyle = style.merge(
                new TextStyle(
                    fontWeight: inlineStyle == "BOLD" ? FontWeight.bold : FontWeight.normal,
                    fontStyle: inlineStyle == "ITALIC" ? FontStyle.italic : FontStyle.normal,
                    decoration: inlineStyle == "UNDERLINE" ? TextDecoration.underline : TextDecoration.none
                )
            );
            return new List<TextSpan> {
                new TextSpan(
                    leftText,
                    style
                ),
                new TextSpan(
                    currentText,
                    textStyle
                ),
                new TextSpan(
                    rightText,
                    style
                )
            };
        }

        static List<TextSpan> _LinkSpans(
            string text,
            Dictionary<string, _EventContentEntity> entityMap,
            List<_EntityRange> entityRanges,
            Action<string> openUrl,
            TextStyle style
        ) {
            if (entityRanges != null && entityRanges.Count > 0) {
                var entityRange = entityRanges.first();
                var key = entityRange.key.ToString();
                if (entityMap.ContainsKey(key)) {
                    var data = entityMap[key];
                    if (data.type == "LINK") {
                        var offset = entityRange.offset;
                        var length = entityRange.length;
                        var leftText = text.Substring(0, offset);
                        var currentText = text.Substring(offset, length);
                        var rightText = text.Substring(length + offset, text.Length - length - offset);
                        var recognizer = new TapGestureRecognizer {
                            onTap = () => openUrl(data.data.url)
                        };
                        return new List<TextSpan> {
                            new TextSpan(
                                leftText,
                                style
                            ),
                            new TextSpan(
                                currentText,
                                style.copyWith(CColors.PrimaryBlue),
                                recognizer: recognizer
                            ),
                            new TextSpan(
                                rightText,
                                style
                            )
                        };
                    }
                }
            }

            return null;
        }
    }
}