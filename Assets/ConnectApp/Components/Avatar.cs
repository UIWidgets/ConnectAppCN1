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
        Avatar(
            string id,
            float size = 36,
            OwnerType type = OwnerType.user,
            User user = null,
            Team team = null,
            Key key = null
        ) : base(key) {
            D.assert(id != null);
            this.id = id;
            this.user = user ?? new User();
            this.team = team ?? new Team();
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


        readonly string id;
        readonly User user;
        readonly Team team;
        readonly float size;
        readonly OwnerType type;

        public override Widget build(BuildContext context) {
            if (this.type == OwnerType.team) {
                return this._buildTeamAvatar();
            }

            var avatarUrl = this.user.avatar ?? "";
            var fullName = this.user.fullName ?? this.user.name;
            var result = _extractName(fullName) ?? "";
            return new ClipRRect(
                borderRadius: BorderRadius.circular(this.size / 2),
                child: avatarUrl.isEmpty()
                    ? new Container(
                        child: new _Placeholder(result, this.size)
                    )
                    : new Container(
                        width: this.size,
                        height: this.size,
                        color: new Color(0xFFD8D8D8),
                        child: Image.network(avatarUrl, fit: BoxFit.cover)
                    )
            );
        }

        Widget _buildTeamAvatar() {
            var avatarUrl = this.team.avatar ?? "";
            var name = this.team.name;
            var result = _extractName(name) ?? "";
            if (avatarUrl.Length <= 0) {
                return new _Placeholder(result, this.size);
            }

            return new Container(
                width: this.size,
                height: this.size,
                color: new Color(0xFFD8D8D8),
                child: Image.network(avatarUrl)
            );
        }


        static string _extractName(string name) {
            if (name == null || name.Length <= 0) {
                return "";
            }

            name = name.Trim();
            var regex = new Regex(@"^\W+");
            if (regex.IsMatch(name)) {
                return name[0].ToString();
            }

            var sep = name.IndexOf(" ") > 0 ? ' ' : ',';
            var tokens = name.Split(sep);
            var length = tokens.Length;
            if (length > 1) {
                return $"{tokens[0][0]}{tokens[length - 1][0]}";
            }

            return tokens[0][0].ToString();
        }
    }

    class _Placeholder : StatelessWidget {
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
                width: this.size,
                height: this.size,
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
                child: new Container(
                    alignment: Alignment.center,
                    child: new Text(this.title.ToUpper(),
                        textAlign: TextAlign.center,
                        style: new TextStyle(
                            height: 1.30f,
                            color: CColors.White,
                            fontFamily: "Roboto-Medium",
                            fontSize: this.size * 0.45f
                        )
                    ))
            );
        }
    }
}