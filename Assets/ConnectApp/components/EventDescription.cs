using System.Collections.Generic;
using ConnectApp.models;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class EventDescription : StatelessWidget {
        public EventDescription(
            Key key = null,
            string content = null,
            Dictionary<string, ContentMap> contentMap = null
        ) : base(key) {
            this.content = content;
            this.contentMap = contentMap;
        }

        private readonly string content;
        private readonly Dictionary<string, ContentMap> contentMap;


        public override Widget build(BuildContext context) {
            if (content == null) return new Container();

            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: ArticleDescription.map(context, content, contentMap)
            );
        }
    }
}