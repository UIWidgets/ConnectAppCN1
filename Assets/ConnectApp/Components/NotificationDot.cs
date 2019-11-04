using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class NotificationDot : StatelessWidget {
        readonly string content;
        readonly BorderSide borderSide;
        readonly EdgeInsets margin;

        public NotificationDot(
            string content,
            BorderSide borderSide = null,
            EdgeInsets margin = null,
            Key key = null
        ) : base(key: key) {
            this.content = content;
            this.borderSide = borderSide;
            this.margin = margin;
        }

        public override Widget build(BuildContext context) {
            if (this.content == null) {
                return new Container();
            }

            Widget content;
            if (this.content == "") {
                content = new Container(
                    width: 10,
                    height: 10,
                    decoration: new BoxDecoration(
                        borderRadius: BorderRadius.all(5),
                        color: CColors.Error
                    )
                );
            }
            else {
                content = new Container(
                    decoration: new BoxDecoration(
                        borderRadius: BorderRadius.all(8),
                        color: CColors.Error
                    ),
                    padding: EdgeInsets.symmetric(1, 4),
                    child: new Text(
                        $"{this.content}",
                        style: CTextStyle.PRedDot
                    )
                );
            }

            if (this.borderSide != null) {
                return new Container(
                    margin: this.margin,
                    padding: EdgeInsets.all(value: this.borderSide.width),
                    decoration: new BoxDecoration(
                        borderRadius: BorderRadius.all(8 + this.borderSide.width),
                        color: this.borderSide.color
                    ),
                    child: content
                );
            }

            return new Container(
                margin: this.margin,
                child: content
            );
        }
    }
}