using System;
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
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.scheduler;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Avatar = UnityEngine.Avatar;
using Icons = ConnectApp.Constants.Icons;

namespace ConnectApp.screens {
    public class FeedbackScreenConnector : StatelessWidget {
        public FeedbackScreenConnector(
            Key key = null
        ) : base(key: key) {
        }
        
        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, FeedbackScreenViewModel>(
                converter: state => new FeedbackScreenViewModel {
                    feedbackType = state.feedbackState.feedbackType
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new FeedbackScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        restoreFeedbackType = type => {
                            dispatcher.dispatch(new ChangeFeedbackTypeAction {type = FeedbackType.Advice});
                        },
                        pushToFeedbackType = () => dispatcher.dispatch(new MainNavigatorPushToAction {routeName = MainNavigatorRoutes.FeedbackType})
                    };
                    return new FeedbackScreen(viewModel: viewModel, actionModel: actionModel);
                }
            );
        }
    }

    public class FeedbackScreen : StatefulWidget {
        public FeedbackScreen(
            FeedbackScreenViewModel viewModel = null,
            FeedbackScreenActionModel actionModel = null,
            Key key = null
        ) : base(key: key) {
            this.viewModel = viewModel;
            this.actionModel = actionModel;
        }

        public readonly FeedbackScreenViewModel viewModel;
        public readonly FeedbackScreenActionModel actionModel;

        public override State createState() {
            return new _FeedbackScreenState();
        }
    }

    class _FeedbackScreenState : State<FeedbackScreen> {
        
        GlobalKey<FormState> _formKey = GlobalKey<FormState>.key();

        readonly FocusNode _contentFocusNode = new FocusNode();
        readonly FocusNode _nameFocusNode = new FocusNode();
        readonly FocusNode _contactFocusNode = new FocusNode();
        
        static readonly TextEditingController _contentController = new TextEditingController();
        static readonly TextEditingController _nameController = new TextEditingController();
        static readonly TextEditingController _contactController = new TextEditingController();

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
//
//            var jobRole = Resources.Load<TextAsset>("files/JobRole").text;
//            this._jobRole = JsonConvert.DeserializeObject<Dictionary<string, string>>(jobRole);
//
//            SchedulerBinding.instance.addPostFrameCallback(_ => {
//                if (this.widget.viewModel.fullName.Length > 0
//                    || this.widget.viewModel.title.Length > 0
//                    || this.widget.viewModel.jobRole != null) {
//                    this.widget.actionModel.cleanPersonalInfo();
//                }
//
//                this.widget.actionModel.changeFullName(obj: user.fullName);
//                this.widget.actionModel.changeTitle(obj: user.title);
//                var jobRoleIds = user.jobRoleIds ?? new List<string>();
//                var jobRoleMap = user.jobRoleMap ?? new Dictionary<string, JobRole>();
//                jobRoleIds.ForEach(jobRoleId => {
//                    if (jobRoleMap.ContainsKey(key: jobRoleId)) {
//                        var jonRole = jobRoleMap[key: jobRoleId];
//                        this.widget.actionModel.changeJobRole(obj: jonRole);
//                    }
//                });
//            });
        }

        public override void dispose() {
            _contentController.dispose();
            _contactController.dispose();
            _nameController.dispose();
            base.dispose();
        }

        void _editPersonalInfo() {
//            CustomDialogUtils.showCustomDialog(
//                child: new CustomLoadingDialog(
//                    message: "保存中"
//                )
//            );
//            this.widget.actionModel.editPersonalInfo(
//                this.widget.viewModel.fullName,
//                this.widget.viewModel.title,
//                this.widget.viewModel.jobRole.id,
//                this.widget.viewModel.place
//            ).Then(() => {
//                CustomDialogUtils.hiddenCustomDialog();
//                this.widget.actionModel.mainRouterPop();
//            }).Catch(error => CustomDialogUtils.hiddenCustomDialog());
        }

        public override Widget build(BuildContext context) {
            return new Container(
                color: CColors.White,
                child: new CustomSafeArea(
                    bottom: false,
                    child: new Container(
                        color: CColors.BgGrey,
                        child: new Column(
                            children: new List<Widget> {
                                this._buildNavigationBar(context),
                                new Flexible(
                                    child: this._buildContent(context)
                                )
                            }
                        )
                    )
                )
            );
        }

        
        Widget _buildNavigationBar(BuildContext context) {
//            var disEnabled = this.widget.viewModel.fullName.isEmpty()
//                             || this.widget.viewModel.jobRole.name.isEmpty();
            return new CustomAppBar(
                () => this.widget.actionModel.mainRouterPop(),
                new Text(
                    "意见反馈",
                    style: CTextStyle.PXLargeMedium
                )
            );
        }

        Widget _buildContent(BuildContext context) {
            return new Container(
                child: new Form(
                    this._formKey,
                    new ListView(
                            children: new List<Widget> {
                                new CustomDivider(
                                    color: CColors.Transparent
                                ),
                                this._buildTypeItem(),
                                new Padding(
                                    padding: EdgeInsets.only(16),
                                    child: new CustomDivider(
                                        color: CColors.Separator2,
                                        height: 1
                                    )
                                ),
                                new EnsureVisibleWhenFocused(
                                    focusNode: this._contentFocusNode,
                                    child: new Container(
                                        height: 216,
                                        color: CColors.White,
                                        padding: EdgeInsets.symmetric(12, 16),
                                        child: this._buildInputArea(context)
                                    )
                                ),
                                new CustomDivider(
                                    color: CColors.Transparent
                                ),
                                new EnsureVisibleWhenFocused(
                                    focusNode: this._nameFocusNode,
                                    child: this._buildInputItem(
                                        "称呼",
                                        "如何称呼您",
                                        controller: _nameController,
                                        focusNode: this._nameFocusNode,
                                        fullName => {},
                                        50
                                    )
                                ),
                                new EnsureVisibleWhenFocused(
                                    focusNode: this._contactFocusNode,
                                    child: this._buildInputItem(
                                        "联系方式",
                                        "请输入您的联系方式",
                                        controller: _contactController,
                                        focusNode: this._contactFocusNode,
                                        title => {},
                                        50
                                    )
                                ),
                                this._buildSubmitButton()
                            }
                        )
                )
            );
        }
        
        Widget _buildInputArea(BuildContext context) {
            return new InputField(
                height: 192,
                controller: _contentController,
                focusNode: this._contentFocusNode,
                maxLength: 500,
                maxLines: null,
                alignment: Alignment.topLeft,
                style: CTextStyle.PLargeBody,
                hintText: "请描述您的建议或问题（500字以内），我们将不断改进",
                hintTextWidth: MediaQuery.of(context).size.width - 16 * 2,
                hintStyle: CTextStyle.PLargeBody4,
                cursorColor: CColors.PrimaryBlue,
                clearButtonMode: InputFieldClearButtonMode.never,
                onChanged: _ => {}
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
                        new Container(
                            width: 78,
                            child: new Text(data: tipText, style: CTextStyle.PLargeBody4)
                        ),
                        new Expanded(
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
                            )
                        )
                    }
                )
            );
        }

        Widget _buildTypeItem() {
            var typeName = this.widget.viewModel.feedbackType.Description;
            return new GestureDetector(
                onTap: () => this.widget.actionModel.pushToFeedbackType(),
                child: new Container(
                    color: CColors.White,
                    height: 60,
                    padding: EdgeInsets.symmetric(horizontal: 16),
                    child: new Row(
                        children: new List<Widget> {
                            new Container(
                                width: 78,
                                child: new Text("类型", style: CTextStyle.PLargeBody4)
                            ),
                            new Expanded(
                                child: new Container(
                                    alignment: Alignment.centerLeft,
                                    child: new Text(data: typeName, style: CTextStyle.PLargeBody)
                                )
                            ),
                            new Container(
                                child: new Icon(
                                    icon: Icons.chevron_right,
                                    size: 24,
                                    color: CColors.Icon
                                )
                            )
                        }
                    )
                )
            );
        }
        
        Widget _buildSubmitButton() {
            Widget right = new Container();
//            if (this.widget.viewModel.loading) {
//                right = new CustomActivityIndicator(
//                    loadingColor: LoadingColor.white,
//                    size: LoadingSize.small
//                );
//            }

            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 32),
                child: new CustomButton(
                    onPressed: () => {
//                        if (this.widget.viewModel.loading) {
//                            return;
//                        }
//
//                        this.widget.actionModel.startReportItem();
//                        this.widget.actionModel.reportItem(this._reportItems[this._selectedIndex]);
                    },
                    padding: EdgeInsets.zero,
                    child: new Container(
                        height: 40,
                        decoration: new BoxDecoration(
//                            this.widget.viewModel.loading
//                                ? CColors.ButtonActive
//                                : 
                                CColors.PrimaryBlue,
                            borderRadius: BorderRadius.all(4)
                        ),
                        child: new Stack(
                            children: new List<Widget> {
                                new Align(
                                    alignment: Alignment.center,
                                    child: new Text(
                                        "提交",
                                        style: CTextStyle.PLargeMediumWhite
                                    )
                                ),
                                new Positioned(
                                    right: 24,
                                    height: 40,
                                    child: right
                                )
                            }
                        )
                    )
                )
            );
        }
    }
}