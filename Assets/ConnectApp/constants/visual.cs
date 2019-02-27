using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

namespace ConnectApp.constants {
    public static class Icons {
        
//          首页
        public static readonly IconData TabHome = new IconData(0xe873, "Material Icons");
        public static readonly IconData TabNotification = new IconData(0xe7f4, "Material Icons");
        public static readonly IconData TabMood = new IconData(0xe7f2, "Material Icons");

        
        
        public static readonly IconData notifications = new IconData(0xe7f4, "Material Icons");
        public static readonly IconData account_circle = new IconData(0xe853, "Material Icons");
        public static readonly IconData search = new IconData(0xe8b6, "Material Icons");
        public static readonly IconData keyboard_arrow_down = new IconData(0xe313, "Material Icons");
        public static readonly IconData more_vert = new IconData(0xe5d4, "Material Icons");
        public static readonly IconData close = new IconData(0xe5cd, "Material Icons");
        public static readonly IconData chevron_right = new IconData(0xe5cc, "Material Icons");
        public static readonly IconData arrow_back = new IconData(0xe5c4, "Material Icons");
        public static readonly IconData share = new IconData(0xe80d, "Material Icons");
        public static readonly IconData settings = new IconData(0xe8b8, "Material Icons");
    }

    public static class CTextStyle {
        public static readonly TextStyle Xtra = new TextStyle(
            height: 1.17f,
            fontSize: 48,
            fontFamily: "PingFang-Semibold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H1 = new TextStyle(
            height: 1.2f,
            fontSize: 40,
            fontFamily: "PingFang-Semibold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H2 = new TextStyle(
            height: 1.25f,
            fontSize: 32,
            fontFamily: "PingFang-Semibold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H3 = new TextStyle(
            height: 1.29f,
            fontSize: 28,
            fontFamily: "PingFang-Semibold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H4 = new TextStyle(
            height: 1.33f,
            fontSize: 24,
            fontFamily: "PingFang-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H5 = new TextStyle(
            height: 1.4f,
            fontSize: 20,
            fontFamily: "PingFang-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle PLarge = new TextStyle(
            height: 1.5f,
            fontSize: 16,
            fontFamily: "PingFang-Regular",
            color: CColors.TextParagraph
        );

        public static readonly TextStyle PRegular = new TextStyle(
            height: 1.57f,
            fontSize: 14,
            fontFamily: "PingFang-Regular",
            color: CColors.TextParagraph
        );

        public static readonly TextStyle PSmall = new TextStyle(
            height: 1.67f,
            fontSize: 12,
            fontFamily: "PingFang-Regular",
            color: CColors.TextParagraph
        );

        public static readonly TextStyle Caption = new TextStyle(
            height: 1.67f,
            fontSize: 12,
            letterSpacing: 0.5f,
            fontFamily: "PingFang-Medium",
            color: CColors.TextParagraph
        );
    }

    public static class CColors {
        public static readonly Color TextTitle = new Color(0xFF000000);
        public static readonly Color TextParagraph = new Color(0xFF212121);
        public static readonly Color primary = new Color(0xFFE91E63);
        public static readonly Color secondary1 = new Color(0xFF00BCD4);
        public static readonly Color background1 = new Color(0xFF292929);
        public static readonly Color background2 = new Color(0xFF383838);
        public static readonly Color background3 = new Color(0xFFF5F5F5);
        public static readonly Color background4 = new Color(0xFF00BCD4);
        public static readonly Color warning = new Color(0xFFF0513C);
        public static readonly Color icon1 = new Color(0xFFFFFFFF);
        public static readonly Color icon2 = new Color(0xFFA4A4A4);
        public static readonly Color text1 = new Color(0xFFFFFFFF);
        public static readonly Color text2 = new Color(0xFFD8D8D8);
        public static readonly Color text3 = new Color(0xFF959595);
        public static readonly Color text4 = new Color(0xFF002835);
        public static readonly Color text5 = new Color(0xFF9E9E9E);
        public static readonly Color text6 = new Color(0xFF002835);
        public static readonly Color text7 = new Color(0xFF5A5A5B);
        public static readonly Color text8 = new Color(0xFF239988);
        public static readonly Color text9 = new Color(0xFFB3B5B6);
        public static readonly Color text10 = new Color(0xFF00BCD4);
        public static readonly Color text11 = new Color(0xFF2B98F0);
        public static readonly Color dividingLine1 = new Color(0xFF666666);
        public static readonly Color dividingLine2 = new Color(0xFF404040);
        public static readonly Color redPoint = new Color(0xFFCC122B);
        public static readonly Color mask = new Color(0x66000000);

        public static readonly Color transparent = new Color(0x00000000);
        public static readonly Color white = new Color(0xFFFFFFFF);
        public static readonly Color black = new Color(0xFF000000);
        public static readonly Color red = new Color(0xFFFF0000);
        public static readonly Color green = new Color(0xFF00FF00);
        public static readonly Color blue = new Color(0xFF0000FF);

        public static readonly Color header = new Color(0xFF060B0C);
        
        
        public static readonly Color primaryBlue = new Color(0xFF2196F3);
        public static readonly Color brownGrey = new Color(0xFFB5B5B5);

        
        
        
    }
}