using Unity.UIWidgets.widgets;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;

namespace ConnectApp.components {
    
    public class Avatar : StatelessWidget {
        public Avatar(
            string avatarUrl,
            Key key,
            float size = 36.0f
        ) : base(key) {
            D.assert(avatarUrl != null);
            this.avatarUrl = avatarUrl;
            this.size = size;
        }

        public readonly string avatarUrl;
        public readonly float size;

        public override Widget build(BuildContext context) {
            return new ClipRRect(
                borderRadius: BorderRadius.circular(size / 2.0f),
                child: new Container(
                    width: size,
                    height: size,
                    child: Image.network(avatarUrl)
                )
            );
        }
    }
}