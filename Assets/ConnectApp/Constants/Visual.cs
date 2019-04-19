using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.constants {
    public static class Icons {
        public static readonly IconData UnityLogo = new IconData(0xe61d, "iconfont");
        public static readonly IconData WechatMoment = new IconData(0xe61e, "iconfont");
        public static readonly IconData WechatIcon = new IconData(0xe61f, "iconfont");

        public static readonly IconData Description = new IconData(0xe607, "iconfont");
        public static readonly IconData Notification = new IconData(0xe611, "iconfont");
        public static readonly IconData search = new IconData(0xe612, "iconfont");
        public static readonly IconData close = new IconData(0xe605, "iconfont");
        public static readonly IconData chevron_right = new IconData(0xe5cc, "Material Icons");
        public static readonly IconData arrow_forward = new IconData(0xe601, "iconfont");
        public static readonly IconData arrow_back = new IconData(0xe600, "iconfont");
        public static readonly IconData share = new IconData(0xe61a, "iconfont");
        public static readonly IconData settings = new IconData(0xe61c, "iconfont");
        public static readonly IconData ellipsis = new IconData(0xe60c, "iconfont");
        public static readonly IconData myEvent = new IconData(0xe614, "iconfont");
        public static readonly IconData IEvent = new IconData(0xe60b, "iconfont");
        public static readonly IconData book = new IconData(0xe613, "iconfont");
        public static readonly IconData eye = new IconData(0xe61b, "iconfont");
        public static readonly IconData bookmark = new IconData(0xe603, "iconfont");
        public static readonly IconData favorite = new IconData(0xe60a, "iconfont");
        public static readonly IconData favorite_border = new IconData(0xe610, "iconfont");
        public static readonly IconData comment = new IconData(0xe618, "iconfont");
        public static readonly IconData play_arrow = new IconData(0xe037, "Material Icons");
        public static readonly IconData replay = new IconData(0xe042, "Material Icons");
        public static readonly IconData expand_less = new IconData(0xe5ce, "Material Icons");
        public static readonly IconData expand_more = new IconData(0xe5cf, "Material Icons");
        public static readonly IconData error_outline = new IconData(0xe609, "iconfont");
        public static readonly IconData Mood = new IconData(0xe60f, "iconfont");
        public static readonly IconData sentiment_satisfied = new IconData(0xe617, "iconfont");
        public static readonly IconData sentiment_dissatisfied = new IconData(0xe619, "iconfont");
        public static readonly IconData cancel = new IconData(0xe602, "iconfont");
        public static readonly IconData check_box = new IconData(0xe604, "iconfont");
        public static readonly IconData check_circle_outline = new IconData(0xe606, "iconfont");
        public static readonly IconData insert_link = new IconData(0xe60d, "iconfont");
        public static readonly IconData delete_outline = new IconData(0xe608, "iconfont");
        public static readonly IconData block = new IconData(0xe60e, "iconfont");
        public static readonly IconData report = new IconData(0xe616, "iconfont");
    }

    public static class CTextStyle {
        public static readonly TextStyle Xtra = new TextStyle(
            height: 1.03f,
            fontSize: 48,
            fontFamily: "Roboto-Bold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H1 = new TextStyle(
            height: 1.06f,
            fontSize: 40,
            fontFamily: "Roboto-Bold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H2 = new TextStyle(
            height: 1.11f,
            fontSize: 32,
            fontFamily: "Roboto-Bold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H2White = new TextStyle(
            height: 1.11f,
            fontSize: 32,
            fontFamily: "Roboto-Bold",
            color: CColors.White
        );

        public static readonly TextStyle H2Body4 = new TextStyle(
            height: 1.11f,
            fontSize: 32,
            fontFamily: "Roboto-Bold",
            color: CColors.TextBody4
        );

        public static readonly TextStyle H3 = new TextStyle(
            height: 1.16f,
            fontSize: 28,
            fontFamily: "Roboto-Bold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H4 = new TextStyle(
            height: 1.18f,
            fontSize: 24,
            fontFamily: "Roboto-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H5 = new TextStyle(
            fontSize: 20,
            fontFamily: "Roboto-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H5White = new TextStyle(
            height: 1.27f,
            fontSize: 20,
            fontFamily: "Roboto-Medium",
            color: CColors.H5White
        );

        public static readonly TextStyle H5Body = new TextStyle(
            height: 1.27f,
            fontSize: 20,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody
        );

        public static readonly TextStyle PXLarge = new TextStyle(
            height: 1.68f,
            fontSize: 18,
            fontFamily: "Roboto-Regular",
            letterSpacing: 0.6f,
            color: CColors.TextBody
        );

        public static readonly TextStyle PXLargeBody4 = new TextStyle(
            height: 1.68f,
            fontSize: 18,
            fontFamily: "Roboto-Regular",
            letterSpacing: 0.6f,
            color: CColors.TextBody4
        );

        public static readonly TextStyle PXLargeBlue = new TextStyle(
            height: 1.68f,
            fontSize: 18,
            fontFamily: "Roboto-Regular",
            letterSpacing: 0.6f,
            color: CColors.PrimaryBlue
        );

        public static readonly TextStyle PLargeTitle = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.TextTitle
        );

        public static readonly TextStyle PLargeMedium = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle PLargeBlue = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.PrimaryBlue
        );

        public static readonly TextStyle PLargeMediumBlue = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Medium",
            color: CColors.PrimaryBlue
        );

        public static readonly TextStyle PLargeWhite = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.White
        );

        public static readonly TextStyle PLargeMediumWhite = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Medium",
            color: CColors.White
        );

        public static readonly TextStyle PLargeError = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.Error
        );

        public static readonly TextStyle PLargeBody = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody
        );

        public static readonly TextStyle PLargeBody2 = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody2
        );

        public static readonly TextStyle PLargeBody4 = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody4
        );

        public static readonly TextStyle PLargeDisabled = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.text2
        );

        public static readonly TextStyle PMediumBlue = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Medium",
            color: CColors.PrimaryBlue
        );

        public static readonly TextStyle PMediumBody = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody
        );

        public static readonly TextStyle PMediumBody3 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody3
        );

        public static readonly TextStyle PMediumBody4 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody4
        );

        public static readonly TextStyle PRegularWhite = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.White
        );

        public static readonly TextStyle PRegularError = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.Error
        );

        public static readonly TextStyle PRegularBody = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody
        );

        public static readonly TextStyle PCodeStyle = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Menlo",
            color: CColors.TextBody3
        );

        public static readonly TextStyle PRegularTitle = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextTitle
        );

        public static readonly TextStyle PRegularBody3 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody3
        );

        public static readonly TextStyle PRegularBody4 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody4
        );

        public static readonly TextStyle PKeyboardTextStyle = new TextStyle(
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody3
        );

        public static readonly TextStyle PRegularBlue = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.PrimaryBlue
        );

        public static readonly TextStyle Caption = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            letterSpacing: 0.5f,
            fontFamily: "Roboto-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle CaptionWhite = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            letterSpacing: 0.5f,
            fontFamily: "Roboto-Medium",
            color: CColors.White
        );

        public static readonly TextStyle CaptionBody = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            letterSpacing: 0.5f,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody
        );

        public static readonly TextStyle PSmall = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            fontFamily: "Roboto-Regular",
            color: CColors.TextTitle
        );

        public static readonly TextStyle PSmallBody3 = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody3
        );

        public static readonly TextStyle PSmallBody4 = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody4
        );
    }

    public static class CColors {
        public static readonly Color PrimaryBlue = new Color(0xFF2196F3);
        public static readonly Color SecondaryPink = new Color(0xFFF32194);
        public static readonly Color Error = new Color(0xFFF44336);
        public static readonly Color TextTitle = new Color(0xFF000000);
        public static readonly Color TextBody = new Color(0xFF212121);
        public static readonly Color TextBody2 = new Color(0xFF424242);
        public static readonly Color TextBody3 = new Color(0xFF616161);
        public static readonly Color TextBody4 = new Color(0xFF797979);
        public static readonly Color Separator = new Color(0xFFE6E6E6);
        public static readonly Color Separator2 = new Color(0xFFEEEEEE);
        public static readonly Color BgGrey = new Color(0xFFFAFAFA);
        public static readonly Color BrownGrey = new Color(0xFFB5B5B5);
        public static readonly Color Disable = new Color(0xFFB2B2B2);
        public static readonly Color WechatGreen = new Color(0xFF48B34F);
        public static readonly Color BottonActive = new Color(0xFF227ABF);

        public static readonly Color H5White = Color.fromRGBO(255, 255, 255, 0.8f);

        public static readonly Color Transparent = new Color(0x00000000);
        public static readonly Color White = new Color(0xFFFFFFFF);
        public static readonly Color Black = new Color(0xFF000000);
        public static readonly Color Red = new Color(0xFFFF0000);
        public static readonly Color Green = new Color(0xFF00FF00);
        public static readonly Color Blue = new Color(0xFF0000FF);
        public static readonly Color Grey = new Color(0xFF9E9E9E);

        public static readonly Color secondary1 = new Color(0xFF00BCD4);
        public static readonly Color background1 = new Color(0xFF292929);
        public static readonly Color background2 = new Color(0xFF383838);
        public static readonly Color background3 = new Color(0xFFF5F5F5);
        public static readonly Color background4 = new Color(0xFF00BCD4);
        public static readonly Color icon2 = new Color(0xFFA4A4A4);
        public static readonly Color icon3 = new Color(0xFF979A9E);
        public static readonly Color text1 = new Color(0xFFFFFFFF);
        public static readonly Color text2 = new Color(0xFFD8D8D8);
        public static readonly Color text3 = new Color(0xFF959595);
    }
}