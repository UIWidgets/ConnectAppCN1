using System.Collections.Generic;
using System.Text;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class CColorUtils {
        static readonly List<Color> ColorsList = new List<Color> {
            CColors.Coral,
            CColors.Orange,
            CColors.Mustard,
            CColors.Feijoa,
            CColors.Riptide,
            CColors.SkyBlue,
            CColors.JordyBlue,
            CColors.Violet,
            CColors.Purple,
            CColors.Comet
        };
        
        static readonly List<Color> DarkColorsList = new List<Color> {
            CColors.DarkCoral,
            CColors.DarkOrange,
            CColors.DarkMustard,
            CColors.DarkFeijoa,
            CColors.DarkRiptide,
            CColors.DarkSkyBlue,
            CColors.DarkJordyBlue,
            CColors.DarkViolet,
            CColors.DarkPurple,
            CColors.DarkComet
        };
        
        static readonly List<Color> CardColorsList = new List<Color> {
            CColors.DarkSkyBlue,
            CColors.DarkJordyBlue,
            CColors.DarkViolet,
            CColors.DarkPurple,
            CColors.DarkComet
        };

        public static Color GetSpecificColorFromId(string id) {
            return id.isEmpty() ? CColors.LoadingGrey : ColorsList[CCommonUtils.GetStableHash(s: id) % ColorsList.Count];
        }
        
        public static Color GetSpecificDarkColorFromId(string id) {
            return id.isEmpty() ? CColors.LoadingGrey : DarkColorsList[CCommonUtils.GetStableHash(s: id) % DarkColorsList.Count];
        }
        
        public static Color GetCardColorFromId(string id) {
            return id.isEmpty() ? CColors.LoadingGrey : CardColorsList[CCommonUtils.GetStableHash(s: id) % CardColorsList.Count];
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