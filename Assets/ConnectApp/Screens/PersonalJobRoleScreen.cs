using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using Newtonsoft.Json;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.screens {
    public class PersonalJobRoleScreenConnector : StatelessWidget {
        public PersonalJobRoleScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EditPersonalInfoScreenViewModel>(
                converter: state => {
                    var currentUserId = state.loginState.loginInfo.userId ?? "";
                    var user = state.userState.userDict.ContainsKey(key: currentUserId)
                        ? state.userState.userDict[key: currentUserId]
                        : new User();
                    return new EditPersonalInfoScreenViewModel {
                        user = user,
                        jobRole = state.userState.jobRole
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new PersonalJobRoleScreen(
                        viewModel: viewModel,
                        () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        jobRole => {
                            dispatcher.dispatch(new ChangePersonalRoleAction {jobRole = jobRole});
                            dispatcher.dispatch(new MainNavigatorPopAction());
                        }
                    );
                }
            );
        }
    }

    public class PersonalJobRoleScreen : StatelessWidget {
        public PersonalJobRoleScreen(
            EditPersonalInfoScreenViewModel viewModel = null,
            Action mainRouterPop = null,
            Action<JobRole> changeJobRole = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.mainRouterPop = mainRouterPop;
            this.changeJobRole = changeJobRole;
        }

        readonly EditPersonalInfoScreenViewModel viewModel;
        readonly Action mainRouterPop;
        readonly Action<JobRole> changeJobRole;

        static Dictionary<string, string> _jobRole {
            get {
                var jobRole = Resources.Load<TextAsset>("files/JobRole").text;
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(jobRole);
            }
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Column(
                        children: new List<Widget> {
                            this._buildNavigationBar(),
                            new Flexible(
                                child: this._buildContent()
                            )
                        }
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            return new CustomAppBar(
                () => this.mainRouterPop(),
                new Text(
                    "请选择对应的身份",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent() {
            var widgets = new List<Widget> {
                new CustomDivider(
                    color: CColors.White
                )
            };
            var values = this.viewModel.user.jobRoleMap.Values;
            foreach (var jobRole in values) {
                var widget = this._buildRoleItem(jobRole: jobRole);
                widgets.Add(item: widget);
            }

            return new Container(
                color: CColors.Background,
                child: new ListView(
                    children: widgets
                )
            );
        }

        Widget _buildRoleItem(JobRole jobRole) {
            var name = _jobRole.ContainsKey(key: jobRole.name)
                ? _jobRole[key: jobRole.name]
                : jobRole.name;
            var isCheck = this.viewModel.jobRole.id == jobRole.id;
            Widget checkWidget;
            if (isCheck) {
                checkWidget = new Icon(
                    icon: Icons.check,
                    size: 24,
                    color: CColors.PrimaryBlue
                );
            }
            else {
                checkWidget = new Container();
            }

            return new GestureDetector(
                onTap: () => this.changeJobRole(obj: jobRole),
                child: new Container(
                    color: CColors.White,
                    height: 44,
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Row(
                        mainAxisAlignment: MainAxisAlignment.spaceBetween,
                        children: new List<Widget> {
                            new Text(
                                data: name,
                                style: isCheck ? CTextStyle.PLargeBlue : CTextStyle.PLargeBody
                            ),
                            checkWidget
                        }
                    )
                )
            );
        }
    }
}