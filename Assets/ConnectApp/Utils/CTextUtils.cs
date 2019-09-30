using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class CTextUtils {
        public static float CalculateTextHeight(string text, TextStyle textStyle, float textWidth, int? maxLines) {
            var textPainter = new TextPainter(
                textDirection: TextDirection.ltr,
                text: new TextSpan(
                    text: text,
                    style: textStyle
                ),
                maxLines: maxLines
            );
            textPainter.layout(maxWidth: textWidth);
            return textPainter.height;
        }
    }
}