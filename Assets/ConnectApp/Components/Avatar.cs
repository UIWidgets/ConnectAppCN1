using System.Collections.Generic;
using System.Text.RegularExpressions;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Image = Unity.UIWidgets.widgets.Image;

namespace ConnectApp.Components {
    public enum OwnerType {
        user,
        team
    }

    public class Avatar : StatelessWidget {
        Avatar(
            string avatarUrl,
            string fullName,
            float size = 36,
            OwnerType type = OwnerType.user,
            Key key = null
        ) : base(key: key) {
            this.avatarUrl = avatarUrl ?? "";
            this.fullName = fullName ?? "";
            this.size = size;
            this.type = type;
        }

        public static Avatar User(
            User user,
            float size = 36,
            Key key = null
        ) {
            return new Avatar(
                avatarUrl: user.avatar,
                user.fullName ?? user.name,
                size: size,
                type: OwnerType.user,
                key: key
            );
        }

        public static Avatar Team(
            Team team,
            float size = 36,
            Key key = null
        ) {
            return new Avatar(
                avatarUrl: team.avatar,
                fullName: team.name,
                size: size,
                type: OwnerType.team,
                key: key
            );
        }

        readonly string avatarUrl;
        readonly string fullName;
        readonly float size;
        readonly OwnerType type;

        public override Widget build(BuildContext context) {
            if (this.type == OwnerType.team) {
                return this._buildTeamAvatar();
            }

            return new Container(
                width: this.size,
                height: this.size,
                decoration: new BoxDecoration(
                    borderRadius: BorderRadius.circular(this.size / 2),
                    border: Border.all(
                        color: CColors.White,
                        2
                    )
                ),
                child: new ClipRRect(
                    borderRadius: BorderRadius.circular(this.size / 2),
                    child: this.avatarUrl.isEmpty()
                        ? new Container(
                            child: new _Placeholder(
                                _extractName(name: this.fullName) ?? "",
                                this.size - 4
                            )
                        )
                        : new Container(
                            width: this.size - 4,
                            height: this.size - 4,
                            color: new Color(0xFFD8D8D8),
                            child: Image.network(src: this.avatarUrl)
                        )
                )
            );
        }

        Widget _buildTeamAvatar() {
            if (this.avatarUrl.isEmpty()) {
                return new _Placeholder(
                    _extractName(name: this.fullName) ?? "",
                    size: this.size
                );
            }

            return new Container(
                width: this.size,
                height: this.size,
                color: new Color(0xFFD8D8D8),
                child: Image.network(src: this.avatarUrl)
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
        ) : base(key: key) {
            D.assert(title != null);
            this.title = title;
            this.size = size;
        }

        readonly string title;
        readonly float size;

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
                    )
                )
            );
        }
    }
}