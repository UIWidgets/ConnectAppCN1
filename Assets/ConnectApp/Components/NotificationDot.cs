using ConnectApp.Constants;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class NotificationDot : StatelessWidget {

        public readonly string content;
        public readonly BorderSide borderSide;
        public NotificationDot(string content, BorderSide borderSide = null) {
            this.content = content;
            this.borderSide = borderSide;
        }
        
        public override Widget build(BuildContext context) {
            if (this.content == null) {
                return new Container();
            }
            
            if (this.content == "") {
                return new Container(
                    width: 10,
                    height: 10,
                    decoration: new BoxDecoration(
                        borderRadius: BorderRadius.all(5),
                        color: CColors.Error
                    )
                );
            }

            return new Container(
                decoration: new BoxDecoration(
                    borderRadius: BorderRadius.all(8),
                    color: CColors.Error
                ),
                padding: EdgeInsets.symmetric(1, 4),
                child: new Text($"{this.content}",
                    style: CTextStyle.PRedDot)
            );
        }
    }
}