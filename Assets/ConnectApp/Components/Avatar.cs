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

namespace ConnectApp.components {
    public enum OwnerType {
        user,
        team
    }

    public class Avatar : StatelessWidget {
        public Avatar(
            string id,
            float size = 36,
            OwnerType type = OwnerType.user,
            Key key = null
        ) : base(key) {
            D.assert(id != null);
            this.id = id;
            this.size = size;
            this.type = type;
        }

        private readonly string id;
        private readonly float size;
        private readonly OwnerType type;

        public override Widget build(BuildContext context) {
            if (type == OwnerType.team) return _buildTeamAvatar();

            return new StoreConnector<AppState, User>(
                converter: (state, dispatch) => state.userState.userDict.ContainsKey(id)
                    ? state.userState.userDict[id]
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

        private Widget _buildTeamAvatar() {
            return new StoreConnector<AppState, Team>(
                converter: (state, dispatch) => state.teamState.teamDict.ContainsKey(id)
                    ? state.teamState.teamDict[id]
                    : new Team(),
                builder: (_context, viewModel) => {
                    var avatarUrl = viewModel.avatar ?? "";
                    var name = viewModel.name;
                    var result = _extractName(name) ?? "";
                    if (avatarUrl.Length <= 0) return new _Placeholder(result, size);
                    return new Container(
                        width: size,
                        height: size,
                        child: Image.network(avatarUrl)
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
                    title.ToUpper(),
                    style: new TextStyle(
                        height: 0.9f,
                        color: CColors.White,
                        fontFamily: "Roboto-Medium",
                        fontSize: size * 0.45f
                    )
                )
            );
        }
    }
}