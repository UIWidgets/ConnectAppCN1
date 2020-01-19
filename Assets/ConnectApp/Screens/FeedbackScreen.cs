using System;
using System.Collections.Generic;
using ConnectApp.Components;
using ConnectApp.Constants;
using ConnectApp.Main;
using ConnectApp.Models.ActionModel;
using ConnectApp.Models.State;
using ConnectApp.Models.ViewModel;
using ConnectApp.redux.actions;
using ConnectApp.Utils;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.Redux;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.screens {
    public class FeedbackScreenConnector : StatelessWidget {
        public FeedbackScreenConnector(
            Key key = null
        ) : base(key: key) {
        }

        public override Widget build(BuildContext context) {
            return new StoreConnector<AppState, FeedbackScreenViewModel>(
                converter: state => new FeedbackScreenViewModel {
                    feedbackType = state.feedbackState.feedbackType,
                    loading = state.feedbackState.loading
                },
                builder: (context1, viewModel, dispatcher) => {
                    var actionModel = new FeedbackScreenActionModel {
                        mainRouterPop = () => dispatcher.dispatch(new MainNavigatorPopAction()),
                        pushToFeedbackType = () => dispatcher.dispatch(new MainNavigatorPushToAction
                            {routeName = MainNavigatorRoutes.FeedbackType}),
                        changeFeedbackType = type => {
                            dispatcher.dispatch(new ChangeFeedbackTypeAction {type = type});
                        },
                        startFeedback = () => dispatcher.dispatch(new StartFeedbackAction()),
                        sendFeedbak = (content, name, contact) =>
                            dispatcher.dispatch<IPromise>(Actions.feedback(viewModel.feedbackType, content, name,
                                contact))
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

    class _FeedbackScreenState : State<FeedbackScreen>, RouteAware {
        FocusNode _contentFocusNode = new FocusNode();
        FocusNode _nameFocusNode = new FocusNode();
        FocusNode _contactFocusNode = new FocusNode();

        readonly TextEditingController _contentController = new TextEditingController();
        readonly TextEditingController _nameController = new TextEditingController();
        readonly TextEditingController _contactController = new TextEditingController();

        ScrollController _scrollController;
        bool _isShowKeyBoard;
        bool _isCompleted;
        GlobalKey _contentFocusNodeKey;
        GlobalKey _nameFocusNodeKey;
        GlobalKey _contactFocusNodeKey;

        public override void initState() {
            base.initState();
            StatusBarManager.statusBarStyle(false);
            this._scrollController = new ScrollController();
            this._contentFocusNodeKey = GlobalKey.key("_contentFocusNodeKey");
            this._nameFocusNodeKey = GlobalKey.key("_nameFocusNodeKey");
            this._contactFocusNodeKey = GlobalKey.key("_contactFocusNodeKey");

            this._contentFocusNode.addListener(this._contentFocusNodeListener);
            this._nameFocusNode.addListener(this._nameFocusNodeListener);
            this._contactFocusNode.addListener(this._contactFocusNodeListener);
        }

        public override void didChangeDependencies() {
            base.didChangeDependencies();
            Router.routeObserve.subscribe(this, (PageRoute) ModalRoute.of(this.context));
        }

        public override void dispose() {
            Router.routeObserve.unsubscribe(this);
            this._contentController.dispose();
            this._contactController.dispose();
            this._nameController.dispose();
            base.dispose();
        }

        float _getPosition(GlobalKey key) {
            var renderBox = (RenderBox) key.currentContext.findRenderObject();
            var position = renderBox.localToGlobal(Offset.zero);
            return position.dy;
        }

        void _contentFocusNodeListener() {
            if (this._contentFocusNode.hasFocus) {
                Promise.Delayed(TimeSpan.FromMilliseconds(300)).Then(() => {
                    this._scrollListView(this._getPosition(this._contentFocusNodeKey));
                });
            }
        }

        void _nameFocusNodeListener() {
            if (this._nameFocusNode.hasFocus) {
                Promise.Delayed(TimeSpan.FromMilliseconds(300)).Then(() => {
                    this._scrollListView(this._getPosition(this._nameFocusNodeKey));
                });
            }
        }

        void _contactFocusNodeListener() {
            if (this._contactFocusNode.hasFocus) {
                Promise.Delayed(TimeSpan.FromMilliseconds(300)).Then(() => {
                    this._scrollListView(this._getPosition(this._contactFocusNodeKey));
                });
            }
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
                                    child: this._buildContent(context)
                                )
                            }
                        )
                    )
                )
            );
        }


        Widget _buildNavigationBar() {
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
                child: new Container(
                    child: new ListView(
                        controller: this._scrollController,
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
                            new Container(
                                height: 216,
                                color: CColors.White,
                                padding: EdgeInsets.symmetric(12, 16),
                                child: this._buildInputArea(context)
                            ),
                            new CustomDivider(
                                color: CColors.Transparent
                            ),
                            this._buildInputItem(
                                "称呼",
                                "如何称呼您",
                                controller: this._nameController,
                                focusNode: this._nameFocusNode,
                                fullName => { },
                                title => { FocusScope.of(context).requestFocus(new FocusNode()); },
                                50,
                                this._nameFocusNodeKey
                            ),
                            this._buildInputItem(
                                "联系方式",
                                "请输入您的联系方式",
                                controller: this._contactController,
                                focusNode: this._contactFocusNode,
                                title => { },
                                title => { FocusScope.of(context).requestFocus(new FocusNode()); },
                                50,
                                this._contactFocusNodeKey
                            ),

                            this._buildSubmitButton(),
                            new Container(
                                height: this._isShowKeyBoard ? MediaQuery.of(context).viewInsets.bottom : 0)
                        }
                    )
                )
            );
        }

        void _scrollListView(float offset) {
            this.setState(() => { this._isShowKeyBoard = true; });
            var bottomHeight = MediaQuery.of(this.context).size.height -
                               offset - MediaQuery.of(this.context).padding.top - 44;
            var paddingBottom = MediaQuery.of(this.context).viewInsets.bottom;
            if (bottomHeight < paddingBottom) {
                var jumpOffset = paddingBottom - bottomHeight;
                this._scrollController.animateTo(jumpOffset, TimeSpan.FromMilliseconds(10), Curves.easeIn);
            }
        }

        Widget _buildInputArea(BuildContext context) {
            return new InputField(
                key: this._contentFocusNodeKey,
                height: 192,
                controller: this._contentController,
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
                onChanged: title => {
                    if (title.isNotEmpty()) {
                        var endTitle = title.Trim();
                        this.setState(() => this._isCompleted = endTitle.isNotEmpty());
                    }
                }
            );
        }

        Widget _buildInputItem(
            string tipText,
            string placeHold,
            TextEditingController controller,
            FocusNode focusNode,
            ValueChanged<string> onChanged,
            ValueChanged<string> onSubmitted,
            int? maxLength,
            Key key
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
                                key: key,
                                height: 60,
                                controller: controller,
                                focusNode: focusNode,
                                maxLength: maxLength,
                                style: CTextStyle.PLargeBody,
                                hintText: placeHold,
                                hintStyle: CTextStyle.PLargeBody4,
                                cursorColor: CColors.PrimaryBlue,
                                clearButtonMode: InputFieldClearButtonMode.whileEditing,
                                onChanged: onChanged,
                                onSubmitted: onSubmitted
                            )
                        )
                    }
                )
            );
        }

        Widget _buildTypeItem() {
            var typeName = this.widget.viewModel.feedbackType.description;
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
            Widget right;
            if (this.widget.viewModel.loading) {
                right = new CustomActivityIndicator(
                    loadingColor: LoadingColor.white,
                    size: LoadingSize.small
                );
            }
            else {
                right = new Container();
            }

            return new Container(
                padding: EdgeInsets.symmetric(horizontal: 16),
                margin: EdgeInsets.only(top: 32),
                child: new CustomButton(
                    onPressed: () => {
                        if (this.widget.viewModel.loading || !this._isCompleted) {
                            return;
                        }

                        this.widget.actionModel.startFeedback();
                        this.widget.actionModel.sendFeedbak(this._contentController.text, this._nameController.text,
                            this._contactController.text
                        );
                    },
                    padding: EdgeInsets.zero,
                    child: new Container(
                        height: 40,
                        decoration: new BoxDecoration(
                            this._isCompleted
                                ? this.widget.viewModel.loading
                                    ? CColors.ButtonActive
                                    : CColors.PrimaryBlue
                                : CColors.Disable,
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

        public void didPopNext() {
        }

        public void didPush() {
        }

        public void didPop() {
            this.widget.actionModel.changeFeedbackType(FeedbackType.Advice);
        }

        public void didPushNext() {
        }
    }
}