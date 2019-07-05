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
                    Personal personal = state.personalState.personalDict.ContainsKey(currentUserId)
                        ? state.personalState.personalDict[currentUserId] : new Personal();
                    return new EditPersonalInfoScreenViewModel {
                        personal = personal
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    return new PersonalJobRoleScreen(
                        viewModel,
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
            var widgets = new List<Widget>();
            var values = this.viewModel.personal.jobRoleMap.Values;
            foreach (var jobRole in values) {
                var widget = this._buildRoleItem(jobRole: jobRole);
                widgets.Add(item: widget);
            }
            return new Container(
                child: new ListView(
                    padding: EdgeInsets.only(top: 16),
                    children: widgets
                )
            );
        }

        Widget _buildRoleItem(JobRole jobRole) {
            var name = _jobRole.ContainsKey(key: jobRole.name)
                ? _jobRole[key: jobRole.name]
                : jobRole.name;
            return new GestureDetector(
                onTap: () => this.changeJobRole(obj: jobRole),
                child: new Container(
                    color: CColors.White,
                    height: 44,
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Text(
                        data: name,
                        style: CTextStyle.PLargeBody
                    )
                )
            );
        }
    }
}