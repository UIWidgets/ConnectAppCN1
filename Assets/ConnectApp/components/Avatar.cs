using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.redux;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.components {
    public class Avatar : StatelessWidget {
        public Avatar(
            string userId,
            Key key = null,
            float size = 36
        ) : base(key) {
            D.assert(userId != null);
            this.userId = userId;
            this.size = size;
        }

        public readonly string userId;
        public readonly float size;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, User>(
                converter: (state, dispatch) => state.userState.userDict.ContainsKey(userId)
                    ? state.userState.userDict[userId]
                    : new User(),
                builder: (_context, viewModel) => {
                    var avatarUrl = viewModel.avatar ?? "";
                    var fullName = viewModel.fullName;
                    var result = _extractName(fullName) ?? "";
                    return new ClipRRect(
                        borderRadius: BorderRadius.circular(size / 2),
                        child: avatarUrl.Length <= 0
                            ? new Container(
                                child: new _Placeholder(result, size)
                            )
                            : new Container(
                                width: size,
                                height: size,
                                child: Image.network(avatarUrl)
                            )
                    );
                }
            );
        }

        private static string _extractName(string name) {
            if (name == null || name.Length <= 0) return "";
            name = name.Trim();
            var regex = new Regex(@"^\W+");
            if (regex.IsMatch(name)) return name[0].ToString();
            var sep = name.IndexOf(" ") > 0 ? ' ' : ',';
            var tokens = name.Split(sep);
            var length = tokens.Length;
            if (length > 1) return $"{tokens[0][0]}{tokens[length - 1][0]}";
            return tokens[0][0].ToString();
        }
    }

    internal class _Placeholder : StatelessWidget {
        public _Placeholder(
            string title,
            float size = 36,
            Key key = null
        ) : base(key) {
            D.assert(title != null);
            this.title = title;
            this.size = size;
        }

        public readonly string title;
        public readonly float size;

        public override Widget build(BuildContext context) {
            return new Container(
                width: size,
                height: size,
                alignment: Alignment.center,
                decoration: new BoxDecoration(
                    gradient: new LinearGradient(
                        colors: new List<Color> {
                            Color.fromARGB(255, 25, 113, 114),
                            Color.fromARGB(255, 123, 188, 32)
                        },
                        begin: Alignment.topLeft,
                        end: Alignment.bottomRight
                    )
                ),
                child: new Text(
                    title,
                    style: new TextStyle(
                        color: CColors.White,
                        fontSize: size * 0.45f
                    )
                )
            );
        }
    }
}