using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.components
{
    public class CustomTabBarItem
    {
        public CustomTabBarItem(
            int index,
            string activeImge,
            string inactiveImge,
            string title,
            Color activeColor,
            Color inActiveColor,
            int size = 17
        )
        {
            this.index = index;
            this.activeImge = activeImge;
            this.inactiveImge = inactiveImge;
            this.title = title;
            this.activeColor = activeColor;
            this.inActiveColor = inActiveColor;
            this.size = size;
        }

        public readonly int size;
        public readonly int index;
        public readonly string activeImge;
        public readonly string inactiveImge;
        public readonly string title;
        public readonly Color activeColor;
        public readonly Color inActiveColor;
    }
}