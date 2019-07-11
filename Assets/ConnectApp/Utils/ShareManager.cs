using System;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.screens;
using RSG;
using Unity.UIWidgets.service;

namespace ConnectApp.Utils {
    public static class ShareManager {
        public static void showArticleShareView(
            Article article,
            bool showReportAndBlock,
            bool isLoggedIn,
            Action pushToLogin,
            Action<string> pushToBlock,
            Action<string, ReportType> pushToReport,
            Func<ShareType, string, string, string, string, IPromise> shareToWechat,
            Action mainRouterPop = null
        ) {
            ShareUtils.showShareView(new ShareView(
                projectType: ProjectType.article,
                showReportAndBlock: showReportAndBlock,
                onPressed: type => {
                    string linkUrl = $"{Config.apiAddress}/p/{article.id}";
                    if (type == ShareType.clipBoard) {
                        Clipboard.setData(new ClipboardData(text: linkUrl));
                        CustomDialogUtils.showToast("复制链接成功", Icons.check_circle_outline);
                    }
                    else if (type == ShareType.block) {
                        ReportManager.block(
                            isLoggedIn: isLoggedIn,
                            reportId: article.id,
                            pushToLogin: pushToLogin,
                            pushToBlock: pushToBlock,
                            mainRouterPop: mainRouterPop
                        );
                    }
                    else if (type == ShareType.report) {
                        ReportManager.report(
                            isLoggedIn: isLoggedIn,
                            reportId: article.id,
                            reportType: ReportType.article,
                            pushToLogin: pushToLogin,
                            pushToReport: pushToReport
                        );
                    }
                    else {
                        CustomDialogUtils.showCustomDialog(
                            child: new CustomLoadingDialog()
                        );
                        string imageUrl = $"{article.thumbnail.url}.200x0x1.jpg";
                        shareToWechat(arg1: type, arg2: article.title, arg3: article.subTitle, arg4: linkUrl, arg5: imageUrl)
                            .Then(onResolved: CustomDialogUtils.hiddenCustomDialog)
                            .Catch(_ => CustomDialogUtils.hiddenCustomDialog());
                    }
                }
            ));
        }
    }
}