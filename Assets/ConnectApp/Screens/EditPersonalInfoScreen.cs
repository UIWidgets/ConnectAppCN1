using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.Model;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using Newtonsoft.Json;
using RSG;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = ConnectApp.Components.Avatar;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.screens {
    public class EditPersonalInfoScreenConnector : StatelessWidget {
        public EditPersonalInfoScreenConnector(
            string personalId,
            Key key = null
        ) : base(key: key) {
            this.personalId = personalId;
        }

        readonly string personalId;

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, EditPersonalInfoScreenViewModel>(
                converter: state => {
                    var user = state.userState.userDict.ContainsKey(key: this.personalId)
                        ? state.userState.userDict[key: this.personalId]
                        : new User();
                    return new EditPersonalInfoScreenViewModel {
                        personalId = this.personalId,
                        user = user,
                        fullName = state.userState.fullName,
                        title = state.userState.title,
                        jobRole = state.userState.jobRole,
                        place = state.userState.place
                    };
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new EditPersonalInfoScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        editPersonalInfo = (fullName, title, jobRoleId, placeId) =>
                            dispatcher.dispatch<IPromise>(Actions.editPersonalInfo(fullName, title, jobRoleId,
                                placeId)),
                        changeFullName = fullName =>
                            dispatcher.dispatch(new ChangePersonalFullNameAction {fullName = fullName}),
                        changeTitle = title =>
                            dispatcher.dispatch(new ChangePersonalTitleAction {title = title}),
                        changeJobRole = jobRole =>
                            dispatcher.dispatch(new ChangePersonalRoleAction {jobRole = jobRole}),
                        cleanPersonalInfo = () => dispatcher.dispatch(new CleanPersonalInfoAction()),
                        pushToJobRole = () => dispatcher.dispatch(
                            new MainNavigatorPushToAction {routeName = MainNavigatorRoutes.PersonalRole}
                        )
                    };
                    return new EditPersonalInfoScreen(viewModel, actionModel);
                }
            );
        }
    }

    public class EditPersonalInfoScreen : StatefulWidget {
        public EditPersonalInfoScreen(
            EditPersonalInfoScreenViewModel viewModel = null,
            EditPersonalInfoScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly EditPersonalInfoScreenViewModel viewModel;
        public readonly EditPersonalInfoScreenActionModel actionModel;

        public override State createState() {
            return new _EditPersonalInfoScreenState();
        }
    }

    class _EditPersonalInfoScreenState : State<EditPersonalInfoScreen> {
        TextEditingController _fullNameController;
        TextEditingController _titleController;

        readonly FocusNode _fullNameFocusNode = new FocusNode();
        readonly FocusNode _titleFocusNode = new FocusNode();
        Dictionary<string, string> _jobRole;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            var user = this.widget.viewModel.user;
            this._fullNameController = new TextEditingController(text: user.fullName);
            this._titleController = new TextEditingController(text: user.title);

            var jobRole = Resources.Load<TextAsset>("files/JobRole").text;
            this._jobRole = JsonConvert.DeserializeObject<Dictionary<string, string>>(jobRole);

            SchedulerBinding.instance.addPostFrameCallback(_ => {
                if (this.widget.viewModel.fullName.Length > 0
                    || this.widget.viewModel.title.Length > 0
                    || this.widget.viewModel.jobRole != null) {
                    this.widget.actionModel.cleanPersonalInfo();
                }

                this.widget.actionModel.changeFullName(obj: user.fullName);
                this.widget.actionModel.changeTitle(obj: user.title);
                var jobRoleIds = user.jobRoleIds ?? new List<string>();
                var jobRoleMap = user.jobRoleMap ?? new Dictionary<string, JobRole>();
                jobRoleIds.ForEach(jobRoleId => {
                    if (jobRoleMap.ContainsKey(key: jobRoleId)) {
                        var jonRole = jobRoleMap[key: jobRoleId];
                        this.widget.actionModel.changeJobRole(obj: jonRole);
                    }
                });
            });
        }

        void _editPersonalInfo() {
            CustomDialogUtils.showCustomDialog(
                child: new CustomLoadingDialog(
                    message: "保存中"
                )
            );
            this.widget.actionModel.editPersonalInfo(
                this.widget.viewModel.fullName,
                this.widget.viewModel.title,
                this.widget.viewModel.jobRole.id,
                this.widget.viewModel.place
            ).Then(() => {
                CustomDialogUtils.hiddenCustomDialog();
                this.widget.actionModel.mainRouterPop();
            }).Catch(error => CustomDialogUtils.hiddenCustomDialog());
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.Background,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(),
                                new Flexible(
                                    child: this._buildContent()
                                )
                            }
                        )
                    )
                )
            );
        }

        Widget _buildNavigationBar() {
            var disEnabled = this.widget.viewModel.fullName.isEmpty()
                             || this.widget.viewModel.jobRole.name.isEmpty();
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    "编辑个人信息",
                    style: CTextStyle.PXLargeMedium
                ),
                new CustomButton(
                    padding: EdgeInsets.only(16, 0, 16),
                    onPressed: () => {
                        if (!disEnabled) {
                            this._editPersonalInfo();
                        }
                    },
                    child: new Text(
                        "提交",
                        style: disEnabled
                            ? CTextStyle.PLargeMediumBlue.copyWith(new Color(0xFF9D9D9D))
                            : CTextStyle.PLargeMediumBlue
                    )
                )
            );
        }

        Widget _buildContent() {
            return new Container(
                child: new ListView(
                    children: new List<Widget> {
                        // this._buildHeader(),
                        this._buildInputItem(
                            "昵称",
                            "请输入你的昵称",
                            controller: this._fullNameController,
                            focusNode: this._fullNameFocusNode,
                            fullName => this.widget.actionModel.changeFullName(obj: fullName),
                            70
                        ),
                        this._buildInputItem(
                            "头衔",
                            "请输入你的头衔",
                            controller: this._titleController,
                            focusNode: this._titleFocusNode,
                            title => this.widget.actionModel.changeTitle(obj: title),
                            45
                        ),
                        new CustomDivider(
                            color: CColors.BgGrey
                        ),
                        this._buildRoleItem()
                    }
                )
            );
        }

        Widget _buildHeader() {
            var user = this.widget.viewModel.user;
            return new CoverImage(
                user.coverImage,
                246,
                new Column(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: new List<Widget> {
                        Avatar.User(
                            user: user,
                            120
                        )
                    }
                )
            );
        }

        Widget _buildInputItem(
            string tipText,
            string placeHold,
            TextEditingController controller,
            FocusNode focusNode,
            ValueChanged<string> onChanged,
            int? maxLength
        ) {
            return new Container(
                color: CColors.White,
                height: 60,
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            flex: 1,
                            child: new Text(data: tipText, style: CTextStyle.PLargeBody4)
                        ),
                        new Expanded(
                            flex: 3,
                            child: new InputField(
                                height: 60,
                                controller: controller,
                                focusNode: focusNode,
                                maxLength: maxLength,
                                style: CTextStyle.PLargeBody,
                                hintText: placeHold,
                                hintStyle: CTextStyle.PLargeBody4,
                                cursorColor: CColors.PrimaryBlue,
                                clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                onChanged: onChanged
                                // onSubmitted: this._searchFollowing
                            )
                        )
                    }
                )
            );
        }

        Widget _buildRoleItem() {
            var jobRoleName = this.widget.viewModel.jobRole.name ?? "";
            var name = this._jobRole.ContainsKey(key: jobRoleName)
                ? this._jobRole[key: jobRoleName]
                : jobRoleName;
            return new Container(
                color: CColors.White,
                height: 60,
                padding: EdgeInsets.symmetric(horizontal: 16),
                child: new Row(
                    children: new List<Widget> {
                        new Expanded(
                            flex: 1,
                            child: new Text("身份", style: CTextStyle.PLargeBody4)
                        ),
                        new Expanded(
                            flex: 3,
                            child: new GestureDetector(
                                onTap: () => this.widget.actionModel.pushToJobRole(),
                                child: new Container(
                                    color: CColors.Transparent,
                                    child: new Stack(
                                        fit: StackFit.expand,
                                        children: new List<Widget> {
                                            name.isEmpty()
                                                ? new Container(
                                                    alignment: Alignment.centerLeft,
                                                    child: new Text("请选择对应的身份", style: CTextStyle.PLargeBody4)
                                                )
                                                : new Container(
                                                    alignment: Alignment.centerLeft,
                                                    child: new Text(name, style: CTextStyle.PLargeBody)
                                                ),
                                            new Positioned(
                                                top: 0,
                                                right: 0,
                                                bottom: 0,
                                                child: new Icon(
                                                    Icons.chevron_right,
                                                    size: 24,
                                                    color: Color.fromRGBO(199, 203, 207, 1)
                                                )
                                            )
                                        }
                                    )
                                )
                            )
                        )
                    }
                )
            );
        }
    }
}