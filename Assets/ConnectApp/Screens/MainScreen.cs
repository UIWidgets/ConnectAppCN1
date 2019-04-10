using System;
using System.Collections.Generic;
using ConnectApp.canvas;
using ConnectApp.components;
using ConnectApp.constants;
using ConnectApp.models;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using RSG;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    
    public class MainScreenConnector : StatelessWidget {
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, bool>(
                converter: state => state.loginState.isLoggedIn,
                builder: (context1, isLoggedIn, dispatcher) => {
                    return new MainScreen(isLoggedIn, () =>
                    {
                        dispatcher.dispatch(new MainNavigatorPushToAction
                        {
                            routeName = MainNavigatorRoutes.Login
                        });
                    });
                }
            );
        }
    }
    
    public class MainScreen : StatelessWidget {

        public MainScreen(
            bool isLoggedIn,
            Action pushToLogin = null
        )
        {
            this.isLoggedIn = isLoggedIn;
            this.pushToLogin = pushToLogin;
        }

        private readonly bool isLoggedIn;
        private readonly Action pushToLogin;
        
        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new SafeArea(
                    top: false,
                    child: new CustomTabBar(
                        new List<Widget> {
                            new ArticlesScreenConnector(),
                            new EventsScreenConnector(),
                            new NotificationScreenConnector(),
                            new PersonalScreenConnector()
                        },
                        new List<CustomTabBarItem> {
                            new CustomTabBarItem(
                                0,
                                Icons.Description,
                                "文章"
                            ),
                            new CustomTabBarItem(
                                1,
                                Icons.ievent,
                                "活动"
                            ),
                            new CustomTabBarItem(
                                2,
                                Icons.Notification,
                                "通知"
                            ),
                            new CustomTabBarItem(
                                3,
                                Icons.Mood,
                                "我的"
                            ),
                        },
                        CColors.White,
                        index => {
                            if (index == 2) {
                                if (!isLoggedIn)
                                {
                                    pushToLogin();
                                    return false;
                                }
                            }

                            return true;
                        }
                    )
                )
            );
        }
    }
}