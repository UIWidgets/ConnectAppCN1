using System.Collections.Generic;
using ConnectApp.Utils;
using Unity.UIWidgets.engine;
using Unity.UIWidgets.external.simplejson;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Plugins {
    public class PickImagePlugin {
        public static void addListener() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsMessageManager.instance.AddChannelMessageDelegate("pickImage", del: _handleMethodCall);
        }

        public static void removeListener() {
            if (Application.isEditor) {
                return;
            }

            UIWidgetsMessageManager.instance.RemoveChannelMessageDelegate("pickImage", del: _handleMethodCall);
        }

        static void _handleMethodCall(string method, List<JSONNode> args) {
            if (GlobalContext.context != null) {
                using (WindowProvider.of(context: GlobalContext.context).getScope()) {
                    switch (method) {
                        case "success": {
                            var node = args[0];
                            var dict = JSON.Parse(aJSON: node);
                            var image = (string) dict["image"];
                            if (image != null) {
                                removeListener();
                                EventBus.publish(sName: EventBusConstant.pickAvatarSuccess, new List<object> {image});
                            }

                            break;
                        }
                        case "cancel": {
                            removeListener();
                            break;
                        }
                    }
                }
            }
        }
    }
}