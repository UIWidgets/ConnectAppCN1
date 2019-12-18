using System;
using System.Collections.Generic;
using ConnectApp.Models.Model;
using ConnectApp.redux;
using ConnectApp.redux.actions;
using Unity.UIWidgets.async;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using UnityEngine;

namespace ConnectApp.Utils {
    public static class MyReactionsManager {
        static readonly Dictionary<string, Dictionary<string, int>> myReactionsDict = new Dictionary<string, Dictionary<string, int>>();
        static readonly Dictionary<string, Timer> timerDict = new Dictionary<string, Timer>();
        
        public static void initialMyReactions(string messageId, Dictionary<string, Dictionary<string, int>> allUserReactionsDict) {
            if (myReactionsDict.ContainsKey(key: messageId)) {
                return;
            }
            var myUserId = UserInfoManager.getUserInfo().userId;
            if (allUserReactionsDict.ContainsKey(key: myUserId)) {
                myReactionsDict[key: messageId] = new Dictionary<string, int>(allUserReactionsDict[myUserId]);
            }
        }
        
        public static void updateMyReaction(ChannelMessageView message, string type) {
            if (!myReactionsDict.ContainsKey(key: message.id)) {
                myReactionsDict.Add(key: message.id, new Dictionary<string, int>());
            }
            if (!myReactionsDict[key: message.id].ContainsKey(key: type)) {
                myReactionsDict[key: message.id].Add(key: type, 0);
            }
            myReactionsDict[key: message.id][key: type] = 1 - myReactionsDict[key: message.id][key: type];

            timerDict.getOrDefault(message.id + type, null)?.cancel();
            timerDict[message.id + type] = Window.instance.run(TimeSpan.FromMilliseconds(1000), () => {
                updateRemoteReaction(message, type);
                timerDict.Remove(message.id + type);
            });
        }

        public static Dictionary<string, int> getMessageReactions(string messageId) {
            return !myReactionsDict.ContainsKey(key: messageId) ? null : myReactionsDict[key: messageId];
        }

        public static void clearData() {
            myReactionsDict.Clear();
        }

        static void updateRemoteReaction(ChannelMessageView message, string type) {
            var local = getMessageReactions(message.id)?.getOrDefault(type, 0) ?? 0;
            var remote = message.getUserReactionsDict(UserInfoManager.getUserInfo().userId)?.getOrDefault(type, 0) ?? 0;
            if (local > 0 && remote == 0) {
                StoreProvider.store.dispatcher.dispatch(Actions.addReaction(messageId: message.id, likeEmoji: type));
            }
            else if (local == 0 && remote > 0) {
                StoreProvider.store.dispatcher.dispatch(Actions.cancelReaction(messageId: message.id, type: type));
            }
        }
    }
}