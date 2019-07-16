using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class FollowButton : StatelessWidget {
        public FollowButton(
            UserType userType = UserType.unFollow,
            GestureTapCallback onFollow = null,
            Key key = null
        ) : base(key: key) {
            this.userType = userType;
            this.onFollow = onFollow;
        }

        readonly UserType userType;
        readonly GestureTapCallback onFollow;

        public override Widget build(BuildContext context) {
            if (this.userType == UserType.me) {
                return new Container();
            }

            Widget buttonChild;
            Color followColor = CColors.PrimaryBlue;
            if (this.userType == UserType.loading) {
                followColor = CColors.Disable2;
                buttonChild = new CustomActivityIndicator(
                    size: LoadingSize.xSmall
                );
            }
            else {
                string followText = "关注";
                Color textColor = CColors.PrimaryBlue;
                if (this.userType == UserType.follow) {
                    followText = "已关注";
                    followColor = CColors.Disable2;
                    textColor = new Color(0xFF959595);
                }
                buttonChild = new Text(
                    data: followText,
                    style: new TextStyle(
                        fontSize: 14,
                        fontFamily: "Roboto-Medium",
                        color: textColor
                    )
                );
            }
            return new CustomButton(
                onPressed: () => this.onFollow(),
                padding: EdgeInsets.zero,
                child: new Container(
                    width: 60,
                    height: 28,
                    alignment: Alignment.center,
                    decoration: new BoxDecoration(
                        color: CColors.White,
                        borderRadius: BorderRadius.circular(14),
                        border: Border.all(color: followColor)
                    ),
                    child: buttonChild
                )
            );
        }
    }
}