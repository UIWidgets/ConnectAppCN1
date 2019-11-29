using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Icons = ConnectApp.Constants.Icons;

namespace ConnectApp.Components {
    public delegate void HandleEmojiCallback(string emojiText);

    public class EmojiBoard : StatefulWidget {
        public EmojiBoard(
            HandleEmojiCallback handleEmoji = null,
            GestureTapCallback handleDelete = null,
            GestureTapCallback handleSubmit = null,
            Key key = null
        ) : base(key: key) {
            this.handleEmoji = handleEmoji;
            this.handleDelete = handleDelete;
            this.handleSubmit = handleSubmit;
        }

        public readonly HandleEmojiCallback handleEmoji;
        public readonly GestureTapCallback handleDelete;
        public readonly GestureTapCallback handleSubmit;

        public override State createState() {
            return new _EmojiBoardState();
        }
    }

    class _EmojiBoardState : State<EmojiBoard>, TickerProvider {
        const int emojiBoardRowSize = 8;
        const int emojiBoardColumnSize = 3;
        static readonly List<int> emojiList = new List<int> {
            0x1F642, 0x1F615, 0x1F60D, 0x1F62F, 0x1F60E, 0x1F62D,
            0x1F60C, 0x1F910, 0x1F634, 0x1F622, 0x1F605, 0x1F621,
            0x1F61C, 0x1F601, 0x1F62E, 0x2639, 0x1F62F, 0x1F62B,
            0x1F922, 0x1F92D, 0x1F60A, 0x1F644, 0x1F624, 0x1F62A,
            0x1F628, 0x1F613, 0x1F603, 0x1F920, 0x1F607, 0x1F92C,
            0x1F9D0, 0x1F92B, 0x1F635, 0x1F643, 0x1F480, 0x1F610,
            0x1F636, 0x1F611, 0x1F925, 0x1F973, 0x1F913, 0x1F608,
            0x1F47F, 0x1F971, 0x1F616, 0x1F623, 0x1F61E, 0x1F606,
            0x1F619, 0x1F97A, 0x1F52A, 0x1F349, 0x1F37A, 0x2615,
            0x1F437, 0x1F339, 0x1F940, 0x1F48B, 0x2764, 0x1F494,
            0x1F382, 0x1F4A3, 0x1F4A9, 0x1F31D, 0x1F31E, 0x1F917,
            0x1F44D, 0x1F44E, 0x1F91D, 0x270C, 0x1F44F, 0x1F919,
            0x270A, 0x1F44C, 0x1F926, 0x1F937, 0x1F64B, 0x1F645,
            0x1F604, 0x1F637, 0x1F602, 0x1F61D, 0x1F633, 0x1F631,
            0x1F614, 0x1F612, 0x1F92A, 0x1F926, 0x1F60F, 0x1F914,
            0x1F928, 0x1F60B, 0x1F47B, 0x1F64F, 0x1F4AA, 0x1F389,
            0x1F381, 0x1F9E7
        };
        CustomTabController _emojiTabController;

        public override void initState() {
            base.initState();
            this._emojiTabController = new CustomTabController(
                (emojiList.Count - 1) / emojiBoardPageSize + 1,
                this
            );
        }

        public override void dispose() {
            this._emojiTabController.dispose();
            base.dispose();
        }

        public Ticker createTicker(TickerCallback onTick) {
            return new Ticker(onTick: onTick, () => $"created by {this}");
        }

        static float emojiSizeFactor {
            get { return 0.7f; }
        }

        static int emojiBoardPageSize {
            get { return emojiBoardRowSize * emojiBoardColumnSize - 1; }
        }

        static float _getEmojiButtonSize(BuildContext context) {
            return (MediaQuery.of(context: context).size.width - 42 - (emojiBoardRowSize - 1) * 2) / emojiBoardRowSize;
        }

        static float _getEmojiSize(BuildContext context) {
            return _getEmojiButtonSize(context: context) * emojiSizeFactor / EmojiUtils.sizeFactor;
        }

        static string _getEmojiText(int index) {
            if (index < emojiList.Count) {
                return emojiList[index: index] > 0x10000
                    ? char.ConvertFromUtf32(emojiList[index: index])
                    : $"{(char) emojiList[index: index]}";
            }

            return "";
        }

        public override Widget build(BuildContext context) {
            return new Column(
                children: new List<Widget> {
                    this._buildEmojiBoard(context: context),
                    this._buildEmojiBottomBar(),
                    new Container(
                        color: CColors.White,
                        height: MediaQuery.of(context: context).padding.bottom
                    )
                }
            );
        }

