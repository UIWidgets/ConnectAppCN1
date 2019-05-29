using ConnectApp.constants;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomTabBarItem {
        public CustomTabBarItem(
            int index,
            IconData icon,
            string title,
            Color activeColor = null,
            Color inActiveColor = null,
            int size = 24
        ) {
            this.index = index;
            this.icon = icon;
            this.title = title;
            this.activeColor = activeColor ?? CColors.PrimaryBlue;
            this.inActiveColor = inActiveColor ?? CColors.BrownGrey;
            this.size = size;
        }

        public readonly int size;
        public readonly int index;
        public readonly IconData icon;
        public readonly string title;
        public readonly Color activeColor;
        public readonly Color inActiveColor;
    }
}