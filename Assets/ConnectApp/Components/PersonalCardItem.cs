using Unity.UIWidgets.gestures;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class PersonalCardItem {
        public PersonalCardItem(
            IconData icon,
            string title,
            GestureTapCallback onTap
        ) {
            this.icon = icon;
            this.title = title;
            this.onTap = onTap;
        }

        public readonly IconData icon;
        public readonly string title;
        public readonly GestureTapCallback onTap;
    }
}