        Widget _buildEmojiBoard(BuildContext context) {
            var emojiButtonSize = _getEmojiButtonSize(context: context);
            return new Container(
                decoration: new BoxDecoration(
                    color: CColors.White,
                    border: new Border(new BorderSide(color: CColors.Separator))
                ),
                padding: EdgeInsets.symmetric(24),
                child: new Column(
                    children: new List<Widget> {
                        new Container(
                            height: (emojiButtonSize + 8) * (emojiBoardColumnSize - 1) + emojiButtonSize,
                            child: new CustomTabBarView(
                                controller: this._emojiTabController,
                                children: this._buildEmojiBoardPages(context: context)
                            )
                        ),
                        new Container(height: 16),
                        new CustomTabPageSelector(
                            controller: this._emojiTabController,
                            indicatorSize: 8,
                            selectedColor: CColors.PrimaryBlue,
                            color: CColors.Disable2
                        )
                    }
                )
            );
        }

        Widget _buildEmojiBottomBar() {
            return new Container(
                color: CColors.EmojiBottomBar,
                height: 36,
                child: new Row(
                    children: new List<Widget> {
                        new Container(width: 16),
                        new GestureDetector(
                            child: new Container(
                                height: 36,
                                width: 44,
                                color: CColors.White,
                                child: new Center(
                                    child: new Text(
                                        char.ConvertFromUtf32(0x1f642),
                                        style: new TextStyle(fontSize: 24, height: 1)
                                    )
                                )
                            )
                        ),
                        new Expanded(child: new Container()),
                        new Container(
                            width: 1,
                            height: 16,
                            color: CColors.Separator
                        ),
                        new GestureDetector(
                            onTap: this.widget.handleSubmit,
                            child: new Container(
                                width: 60,
                                height: 36,
                                color: CColors.Transparent,
                                child: new Center(
                                    child: new Text(
                                        "发送",
                                        style: CTextStyle.PMediumBlue.copyWith(height: 1)
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }

        List<Widget> _buildEmojiBoardPages(BuildContext context) {
            var emojiButtonSize = _getEmojiButtonSize(context: context);
            List<Widget> emojiPages = new List<Widget>();
            for (int i = 0; i < emojiList.Count; i += emojiBoardPageSize) {
                List<Widget> rows = new List<Widget>();
                for (int j = 0; j < emojiBoardColumnSize; j++) {
                    List<Widget> emojis = new List<Widget>();
                    for (int k = 0; k < emojiBoardRowSize; k++) {
                        emojis.Add(j == emojiBoardColumnSize - 1 && k == emojiBoardRowSize - 1
                            ? this._buildDeleteButton(context: context)
                            : this._buildEmojiButton(context: context, i: i, j: j, k: k));
                    }
                    if (j > 0) {
                        rows.Add(new Container(height: 8));
                    }

                    rows.Add(new Container(
                        height: emojiButtonSize,
                        padding: EdgeInsets.symmetric(0, 20),
                        child: new Row(
                            mainAxisAlignment: MainAxisAlignment.spaceBetween,
                            children: emojis
                        )
                    ));
                }

                emojiPages.Add(new Container(
                    width: MediaQuery.of(context: context).size.width,
                    child: new Column(
                        mainAxisSize: MainAxisSize.min,
                        children: rows
                    )
                ));
            }

            return emojiPages;
        }

        Widget _buildDeleteButton(BuildContext context) {
            var emojiButtonSize = _getEmojiButtonSize(context: context);
            return new GestureDetector(
                onTap: this.widget.handleDelete,
                child: new Container(
                    width: emojiButtonSize,
                    height: emojiButtonSize,
                    padding: EdgeInsets.only(2),
                    child: new Center(
                        child: new Icon(
                            icon: Icons.outline_delete_keyboard,
                            size: emojiButtonSize * 0.7f,
                            color: CColors.Icon
                        )
                    )
                )
            );
        }

        Widget _buildEmojiButton(BuildContext context, int i, int j, int k) {
            var emojiButtonSize = _getEmojiButtonSize(context: context);
            int index = i + j * emojiBoardRowSize + k;
            var emojiText = _getEmojiText(index: index);

            return new GestureDetector(
                onTap: () => this.widget.handleEmoji(emojiText: emojiText),
                child: new Container(
                    width: emojiButtonSize,
                    height: emojiButtonSize,
                    color: CColors.Transparent,
                    padding: k == 0 ? EdgeInsets.zero : EdgeInsets.only(2),
                    child: new Center(
                        child: new Text(
                            data: emojiText,
                            style: new TextStyle(fontSize: _getEmojiSize(context: context), height: 1.46f)
                        )
                    )
                )
            );
        }
    }
}