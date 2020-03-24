using System;
using ConnectApp.redux;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public class GamepadButton : StatefulWidget {
        public GamepadButton(
            Key key = null
        ) : base(key: key) {
        }
        
        public override State createState() {
            return new _GamepadButtonState();
        }
    }

    class _GamepadButtonState : State<GamepadButton> {
        public override void initState() {
            base.initState();
            int currentTabIndex = StoreProvider.store.getState().tabBarState.currentTabIndex;
            if (currentTabIndex != 0) {
                return;
            }
        
            Promise.Delayed(TimeSpan.FromMilliseconds(1)).Then(() => {
                var rect = this.context.getContextRect();
                var kingKongTypes = PreferencesManager.initKingKongType();
                if (!kingKongTypes.Contains(item: KingKongType.tinyGame)) {
                    this._showTinyGameDialog(rect: rect);
                }
            });
        }
        
        void _showTinyGameDialog(Rect rect) {
            PreferencesManager.updateKingKongType(type: KingKongType.tinyGame);
            CustomDialogUtils.showCustomDialog(
                child: new Bubble(
                    "这里可以看 Tiny Projects 哦",
                    rect.left + rect.width / 2.0f,
                    rect.bottom + 8 - CCommonUtils.getSafeAreaTopPadding(context: this.context),
                    contentRight: 16
                ),
                barrierDismissible: true,
                onPop: () => {}
            );
        }
        public override Widget build(BuildContext context) {
            return Image.asset("image/egg-gamepad");
        }
    }
}