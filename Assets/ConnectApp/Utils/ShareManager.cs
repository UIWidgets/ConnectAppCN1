using ConnectApp.Components;
using Unity.UIWidgets.ui;

namespace ConnectApp.Utils {
    public static class ShareManager {
        public static void showArticleShareView(
            bool showReportAndBlock,
            bool isLoggedIn,
            VoidCallback pushToCopy,
            VoidCallback pushToLogin,
            VoidCallback pushToBlock,
            VoidCallback pushToReport,
            OnShareType shareToWechat,
            VoidCallback mainRouterPop = null
        ) {
            ActionSheetUtils.showModalActionSheet(new ShareView(
                projectType: ProjectType.article,
                showReportAndBlock: showReportAndBlock,
                onPressed: type => {
                    if (type == ShareType.clipBoard) {
                        pushToCopy?.Invoke();
                    }
                    else if (type == ShareType.block) {
                        ReportManager.blockProject(
                            isLoggedIn: isLoggedIn,
                            pushToLogin: pushToLogin,
                            pushToBlock: pushToBlock,
                            mainRouterPop: mainRouterPop
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(
                            isLoggedIn: isLoggedIn,
                            pushToLogin: pushToLogin,
                            pushToReport: pushToReport
                        );
                    }
                    else {
                        shareToWechat?.Invoke(type: type);
                    }
                }
            ));
        }
    }
}