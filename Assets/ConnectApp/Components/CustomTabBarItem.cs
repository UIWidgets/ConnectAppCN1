using System.Collections.Generic;
using ConnectApp.Constants;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class CustomTabBarItem {
        public CustomTabBarItem(
            int index,
            IconData normalIcon,
            List<string> selectedImages,
            string title,
            Color activeColor = null,
            Color inActiveColor = null,
            int size = 24
        ) {
            this.index = index;
            this.normalIcon = normalIcon;
            this.selectedImages = selectedImages;
            this.title = title;
            this.activeColor = activeColor ?? CColors.TextBody;
            this.inActiveColor = inActiveColor ?? CColors.ShadyLady;
            this.size = size;
        }

        public readonly int size;
        public readonly int index;
        public readonly IconData normalIcon;
        public readonly List<string> selectedImages;
        public readonly string title;
        public readonly Color activeColor;
        public readonly Color inActiveColor;
    }
}