using System.Collections.Generic;
using System.Linq;
using ConnectApp.Api;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using Unity.UIWidgets.foundation;
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
                return NotificationApi.FetchNotifications(pageNumber: pageNumber)
                    .Then(notificationResponse => {
                        var results = notificationResponse.results;
                        if (results != null && results.Count > 0) {
                            var userMap = notificationResponse.userMap;
                            Dictionary<string, Team> teamMap = new Dictionary<string, Team>();
                            results.ForEach(item => {
                                var data = item.data;
                                if (data.userId.isNotEmpty()) {
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
                                }
                                if (data.teamId.isNotEmpty()) {
                                    var team = new Team {
                                        id = data.teamId,
                                        name = data.teamName,
                                        avatar = data.teamAvatarWithCDN ?? ""
                                    };
                                    if (teamMap.ContainsKey(key: data.teamId)) {
                                        teamMap[key: data.teamId] = team;
                                    }
                                    else {
                                        teamMap.Add(key: data.teamId, value: team);
                                    }
                                }
                            });
                            dispatcher.dispatch(new UserMapAction {userMap = userMap});
                            dispatcher.dispatch(new TeamMapAction {teamMap = teamMap});
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