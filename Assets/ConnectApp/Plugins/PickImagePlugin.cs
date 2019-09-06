using System.Collections.Generic;
using ConnectApp.Api;
using ConnectApp.Components;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.material;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public class PickImagePlugin {
        public static void addListener() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsMessageManager.instance.AddChannelMessageDelegate("pickImage", _handleMethodCall);
        }

        public static void removeListener() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsMessageManager.instance.RemoveChannelMessageDelegate("pickImage", _handleMethodCall);
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (GloableContext.context != null) {
                using (WindowProvider.of(GloableContext.context).getScope()) {
                    switch (method) {
                        case "success": {
                            var node = args[0];
                            var dict = JSON.Parse(node);
                            var image = dict["image"];
                            if (image != null) {
                                removeListener();
                                updateAvatar(image);
                            }
                        }
                            break;
                        case "cancel": {
                            removeListener();
                        }
                            break;
                    }
                }
            }
        }

        static void updateAvatar(string image) {
            CustomDialogUtils.showCustomDialog(child: new CustomLoadingDialog());
            UserApi.UpdateAvatar(image).Then(response => {
                StoreProvider.store.dispatcher.dispatch(new UpdateAvatarSuccessAction {
                    avatar = response.avatar
                });
                CustomDialogUtils.hiddenCustomDialog();
                CustomDialogUtils.showToast("头像更新成功", Icons.sentiment_satisfied);
            }).Catch(exception => {
                CustomDialogUtils.hiddenCustomDialog();
                CustomDialogUtils.showToast("头像更新失败", Icons.sentiment_dissatisfied);
            });
        }
    }
}