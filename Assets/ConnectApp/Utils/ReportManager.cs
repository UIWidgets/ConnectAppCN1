using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.screens;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class ReportManager {
        public static void showReportView(
            bool isLoggedIn,
            string userName,
            ReportType reportType,
            VoidCallback pushToLogin,
            VoidCallback pushToReport,
            VoidCallback pushToBlock = null,
            VoidCallback blockUserCallback = null
        ) {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    $"屏蔽该用户 {userName}",
                    type: ActionType.destructive,
                    () => blockUser(
                        isLoggedIn: isLoggedIn,
                        userName: userName,
                        pushToLogin: pushToLogin,
                        blockUserAction: blockUserCallback
                    )
                ),
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
                items.Insert(0, 
                    new ActionSheetItem(
                    "屏蔽",
                        type: ActionType.normal,
                        () => blockProject(
                            isLoggedIn: isLoggedIn,
                            pushToLogin: pushToLogin,
                            pushToBlock: pushToBlock
                        )
                    )
                );
            }

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                items: items
            ));
        }
        
        public static void showBlockUserView(
            bool isLoggedIn,
            bool hasBeenBlocked,
            VoidCallback pushToLogin,
            VoidCallback blockUserCallback = null,
            VoidCallback cancelBlockUserCallback = null
        ) {
            var items = new List<ActionSheetItem> {
                new ActionSheetItem(
                    hasBeenBlocked ? "取消屏蔽该用户" : "屏蔽该用户",
                    type: ActionType.destructive,
                    () => {
                        if (hasBeenBlocked) {
                            cancelBlockUser(
                                isLoggedIn: isLoggedIn,
                                pushToLogin: pushToLogin,
                                cancelBlockUserAction: cancelBlockUserCallback
                            );
                        }
                        else {
                            blockUser(
                                isLoggedIn: isLoggedIn,
                                pushToLogin: pushToLogin,
                                blockUserAction: blockUserCallback
                            );
                        }
                    }
                ),
                new ActionSheetItem("取消", type: ActionType.cancel)
            };
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

        public static void blockProject(
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
        
        public static void blockUser(
            bool isLoggedIn,
            VoidCallback pushToLogin,
            VoidCallback blockUserAction,
            VoidCallback mainRouterPop = null,
            string userName = ""
        ) {
            if (!isLoggedIn) {
                pushToLogin();
                return;
            }

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: $"确定屏蔽该用户{userName}吗？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "确定",
                        type: ActionType.destructive,
                        () => {
                            blockUserAction();
                            mainRouterPop?.Invoke();
                        }
                    ),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
        }
        
        public static void cancelBlockUser(
            bool isLoggedIn,
            VoidCallback pushToLogin,
            VoidCallback cancelBlockUserAction,
            VoidCallback mainRouterPop = null
        ) {
            if (!isLoggedIn) {
                pushToLogin();
                return;
            }

            ActionSheetUtils.showModalActionSheet(new ActionSheet(
                title: "确定取消屏蔽该用户吗？",
                items: new List<ActionSheetItem> {
                    new ActionSheetItem(
                        "确定",
                        type: ActionType.destructive,
                        () => {
                            cancelBlockUserAction();
                            mainRouterPop?.Invoke();
                        }
                    ),
                    new ActionSheetItem("取消", type: ActionType.cancel)
                }
            ));
        }
    }
}