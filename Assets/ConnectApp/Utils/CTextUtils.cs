using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class CTextUtils {
        public static Size CalculateTextSize(string text, TextStyle textStyle, float textWidth,
            int? maxLines = null) {
            if (text.isEmpty()) {
                return Size.zero;
            }

            var textPainter = new TextPainter(
                textDirection: TextDirection.ltr,
                text: new TextSpan(
                    text: text,
                    style: textStyle
                ),
                maxLines: maxLines
            );
            textPainter.layout(maxWidth: textWidth);
            return textPainter.size;
        }
        
        public static float CalculateTextHeight(string text, TextStyle textStyle, float textWidth,
            int? maxLines = null) {
            return CalculateTextSize(text, textStyle, textWidth, maxLines).height;
        }
        
        public static float CalculateTextWidth(string text, TextStyle textStyle, float textWidth,
            int? maxLines = null) {
            return CalculateTextSize(text, textStyle, textWidth, maxLines).width;
        }
        
        public static TextStyle defaultHeight(this TextStyle style) {
            return style.merge(new TextStyle(height: 1));
        }
    }
}