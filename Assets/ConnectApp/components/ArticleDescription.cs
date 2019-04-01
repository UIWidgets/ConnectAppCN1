using System.Collections.Generic;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using Color = Unity.UIWidgets.ui.Color;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.components {
    public static class ArticleDescription {
        public static List<Widget> map(BuildContext context, string cont, Dictionary<string, ContentMap> contentMap) {
            if (cont == null) return new List<Widget>();

            var content = JsonConvert.DeserializeObject<EventContent>(cont);
            var widgets = new List<Widget>();

            var _isFirstUnorderedListItem = false;
            var _isFirstOrderedListItem = false;
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
                        widgets.Add(_CodeBlock(context, text));
                        break;
                    case "unstyled":
                        if (text != null || text.Length > 0)
                            widgets.Add(
                                _Unstyled(
                                    text,
                                    content.entityMap,
                                    block.entityRanges
                                )
                            );
                        break;
                    case "unordered-list-item": {
                        if (_isFirstUnorderedListItem) break;
                        var items = new List<string>();
                        var unorderedItem = blocks.FindAll(item => item.type == "unordered-list-item");
                        unorderedItem.ForEach(item => items.Add(item.text));

                        widgets.Add(_UnorderedList(items));
                        _isFirstUnorderedListItem = true;
                    }
                        break;
                    case "ordered-list-item": {
                        if (_isFirstOrderedListItem) break;
                        var items = new List<string>();
                        var orderedItem = blocks.FindAll(item => item.type == "ordered-list-item");
                        orderedItem.ForEach(item => items.Add(item.text));

                        widgets.Add(_OrderedList(items));
                        _isFirstOrderedListItem = true;
                    }
                        break;
                    case "atomic": {
                        var key = block.entityRanges.first().key.ToString();
                        if (content.entityMap.ContainsKey(key)) {
                            var dataMap = content.entityMap[key];
                            var data = dataMap.data;
                            if (data.contentId.isNotEmpty())
                            {
                                if (contentMap.ContainsKey(data.contentId)) {
                                    var map = contentMap[data.contentId];
                                    var originalImage = map.originalImage == null
                                        ? map.thumbnail
                                        : map.originalImage;
                                    widgets.Add(_Atomic(context,dataMap.type, data.title, originalImage));
                                }
                            }
                            
                        }
                    }
                        break;
                }
            }

            return widgets;
        }

        private static Widget _H1(string text) {
            if (text == null) return new Container();
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16, 24),
                child: new Text(
                    text,
                    style: CTextStyle.H4
                )
            );
        }

        private static Widget _H2(string text) {
            if (text == null) return new Container();
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, 16, 16, 24),
                child: new Text(
                    text,
                    style: CTextStyle.H5
                )
            );
        }

        private static Widget _Unstyled(
            string text,
            Dictionary<string, _EventContentEntity> entityMap,
            List<_EntityRange> entityRanges
        ) {
            if (text == null) return new Container();
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
                            onTap = () => {
                                StoreProvider.store.Dispatch(new OpenUrlAction{url = data.data.url});
                            }
                        };
                        return new Container(
                            color: CColors.White,
                            padding: EdgeInsets.only(16, right: 16, bottom: 24),
                            child: new RichText(
                                text: new TextSpan(
                                    children: new List<TextSpan> {
                                        new TextSpan(
                                            leftText,
                                            CTextStyle.PXLarge
                                        ),
                                        new TextSpan(
                                            currentText,
                                            CTextStyle.PXLargeBlue,
                                            recognizer: recognizer
                                        ),
                                        new TextSpan(
                                            rightText,
                                            CTextStyle.PXLarge
                                        )
                                    }
                                )
                            )
                        );
                    }
                }
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, bottom: 24),
                child: new Text(
                    text,
                    style: CTextStyle.PXLarge
                )
            );
        }

        private static Widget _CodeBlock(BuildContext context, string text) {
            if (text == null) return new Container();
            return new Container(
                color: Color.fromRGBO(110, 198, 255, 0.12f),
                width: MediaQuery.of(context).size.width,
                child: new Container(
                    padding: EdgeInsets.all(16),
                    child: new Text(
                        text,
                        style: CTextStyle.PRegularBody
                    )
                )
            );
        }


        private static Widget _QuoteBlock(string text) {
            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(16, right: 16, bottom: 24),
                child: new Container(
                    decoration: new BoxDecoration(
                        color:CColors.White,
                        border: new Border(
                            left: new BorderSide(
                                CColors.Separator,
                                8
                            )
                        )
                    ),
                    padding: EdgeInsets.only(16),
                    child: new Text(
                        text,
                        style: CTextStyle.PXLargeBody4
                    )
                )
            );
        }

        private static Widget _Atomic(BuildContext context,string type, string title, _OriginalImage originalImage) {
            if (type == "ATTACHMENT") return new Container();

            var playButton = Positioned.fill(
                new Container()
            );
            if (type == "VIDEO")
                playButton = Positioned.fill(
                    new Center(
                        child: new CustomButton(
                            child: new Container(
                                width: 80,
                                height: 80,
                                decoration: new BoxDecoration(
                                    CColors.White,
                                    borderRadius: BorderRadius.all(40)
                                ),
                                child: new Icon(
                                    Icons.play_arrow,
                                    size: 64,
                                    color: CColors.icon3
                                )
                            )
                        )
                    )
                );
            var nodes = new List<Widget> {
                new Stack(
                    children: new List<Widget> {
                        new Container(
                            color:CColors.Separator,
                            width:MediaQuery.of(context).size.width-32,
                            height:(MediaQuery.of(context).size.width-32)*originalImage.height / originalImage.width,
                            child: Image.network(originalImage.url ?? "",fit:BoxFit.cover)
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


        private static Widget _OrderedList(List<string> items) {
            var widgets = new List<Widget>();

            for (var i = 0; i < items.Count; i++) {
                var spans = new List<TextSpan> {
                    new TextSpan(
                        $"{i + 1}. ",
                        CTextStyle.PXLarge
                    ),
                    new TextSpan(
                        items[i],
                        CTextStyle.PXLarge
                    )
                };
                widgets.Add(
                    new Container(
                        padding: EdgeInsets.only(16, right: 16),
                        margin: EdgeInsets.only(top: i == 0 ? 0 : 4),
                        child: new RichText(
                            text: new TextSpan(
                                style: CTextStyle.PXLarge,
                                children: spans
                            )
                        )
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 24),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: widgets
                )
            );
        }

        private static Widget _UnorderedList(List<string> items) {
            var widgets = new List<Widget>();

            for (var i = 0; i < items.Count; i++) {
                var spans = new List<Widget> {
                    new Container(
                        width: 8,
                        height: 8,
                        margin: EdgeInsets.only(top: 12, right: 8),
                        decoration: new BoxDecoration(
                            CColors.Black,
                            borderRadius: BorderRadius.all(4)
                        )
                    ),
                    new Expanded(
                        child: new Text(
                            items[i],
                            style: CTextStyle.PXLarge
                        )
                    )
                };
                widgets.Add(
                    new Container(
                        padding: EdgeInsets.only(16, right: 16),
                        margin: EdgeInsets.only(top: i == 0 ? 0 : 4),
                        child: new Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: spans
                        )
                    )
                );
            }

            return new Container(
                color: CColors.White,
                padding: EdgeInsets.only(bottom: 24),
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: widgets
                )
            );
        }
    }
}