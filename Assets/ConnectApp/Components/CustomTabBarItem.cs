using ConnectApp.Constants;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomTabBarItem {
        public CustomTabBarItem(
            int index,
            IconData normalIcon,
            IconData selectedIcon,
            string title,
            Color activeColor = null,
            Color inActiveColor = null,
            int size = 24,
            string notification = null
        ) {
            this.index = index;
            this.normalIcon = normalIcon;
            this.selectedIcon = selectedIcon;
            this.title = title;
            this.activeColor = activeColor ?? CColors.PrimaryBlue;
            this.inActiveColor = inActiveColor ?? CColors.TextBody4;
            this.size = size;
            this.notification = notification;
        }

        public readonly int size;
        public readonly int index;
        public readonly IconData normalIcon;
        public readonly IconData selectedIcon;
        public readonly string title;
        public readonly Color activeColor;
        public readonly Color inActiveColor;
        public readonly string notification;
    }
}