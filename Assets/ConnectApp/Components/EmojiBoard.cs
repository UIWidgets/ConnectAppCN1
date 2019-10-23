using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.material;
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
            0x1F600, 0x1F603, 0x1F604, 0x1F601, 0x1F606, 0x1F605, 0x1F602, 0x1F923, 0x1F62D,
            0x1F617, 0x1F619, 0x1F61A, 0x1F618, 0x263A, 0x1F60A, 0x1F970, 0x1F60D, 0x1F929,
            0x1F917, 0x1F642, 0x1F643, 0x1F609, 0x1F60B, 0x1F61B, 0x1F61D, 0x1F61C, 0x1F92A,
            0x1F914, 0x1F928, 0x1F9D0, 0x1F644, 0x1F60F, 0x1F612, 0x1F623, 0x1F614, 0x1F60C,
            0x2639, 0x1F641, 0x1F615, 0x1F61F, 0x1F97A, 0x1F62C, 0x1F910, 0x1F92B, 0x1F92D,
            0x1F630, 0x1F628, 0x1F627, 0x1F626, 0x1F62E, 0x1F62F, 0x1F632, 0x1F633, 0x1F92F,
            0x1F622, 0x1F625, 0x1F613, 0x1F61E, 0x1F616, 0x1F629, 0x1F62B, 0x1F635, 0x1F631,
            0x1F922, 0x1F92E, 0x1F927, 0x1F637, 0x1F974, 0x1F912, 0x1F915, 0x1F975, 0x1F976,
            0x1F636, 0x1F610, 0x1F611, 0x1F624, 0x1F924, 0x1F634, 0x1F62A, 0x1F607, 0x1F920,
            0x1F973, 0x1F60E, 0x1F913, 0x1F925
        };
        TabController _emojiTabController;

        public override void initState() {
            base.initState();
            this._emojiTabController = new TabController(
                length: (emojiList.Count - 1) / emojiBoardPageSize + 1,
                vsync: this
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
                            child: new TabBarView(
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