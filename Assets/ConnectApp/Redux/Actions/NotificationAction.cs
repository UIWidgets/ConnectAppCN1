using System.Collections.Generic;
using System.Linq;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class StartFetchNotificationsAction : RequestAction {
    }

    public class FetchNotificationsSuccessAction : BaseAction {
        public int pageTotal;
        public List<Notification> notifications;
        public List<User> mentions;
    }

    public class FetchNotificationsFailureAction : BaseAction {
    }

    public static partial class Actions {
        public static object fetchNotifications(int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return NotificationApi.FetchNotifications(pageNumber)
                    .Then(notificationResponse => {
                        var oldResults = notificationResponse.results;
                        if (oldResults != null && oldResults.Count > 0) {
                            var userMap = notificationResponse.userMap;
                            oldResults.ForEach(item => {
                                var data = item.data;
                                var user = new User {
                                    id = data.userId,
                                    fullName = data.fullname,
                                    avatar = data.avatarWithCDN
                                };
                                if (userMap.ContainsKey(data.userId)) {
                                    userMap[data.userId] = user;
                                }
                                else {
                                    userMap.Add(data.userId, user);
                                }
                            });
                            dispatcher.dispatch(new UserMapAction {
                                userMap = userMap
                            });
                        }

                        var mentions = getState().notificationState.mentions;
                        var notifications = getState().notificationState.notifications;
                        if (pageNumber == 1) {
                            notifications = notificationResponse.results;
                            mentions = notificationResponse.userMap.Values.ToList();
                        }
                        else {
                            if (pageNumber <= notificationResponse.pageTotal) {
                                notifications.AddRange(notificationResponse.results);
                            }

                            foreach (var user in notificationResponse.userMap.Values) {
                                if (!mentions.Contains(user)) {
                                    mentions.Add(user);
                                }
                            }
                        }

                        dispatcher.dispatch(new FetchNotificationsSuccessAction {
                            pageTotal = notificationResponse.pageTotal, notifications = notifications,
                            mentions = mentions
                        });
                    })
                    .Catch(err => { dispatcher.dispatch(new FetchNotificationsFailureAction()); });
            });
        }

        public static object fetchMakeAllSeen() {
            return new ThunkAction<AppState>((dispatcher, getState) => NotificationApi.FetchMakeAllSeen());
        }
    }
}