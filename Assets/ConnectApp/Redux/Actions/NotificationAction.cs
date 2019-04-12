using System.Collections.Generic;
using ConnectApp.api;
using ConnectApp.models;
using Unity.UIWidgets.Redux;

namespace ConnectApp.redux.actions {
    public class StartFetchNotificationsAction : RequestAction {
    }

    public class FetchNotificationsSuccessAction : BaseAction {
        public int total;
        public List<Notification> notifications;
    }

    public class FetchNotificationsFailureAction : BaseAction {
    }
    
    public class FetchMakeAllSeenAction : RequestAction {
    }

    public static partial class Actions {
        public static object fetchNotifications(int pageNumber) {
            return new ThunkAction<AppState>((dispatcher, getState) => {
                return NotificationApi.FetchNotifications(pageNumber)
                    .Then(notificationResponse => {
                        var oldResults = notificationResponse.results;
                        if (oldResults != null && oldResults.Count > 0)
                            oldResults.ForEach(item => {
                                var data = item.data;
                                var user = new User {
                                    id = data.userId,
                                    fullName = data.fullname
                                };
                                dispatcher.dispatch(new UserMapAction {
                                    userMap = new Dictionary<string, User> {
                                        {data.userId, user}
                                    }
                                });
                            });
                        var notifications = new List<Notification>();
                        if (pageNumber == 1) {
                            notifications = notificationResponse.results;
                        }
                        else {
                            notifications = getState().notificationState.notifications;
                            notifications.AddRange(notificationResponse.results);
                        }

                        dispatcher.dispatch(new FetchNotificationsSuccessAction
                            {total = notificationResponse.total, notifications = notifications});
                    })
                    .Catch(err => { dispatcher.dispatch(new FetchNotificationsFailureAction()); });
            });
        }
        
        public static object fetchMakeAllSeen() {
            return new ThunkAction<AppState>((dispatcher, getState) => NotificationApi.FetchMakeAllSeen());
        }
    }
}