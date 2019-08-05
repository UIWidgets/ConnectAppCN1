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
        public int page;
        public int pageNumber;
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
                        var results = notificationResponse.results;
                        if (results != null && results.Count > 0) {
                            var userMap = notificationResponse.userMap;
                            results.ForEach(item => {
                                var data = item.data;
                                var user = new User {
                                    id = data.userId,
                                    fullName = data.fullname,
                                    avatar = data.avatarWithCDN
                                };
                                if (userMap.ContainsKey(key: data.userId)) {
                                    userMap[key: data.userId] = user;
                                }
                                else {
                                    userMap.Add(key: data.userId, value: user);
                                }
                            });
                            dispatcher.dispatch(new UserMapAction {
                                userMap = userMap
                            });
                        }

                        dispatcher.dispatch(new FetchNotificationsSuccessAction {
                            page = notificationResponse.page,
                            pageNumber = pageNumber,
                            pageTotal = notificationResponse.pageTotal,
                            notifications = results,
                            mentions = notificationResponse.userMap.Values.ToList()
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