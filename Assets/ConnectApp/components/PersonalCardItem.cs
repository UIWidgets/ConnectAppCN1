using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class PersonalCardItem {
        public PersonalCardItem(
            IconData icon,
            string title
        ) {
            this.icon = icon;
            this.title = title;
        }

        public readonly IconData icon;
        public readonly string title;
    }
}