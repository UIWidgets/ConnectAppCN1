using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.screens;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class ReportManager {
        public static void showReportView(
            bool isLoggedIn,
            ReportType reportType,
            VoidCallback pushToLogin,
            VoidCallback pushToReport,
            VoidCallback pushToBlock = null
        ) {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "举报",
                    type: ActionType.normal,
                    () => report(
                        isLoggedIn: isLoggedIn,
                        pushToLogin: pushToLogin,
                        pushToReport: pushToReport
                    )
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };
            if (reportType == ReportType.article) {
                items.Insert(0, new ActionSheetItem(
                    "屏蔽",
                    type: ActionType.normal,
                    () => block(
                        isLoggedIn: isLoggedIn,
                        pushToLogin: pushToLogin, pushToBlock: pushToBlock
                    )
                ));
            }

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                items: items
            ));
        }

        public static void report(
            bool isLoggedIn,
            VoidCallback pushToLogin,
            VoidCallback pushToReport
        ) {
            if (!isLoggedIn) {
                pushToLogin();
                return;
            }

            pushToReport();
        }

        public static void block(
            bool isLoggedIn,
            VoidCallback pushToLogin,
            VoidCallback pushToBlock,
            VoidCallback mainRouterPop = null
        ) {
            if (!isLoggedIn) {
                pushToLogin();
                return;
            }

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "确定屏蔽当前内容吗？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "确定",
                        type: ActionType.destructive,
                        () => {
                            pushToBlock();
                            if (mainRouterPop != null) {
                                mainRouterPop();
                            }
                        }
                    ),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
        }
    }
}