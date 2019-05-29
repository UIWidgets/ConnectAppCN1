using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.screens;

namespace ConnectApp.Utils {
    public static class ReportManager {
        public static void showReportView(
            bool isLoggedIn,
            string reportId,
            ReportType reportType,
            Action pushToLogin,
            Action<string, ReportType> pushToReport,
            Action<string> pushToBlock = null
        ) {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    "举报",
                    ActionType.normal,
                    () => report(isLoggedIn, reportId, reportType, pushToLogin, pushToReport)
                ),
                new ActionSheetItem("取消", ActionType.cancel)
            };
            if (reportType == ReportType.article) {
                items.Insert(0, new ActionSheetItem(
                    "屏蔽",
                    ActionType.normal,
                    () => block(isLoggedIn, reportId, pushToLogin, pushToBlock)
                ));
            }

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                items: items
            ));
        }

        public static void report(
            bool isLoggedIn,
            string reportId,
            ReportType reportType,
            Action pushToLogin,
            Action<string, ReportType> pushToReport
        ) {
            if (!isLoggedIn) {
                pushToLogin();
                return;
            }

            pushToReport(reportId, reportType);
        }

        public static void block(
            bool isLoggedIn,
            string reportId,
            Action pushToLogin,
            Action<string> pushToBlock,
            Action mainRouterPop = null
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
                        ActionType.destructive,
                        () => {
                            pushToBlock(reportId);
                            if (mainRouterPop != null) {
                                mainRouterPop();
                            }
                        }
                    ),
                    new ActionSheetItem("取消", ActionType.cancel)
                }
            ));
        }
    }
}