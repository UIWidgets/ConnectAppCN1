using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components.pull_to_refresh {
    public enum IconPosition {
        left,
        right,
        top,
        bottom
    }

    public class ClassicIndicator : Indicator {
        public ClassicIndicator(
            int mode,
            string releaseText = null,
            string idleText = null,
            string refreshingText = null,
            string completeText = null,
            string failedText = null,
            string noDataText = null,
            Widget releaseIcon = null,
            Widget idleIcon = null,
            Widget noMoreIcon = null,
            Widget refreshingIcon = null,
            Widget completeIcon = null,
            Widget failedIcon = null,
            float height = 60,
            float spacing = 15,
            IconPosition iconPos = IconPosition.left,
            TextStyle textStyle = null,
            Key key = null
        ) : base(mode, key) {
            this.releaseText = releaseText ?? "";
            this.idleText = idleText ?? "";
            this.refreshingText = refreshingText ?? "";
            this.completeText = completeText ?? "";
            this.failedText = failedText ?? "";
            this.noDataText = noDataText ?? "";
            this.releaseIcon = releaseIcon ?? new CustomActivityIndicator(animating: AnimatingType.reset);
            this.idleIcon = idleIcon ?? new Icon(new IconData(1, "23"));
            this.noMoreIcon = noMoreIcon ?? new Icon(Icons.close, color: CColors.Grey);
            this.refreshingIcon = refreshingIcon ?? new CustomActivityIndicator();
            this.completeIcon = completeIcon ?? new CustomActivityIndicator(animating: AnimatingType.stop);
            this.failedIcon = failedIcon ?? new Icon(Icons.close, color: CColors.Grey);
            this.height = height;
            this.spacing = spacing;
            this.iconPos = iconPos;
            this.textStyle = textStyle;
        }

        public readonly string releaseText;
        public readonly string idleText;
        public readonly string refreshingText;
        public readonly string completeText;
        public readonly string failedText;
        public readonly string noDataText;
        public readonly Widget releaseIcon;
        public readonly Widget idleIcon;
        public readonly Widget refreshingIcon;
        public readonly Widget completeIcon;
        public readonly Widget failedIcon;
        public readonly Widget noMoreIcon;
        public readonly float height;
        public readonly float spacing;
        public readonly IconPosition iconPos;
        public readonly TextStyle textStyle;


        public override State createState() {
            return new _ClassicIndicatorState();
        }
    }


    class _ClassicIndicatorState : State<ClassicIndicator> {
        public override Widget build(BuildContext context) {
            Widget textWidget = this._buildText();
            Widget iconWidget = this._buildIcon();
            var children = new List<Widget> {
                iconWidget,
                new Container(
                    width: this.widget.spacing,
                    height: this.widget.spacing
                ),
                textWidget
            };
            Widget container = new Row(this.widget.iconPos == IconPosition.right
                    ? TextDirection.rtl
                    : TextDirection.ltr,
                mainAxisAlignment: MainAxisAlignment.center,
                children: children
            );
            if (this.widget.iconPos == IconPosition.top || this.widget.iconPos == IconPosition.bottom) {
                container = new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    verticalDirection: this.widget.iconPos == IconPosition.top
                        ? VerticalDirection.down
                        : VerticalDirection.up,
                    children: children
                );
            }

            return new Container(
                alignment: Alignment.center,
                height: this.widget.height,
                child: new Center(
                    child: container
                )
            );
        }


        Widget _buildText() {
            return new Text(this.widget.mode == RefreshStatus.canRefresh
                    ? this.widget.releaseText
                    : this.widget.mode == RefreshStatus.completed
                        ? this.widget.completeText
                        : this.widget.mode == RefreshStatus.failed
                            ? this.widget.failedText
                            : this.widget.mode == RefreshStatus.refreshing
                                ? this.widget.refreshingText
                                : this.widget.mode == RefreshStatus.noMore
                                    ? this.widget.noDataText
                                    : this.widget.idleText,
                style: this.widget.textStyle);
        }

        Widget _buildIcon() {
            Widget icon = this.widget.mode == RefreshStatus.canRefresh
                ? this.widget.releaseIcon
                : this.widget.mode == RefreshStatus.noMore
                    ? this.widget.noMoreIcon
                    : this.widget.mode == RefreshStatus.idle
                        ? this.widget.idleIcon
                        : this.widget.mode == RefreshStatus.completed
                            ? this.widget.completeIcon
                            : this.widget.mode == RefreshStatus.failed
                                ? this.widget.failedIcon
                                : new SizedBox(
                                    width: 24,
                                    height: 24,
                                    child: this.widget.refreshingIcon
                                );
            return icon;
        }
    }
}