//using Unity.UIWidgets.ui;

using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomTabBarItem {
        public CustomTabBarItem(
            int index,
            IconData icon,
            string title,
            Color activeColor,
            Color inActiveColor,
            int size = 24
        ) {
            this.index = index;
            this.icon = icon;
            this.title = title;
            this.activeColor = activeColor;
            this.inActiveColor = inActiveColor;
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