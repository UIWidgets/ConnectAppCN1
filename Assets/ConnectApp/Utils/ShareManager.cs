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
            ShareUtils.showShareView(new ShareView(
                projectType: ProjectType.article,
                showReportAndBlock: showReportAndBlock,
                onPressed: type => {
                    if (type == ShareType.clipBoard) {
                        if (pushToCopy != null) {
                            pushToCopy();
                        }
                    }
                    else if (type == ShareType.block) {
                        ReportManager.block(
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
                        if (shareToWechat != null) {
                            shareToWechat(type: type);
                        }
                    }
                }
            ));
        }
    }
}