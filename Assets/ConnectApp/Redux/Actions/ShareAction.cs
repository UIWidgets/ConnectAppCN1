using System;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.Models.State;
using ConnectApp.Plugins;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public static partial class Actions {
        public static object shareToWechat(ShareType type, string title, string description, string linkUrl,
            string imageUrl, string path = "", bool isMiniProgram = false) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return ShareApi.FetchImageBytes(imageUrl)
                    .Then(imageBytes => {
                        var encodeBytes = Convert.ToBase64String(imageBytes);

                        if (type == ShareType.friends) {
                            if (isMiniProgram) {
                                WechatPlugin.instance()
                                    .shareToMiniProgram(title, description, linkUrl, encodeBytes, path);
                            }
                            else {
                                WechatPlugin.instance().shareToFriend(title, description, linkUrl, encodeBytes);
                            }
                        }
                        else if (type == ShareType.moments) {
                            WechatPlugin.instance().shareToTimeline(title, description, linkUrl, encodeBytes);
                        }
                    }).Catch(error => {
                        if (type == ShareType.friends) {
                            if (isMiniProgram) {
                                WechatPlugin.instance().shareToMiniProgram(title, description, linkUrl, null, path);
                            }
                            else {
                                WechatPlugin.instance().shareToFriend(title, description, linkUrl, null);
                            }
                        }
                        else if (type == ShareType.moments) {
                            WechatPlugin.instance().shareToTimeline(title, description, linkUrl, null);
                        }
                    });
            });
        }
    }
}