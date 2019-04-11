using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConnectApp.constants;
using ConnectApp.models;
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
        private Avatar(
            string id,
            float size = 36,
            OwnerType type = OwnerType.user,
            User user = null,
            Team team = null,
            Key key = null
        ) : base(key) {
            D.assert(id != null);
            this.id = id;
            this.user = user;
            this.team = team;
            this.size = size;
            this.type = type;
        }

        public static Avatar User(
            string id,
            User user = null,
            float size = 36,
            Key key = null
        ) {
            return new Avatar(
                id,
                size,
                OwnerType.user,
                user,
                key: key
            );
        }

        public static Avatar Team(
            string id,
            Team team = null,
            float size = 36,
            Key key = null
        ) {
            return new Avatar(
                id,
                size,
                OwnerType.team,
                null,
                team,
                key
            );
        }


        private readonly string id;
        private readonly User user;
        private readonly Team team;
        private readonly float size;
        private readonly OwnerType type;

        public override Widget build(BuildContext context) {
            if (type == OwnerType.team) return _buildTeamAvatar();
            var avatarUrl = user.avatar ?? "";
            var fullName = user.fullName;
            var result = _extractName(fullName) ?? "";
            return new ClipRRect(
                borderRadius: BorderRadius.circular(size / 2),
                child: avatarUrl.isEmpty()
                    ? new Container(
                        child: new _Placeholder(result, size)
                    )
                    : new Container(
                        width: size,
                        height: size,
                        child: Image.network(avatarUrl, fit: BoxFit.cover)
                    )
            );
        }

        private Widget _buildTeamAvatar() {
            var avatarUrl = team.avatar ?? "";
            var name = team.name;
            var result = _extractName(name) ?? "";
            if (avatarUrl.Length <= 0) return new _Placeholder(result, size);
            return new Container(
                width: size,
                height: size,
                child: Image.network(avatarUrl)
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