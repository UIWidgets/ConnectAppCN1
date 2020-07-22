using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.Constants {
    public static class CConstant {
        public const float TabBarHeight = 49;
    }

    public static class Icons {
        public static readonly IconData UnityLogo = new IconData(0xe61d, "iconfont");
        public static readonly IconData UnityTabIcon = new IconData(0xe623, "iconfont");
        public static readonly IconData LogoWithUnity = new IconData(0xe622, "iconfont");
        public static readonly IconData WechatMoment = new IconData(0xe61e, "iconfont");
        public static readonly IconData WechatIcon = new IconData(0xe61f, "iconfont");
        public static readonly IconData outline_delete_keyboard = new IconData(0xe626, "iconfont");
        public static readonly IconData eventIcon = new IconData(0xe60b, "iconfont");
        public static readonly IconData outline_event = new IconData(0xe614, "iconfont");
        public static readonly IconData mood = new IconData(0xe60f, "iconfont");
        public static readonly IconData close = new IconData(0xe605, "iconfont");
        public static readonly IconData arrow_back = new IconData(0xe600, "iconfont");
        public static readonly IconData share = new IconData(0xe61a, "iconfont");
        public static readonly IconData settings = new IconData(0xe61c, "iconfont");
        public static readonly IconData ellipsis = new IconData(0xe60c, "iconfont");
        public static readonly IconData book = new IconData(0xe613, "iconfont");
        public static readonly IconData eye = new IconData(0xe61b, "iconfont");
        public static readonly IconData bookmark = new IconData(0xe603, "iconfont");
        public static readonly IconData favorite = new IconData(0xe60a, "iconfont");
        public static readonly IconData favorite_border = new IconData(0xe610, "iconfont");
        public static readonly IconData comment = new IconData(0xe618, "iconfont");
        public static readonly IconData error_outline = new IconData(0xe609, "iconfont");
        public static readonly IconData sentiment_satisfied = new IconData(0xe617, "iconfont");
        public static readonly IconData sentiment_dissatisfied = new IconData(0xe619, "iconfont");
        public static readonly IconData cancel = new IconData(0xe602, "iconfont");
        public static readonly IconData check_circle_outline = new IconData(0xe606, "iconfont");
        public static readonly IconData insert_link = new IconData(0xe60d, "iconfont");
        public static readonly IconData delete_outline = new IconData(0xe608, "iconfont");
        public static readonly IconData block = new IconData(0xe60e, "iconfont");
        public static readonly IconData report = new IconData(0xe616, "iconfont");
        public static readonly IconData computer = new IconData(0xe625, "iconfont");
        public static readonly IconData thumb_bold = new IconData(0xe627, "iconfont");
        public static readonly IconData thumb_line = new IconData(0xe628, "iconfont");
        public static readonly IconData outline_mail = new IconData(0xe621, "iconfont");
        public static readonly IconData outline_scan = new IconData(0xe624, "iconfont");
        public static readonly IconData outline_notifications = new IconData(0xe629, "iconfont");
        public static readonly IconData outline_search = new IconData(0xe62a, "iconfont");
        public static readonly IconData outline_settings = new IconData(0xe62b, "iconfont");
        public static readonly IconData outline_sentiment_smile = new IconData(0xe62d, "iconfont");
        public static readonly IconData tab_home_line = new IconData(0xe634, "iconfont");
        public static readonly IconData tab_home_fill = new IconData(0xe633, "iconfont");
        public static readonly IconData tab_home_refresh_fill = new IconData(0xe635, "iconfont");
        public static readonly IconData tab_events_line = new IconData(0xe632, "iconfont");
        public static readonly IconData tab_events_fill = new IconData(0xe62f, "iconfont");
        public static readonly IconData tab_messenger_line = new IconData(0xe631, "iconfont");
        public static readonly IconData tab_messenger_fill = new IconData(0xe62c, "iconfont");
        public static readonly IconData tab_mine_line = new IconData(0xe630, "iconfont");
        public static readonly IconData tab_mine_fill = new IconData(0xe62e, "iconfont");
        public static readonly IconData baseline_forward_arrow = new IconData(0xe601, "iconfont");
        public static readonly IconData outline_share = new IconData(0xe636, "iconfont");
        public static readonly IconData outline_carousel = new IconData(0xe637, "iconfont");
        public static readonly IconData outline_list = new IconData(0xe638, "iconfont");
        
        public static readonly IconData outline_keyboard = new IconData(0xeaf9, "Outline Material Icons");
        public static readonly IconData outline_question_answer = new IconData(0xeaa4, "Outline Material Icons");
        public static readonly IconData outline_time = new IconData(0xebed, "Outline Material Icons");
        public static readonly IconData outline_photo_size_select_actual =
            new IconData(0xebca, "Outline Material Icons");

        public static readonly IconData question_answer = new IconData(0xe8af, "Material Icons");
        public static readonly IconData whatshot = new IconData(0xe80e, "Material Icons");
        public static readonly IconData notifications_off = new IconData(0xe7f6, "Material Icons");
        public static readonly IconData chevron_right = new IconData(0xe5cc, "Material Icons");
        public static readonly IconData keyboard_arrow_up = new IconData(0xe316, fontFamily: "Material Icons");
        public static readonly IconData keyboard_arrow_down = new IconData(0xe313, "Material Icons");
        public static readonly IconData bookmark_border = new IconData(0xe867, "Material Icons");
        public static readonly IconData play_arrow = new IconData(0xe037, "Material Icons");
        public static readonly IconData replay = new IconData(0xe042, "Material Icons");
        public static readonly IconData expand_less = new IconData(0xe5ce, "Material Icons");
        public static readonly IconData expand_more = new IconData(0xe5cf, "Material Icons");
        public static readonly IconData error = new IconData(0xe000, fontFamily: "Material Icons");
        public static readonly IconData delete = new IconData(0xe872, "Material Icons");
        public static readonly IconData fullscreen = new IconData(0xe5d0, "Material Icons");
        public static readonly IconData fullscreen_exit = new IconData(0xe5d1, "Material Icons");
        public static readonly IconData pause = new IconData(0xe034, "Material Icons");
        public static readonly IconData check = new IconData(0xe5ca, "Material Icons");
        public static readonly IconData open_in_browser = new IconData(0xe89d, "Material Icons");
        public static readonly IconData camera_alt = new IconData(0xe3b0, "Material Icons");
        public static readonly IconData add = new IconData(0xe145, "Material Icons");
        public static readonly IconData edit = new IconData(0xe3c9, "Material Icons");
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

        public static readonly TextStyle H4White = new TextStyle(
            height: 1.18f,
            fontSize: 24,
            fontFamily: "Roboto-Medium",
            color: CColors.White
        );
        
        public static readonly TextStyle Bold20 = new TextStyle(
            fontSize: 20,
            fontFamily: "Roboto-Bold",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H5 = new TextStyle(
            height: 1.27f,
            fontSize: 20,
            fontFamily: "Roboto-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle H5Body = new TextStyle(
            height: 1.27f,
            fontSize: 20,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody
        );
        
        public static readonly TextStyle H5White = new TextStyle(
            height: 1.27f,
            fontSize: 20,
            fontFamily: "Roboto-Medium",
            color: CColors.White
        );

        public static readonly TextStyle PXLargeMedium = new TextStyle(
            fontSize: 18,
            fontFamily: "Roboto-Medium",
            color: CColors.TextTitle
        );

        public static readonly TextStyle PXLargeMediumWhite = new TextStyle(
            fontSize: 18,
            fontFamily: "Roboto-Medium",
            color: CColors.White
        );

        public static readonly TextStyle PXLargeMediumBody = new TextStyle(
            height: 1.44f,
            fontSize: 18,
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

        public static readonly TextStyle PXLargeWhite = new TextStyle(
            fontSize: 18,
            fontFamily: "Roboto-Regular",
            color: CColors.White
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

        public static readonly TextStyle PLargeBlack = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.Black
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

        public static readonly TextStyle PLargeBody5 = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody5
        );

        public static readonly TextStyle PLargeDisabled = new TextStyle(
            height: 1.33f,
            fontSize: 16,
            fontFamily: "Roboto-Regular",
            color: CColors.Disable2
        );

        public static readonly TextStyle PMediumWhite = new TextStyle(
            fontSize: 14,
            fontFamily: "Roboto-Medium",
            color: CColors.White
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

        public static readonly TextStyle PMediumBody2 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Medium",
            color: CColors.TextBody2
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

        public static readonly TextStyle PRegularBody2 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody2
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

        public static readonly TextStyle PRegularBody5 = new TextStyle(
            height: 1.46f,
            fontSize: 14,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody5
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

        public static readonly TextStyle PSmallBody5 = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            fontFamily: "Roboto-Regular",
            color: CColors.TextBody5
        );

        public static readonly TextStyle PSmallWhite = new TextStyle(
            height: 1.53f,
            fontSize: 12,
            fontFamily: "Roboto-Regular",
            color: CColors.White
        );

        public static readonly TextStyle PRedDot = new TextStyle(
            height: 1.1f,
            fontSize: 12,
            fontFamily: "Roboto-Bold",
            color: CColors.White
        );
    }

    public static class CColors {
        public static readonly Color PrimaryBlue = new Color(0xFF2196F3);
        public static readonly Color SecondaryPink = new Color(0xFFF32194);
        public static readonly Color Error = new Color(0xFFF44336);
        public static readonly Color Thumb = new Color(0xFFF44336);
        public static readonly Color Cancel = new Color(0xFF797979);
        public static readonly Color TextTitle = new Color(0xFF000000);
        public static readonly Color TextBody = new Color(0xFF212121);
        public static readonly Color TextBody2 = new Color(0xFF424242);
        public static readonly Color TextBody3 = new Color(0xFF616161);
        public static readonly Color TextBody4 = new Color(0xFF797979);
        public static readonly Color TextBody5 = new Color(0xFF959595);
        public static readonly Color Separator = new Color(0xFFE6E6E6);
        public static readonly Color Separator2 = new Color(0xFFEEEEEE);
        public static readonly Color BgGrey = new Color(0xFFFAFAFA);
        public static readonly Color BrownGrey = new Color(0xFFB5B5B5);
        public static readonly Color Disable = new Color(0xFFB2B2B2);
        public static readonly Color Disable2 = new Color(0xFFD8D8D8);
        public static readonly Color WeChatGreen = new Color(0xFF48B34F);
        public static readonly Color ButtonActive = new Color(0xFF227ABF);
        public static readonly Color LoadingGrey = new Color(0xFFD8D8D8);
        public static readonly Color LightBlueGrey = new Color(0xFFC7CBCF);
        public static readonly Color ShadyLady = new Color(0xFF9D9D9D);
        public static readonly Color BlackHaze = new Color(0xFFF5F7F8);
        public static readonly Color Grey80 = new Color(0xFFCCCCCC);

        public static readonly Color H5White = Color.fromRGBO(255, 255, 255, 0.8f);
        public static readonly Color TabBarBg = Color.fromRGBO(255, 255, 255, 0.8f);

        public static readonly Color Transparent = new Color(0x00000000);
        public static readonly Color White = new Color(0xFFFFFFFF);
        public static readonly Color Black = new Color(0xFF000000);
        public static readonly Color Red = new Color(0xFFFF0000);
        public static readonly Color Green = new Color(0xFF00FF00);
        public static readonly Color Blue = new Color(0xFF0000FF);
        public static readonly Color Grey = new Color(0xFF9E9E9E);
        public static readonly Color DarkGray = new Color(0x88FFFFFF);

        public static readonly Color Background = new Color(0xFFFAFAFA);
        public static readonly Color Icon = new Color(0xFF979A9E);
        public static readonly Color EmojiBottomBar = new Color(0xFFF4F4F4);

        public static readonly Color Coral = new Color(0xFFFF8686);
        public static readonly Color Orange = new Color(0xFFFFAB6D);
        public static readonly Color Mustard = new Color(0xFFFFDB55);
        public static readonly Color Feijoa = new Color(0xFFADE376);
        public static readonly Color Riptide = new Color(0xFF80E5D7);
        public static readonly Color SkyBlue = new Color(0xFF86D9ED);
        public static readonly Color JordyBlue = new Color(0xFF8DA8F2);
        public static readonly Color Violet = new Color(0xFF9E91F8);
        public static readonly Color Purple = new Color(0xFFC586F3);
        public static readonly Color Comet = new Color(0xFF636672);
        
        public static readonly Color DarkCoral = new Color(0xFFF29393);
        public static readonly Color DarkOrange = new Color(0xFFE5A06D);
        public static readonly Color DarkMustard = new Color(0xFFEFD264);
        public static readonly Color DarkFeijoa = new Color(0xFFADDE7A);
        public static readonly Color DarkRiptide = new Color(0xFF73D7C9);
        public static readonly Color DarkSkyBlue = new Color(0xFF80C3D4);
        public static readonly Color DarkJordyBlue = new Color(0xFF96ACE7);
        public static readonly Color DarkViolet = new Color(0xFF968BE5);
        public static readonly Color DarkPurple = new Color(0xFFC095DF);
        public static readonly Color DarkComet = new Color(0xFF8F92A0);
        
        public static readonly Color AquaMarine = new Color(0xFF1DE9B6);
        public static readonly Color MuteIcon = new Color(0xFFC7CBCF);
        public static readonly Color GreyMessage = new Color(0xFFF0F0F0);
        public static readonly Color BlueMessage = new Color(0xFFC5E8FF);
        public static readonly Color MessageReaction = new Color(0xFFF8F8F8);
        public static readonly Color MessageReactionCount = new Color(0xFF0069C0);
    }
}