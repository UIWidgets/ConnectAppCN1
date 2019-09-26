using System.Collections.Generic;
using System.Text;
using ConnectApp.Constants;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class CColorUtils {
        public static readonly List<Color> ColorsList = new List<Color> {
            CColors.Gerakdine,
            CColors.Tan,
            CColors.Mustard,
            CColors.Feijoa,
            CColors.Riptide,
            CColors.SkyBlue,
            CColors.Portage,
            CColors.DullLavender,
            CColors.BrightLavender,
            CColors.Comet
        };

        public static Color GetAvatarBackgroundColor(string id) {
            return ColorsList[GetStableHash(s: id) % ColorsList.Count];
        }

        const int MUST_BE_LESS_THAN = 10000; // 4 decimal digits

        public static int GetStableHash(string s) {
            uint hash = 0;
            // if you care this can be done much faster with unsafe 
            // using fixed char* reinterpreted as a byte*
            foreach (byte b in Encoding.Unicode.GetBytes(s)) {
                hash += b;
                hash += (hash << 10);
                hash ^= (hash >> 6);
            }

            // final avalanche
            hash += (hash << 3);
            hash ^= (hash >> 11);
            hash += (hash << 15);
            // helpfully we only want positive integer < MUST_BE_LESS_THAN
            // so simple truncate cast is ok if not perfect
            return (int) (hash % MUST_BE_LESS_THAN);
        }

        public static readonly List<Color> FavoriteCoverColors = new List<Color> {
            Color.fromRGBO(255, 134, 134, 0.6f),
            Color.fromRGBO(255, 171, 109, 0.6f),
            Color.fromRGBO(255, 219, 85, 0.6f),
            Color.fromRGBO(173, 227, 118, 0.6f),
            Color.fromRGBO(128, 229, 215, 0.6f),
            Color.fromRGBO(134, 217, 237, 0.6f),
            Color.fromRGBO(141, 168, 242, 0.6f),
            Color.fromRGBO(158, 145, 248, 0.6f),
            Color.fromRGBO(197, 134, 243, 0.6f),
            Color.fromRGBO(99, 102, 114, 0.6f)
        };
    }
}