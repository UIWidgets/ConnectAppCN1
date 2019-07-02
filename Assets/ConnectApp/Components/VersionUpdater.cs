using ConnectApp.Utils;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Components {
    public class VersionUpdater : StatefulWidget {
        public VersionUpdater(
            Widget child = null,
            Key key = null
        ) : base(key: key) {
            this.child = child;
        }

        public readonly Widget child;

        public override State createState() {
            return new _VersionUpdaterState();
        }
    }

    public class _VersionUpdaterState : State<VersionUpdater> {
        public override void initState() {
            base.initState();
            var needCheckUpdater = VersionManager.needCheckUpdater();
            if (needCheckUpdater) {
                VersionManager.checkForUpdates(type: CheckVersionType.first);
            }
        }

        public override Widget build(BuildContext context) {
            return this.widget.child;
        }
    }
}