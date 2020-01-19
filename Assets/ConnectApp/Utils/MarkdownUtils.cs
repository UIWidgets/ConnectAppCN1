using ConnectApp.Constants;
using markdown;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public class MarkdownUtils {
        public static MarkdownStyleSheet defaultStyle() {
            return new MarkdownStyleSheet(
                CTextStyle.PXLargeBlue,
                CTextStyle.PXLarge,
                CTextStyle.PCodeStyle,
                CTextStyle.H4,
                CTextStyle.H5,
                CTextStyle.PXLarge,
                CTextStyle.PXLarge,
                CTextStyle.PXLarge,
                CTextStyle.PXLarge,
                CTextStyle.PXLarge.copyWith(fontStyle: FontStyle.italic),
                CTextStyle.PXLarge.copyWith(fontWeight: FontWeight.bold),
                new TextStyle(decoration: TextDecoration.lineThrough),
                CTextStyle.PXLargeBody4,
                CTextStyle.PXLarge,
                CTextStyle.PXLarge,
                8.0f,
                24.0f,
                CTextStyle.PXLarge,
                new TextStyle(fontWeight: FontWeight.w600),
                CTextStyle.PXLarge,
                TextAlign.center,
                TableBorder.all(color: CColors.Grey, width: 0),
                new FlexColumnWidth(),
                EdgeInsets.fromLTRB(16, 8, 16, 8),
                tableCellsDecoration: new BoxDecoration(color: CColors.Grey),
                blockquotePadding: EdgeInsets.all(16.0f),
                blockquoteDecoration: new BoxDecoration(
                    border: new Border(
                        left: new BorderSide(
                            color: CColors.Separator,
                            8
                        ))
                ),
                codeblockPadding: EdgeInsets.all(30.0f),
                codeblockDecoration: new BoxDecoration(
                    color: Color.fromRGBO(110, 198, 255, 0.12f)
                ),
                horizontalRuleDecoration: new BoxDecoration(
                    border: new Border(
                        top: new BorderSide(width: 5.0f, color: CColors.Grey)
                    )
                ));
        }
    }
}