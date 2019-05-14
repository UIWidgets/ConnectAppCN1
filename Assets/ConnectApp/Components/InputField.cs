using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.material;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using Icons = ConnectApp.constants.Icons;

namespace ConnectApp.components {
    public enum InputFieldClearButtonMode {
        never,
        hasText,
        whileEditing,
        always
    }

    public class InputField : StatefulWidget {
        public InputField(
            Key key = null,
            TextEditingController controller = null,
            FocusNode focusNode = null,
            bool obscureText = false,
            bool autocorrect = true,
            bool enabled = true,
            TextStyle style = null,
            TextAlign textAlign = TextAlign.left,
            int maxLines = 1,
            bool autofocus = false,
            string hintText = null,
            TextStyle hintStyle = null,
            string labelText = null,
            TextStyle labelStyle = null,
            Color cursorColor = null,
            TextInputAction textInputAction = TextInputAction.none,
            TextInputType keyboardType = null,
            float height = 44.0f,
            InputFieldClearButtonMode clearButtonMode = InputFieldClearButtonMode.never,
            bool enableInteractiveSelection = true,
            Color selectionColor = null,
            ValueChanged<string> onChanged = null,
            ValueChanged<string> onSubmitted = null,
            EdgeInsets scrollPadding = null
        ) : base(key) {
            this.controller = controller;
            this.textAlign = textAlign;
            this.focusNode = focusNode;
            this.obscureText = obscureText;
            this.autocorrect = autocorrect;
            this.enabled = enabled;
            this.style = style;
            this.textAlign = textAlign;
            this.maxLines = maxLines;
            this.autofocus = autofocus;
            this.hintText = hintText;
            this.hintStyle = hintStyle;
            this.labelText = labelText;
            this.labelStyle = labelStyle;
            this.height = height;
            this.clearButtonMode = clearButtonMode;
            this.enableInteractiveSelection = enableInteractiveSelection;
            this.selectionColor = selectionColor ?? CColors.PrimaryBlue;
            this.cursorColor = cursorColor;
            this.textInputAction = textInputAction;
            this.keyboardType = keyboardType;
            this.onChanged = onChanged;
            this.onSubmitted = onSubmitted;
            this.scrollPadding = scrollPadding;
        }

        public readonly TextEditingController controller;
        public readonly FocusNode focusNode;
        public readonly bool obscureText;
        public readonly bool autocorrect;
        public readonly bool enabled;
        public readonly TextStyle style;
        public readonly TextAlign textAlign;
        public readonly int maxLines;
        public readonly bool autofocus;
        public readonly string hintText;
        public readonly TextStyle hintStyle;
        public readonly string labelText;
        public readonly TextStyle labelStyle;
        public readonly Color cursorColor;
        public readonly TextInputAction textInputAction;
        public readonly TextInputType keyboardType;
        public readonly float height;
        public readonly InputFieldClearButtonMode clearButtonMode;
        public readonly bool enableInteractiveSelection;
        public readonly Color selectionColor;
        public readonly ValueChanged<string> onChanged;
        public readonly ValueChanged<string> onSubmitted;

        public readonly EdgeInsets scrollPadding;

        public override State createState() {
            return new _InputFieldState();
        }
    }

    class _InputFieldState : State<InputField> {
        TextEditingController _textEditingController;
        FocusNode _focusNode;
        bool _isHintTextHidden;
        bool _isFocus;

        public override void initState() {
            base.initState();
            this._textEditingController =
                this.widget.controller != null ? this.widget.controller : new TextEditingController("");
            this._focusNode = this.widget.focusNode != null ? this.widget.focusNode : new FocusNode();
            this._isHintTextHidden = false;
            this._isFocus = this.widget.autofocus;
            this._textEditingController.addListener(this._controllerListener);
            this._focusNode.addListener(this._focusNodeListener);
        }

        public override void dispose() {
            this._textEditingController.removeListener(this._controllerListener);
            this._focusNode.removeListener(this._focusNodeListener);
            base.dispose();
        }

        void _controllerListener() {
            var isTextEmpty = this._textEditingController.text.Length > 0;
            if (this._isHintTextHidden != isTextEmpty) {
                this.setState(() => { this._isHintTextHidden = isTextEmpty; });
            }
        }

        void _focusNodeListener() {
            if (this._isFocus != this._focusNode.hasFocus) {
                this.setState(() => { this._isFocus = this._focusNode.hasFocus; });
            }
        }

        public override Widget build(BuildContext context) {
            return new IgnorePointer(
                ignoring: !this.widget.enabled,
                child: new Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: new List<Widget> {
                        this._buildLabelText(),
                        new Stack(
                            children: new List<Widget> {
                                this._buildEditableText(context), this._buildHintText()
                            }
                        )
                    }
                )
            );
        }

        Widget _buildLabelText() {
            if (this.widget.labelText == null) {
                return new Container();
            }

            if (!this._isHintTextHidden) {
                return new Container(height: 20);
            }

            return new Container(
                height: 20,
                alignment: Alignment.bottomLeft,
                child: new Text(this.widget.labelText,
                    style: this.widget.labelStyle
                )
            );
        }

        Widget _buildHintText() {
            if (this.widget.hintText == null || this._isHintTextHidden) {
                return new Container();
            }

            return new Positioned(
                top: 0,
                left: 0,
                bottom: 0,
                child: new IgnorePointer(
                    ignoring: true,
                    child: new Container(
                        alignment: Alignment.center,
                        child: new Text(this.widget.hintText, style: this.widget.hintStyle)
                    )
                )
            );
        }

        Widget _buildEditableText(BuildContext context) {
            Widget clearButton = new Container();
            if (this.widget.clearButtonMode == InputFieldClearButtonMode.always) {
                clearButton = this._buildClearButton(context);
            }
            else if (this.widget.clearButtonMode == InputFieldClearButtonMode.hasText) {
                if (this._isHintTextHidden) {
                    clearButton = this._buildClearButton(context);
                }
            }
            else if (this.widget.clearButtonMode == InputFieldClearButtonMode.whileEditing) {
                if (this._isHintTextHidden && this._isFocus) {
                    clearButton = this._buildClearButton(context);
                }
            }

            return new GestureDetector(
                onTap: () => {
                    var focusNode = this.widget.focusNode ?? this._focusNode;
                    FocusScope.of(context).requestFocus(focusNode);
                },
                child: new Container(
                    height: this.widget.height,
                    alignment: Alignment.center,
                    color: CColors.Transparent,
                    child: new Row(
                        children: new List<Widget> {
                            new Expanded(
                                child: new EditableText(
                                    maxLines: this.widget.maxLines,
                                    controller: this._textEditingController,
                                    focusNode: this._focusNode,
                                    autofocus: this.widget.autofocus,
                                    obscureText: this.widget.obscureText,
                                    style: this.widget.style,
                                    cursorColor: this.widget.cursorColor,
                                    autocorrect: this.widget.autocorrect,
                                    textInputAction: this.widget.textInputAction,
                                    keyboardType: this.widget.keyboardType,
                                    textAlign: this.widget.textAlign,
                                    scrollPadding: this.widget.scrollPadding,
                                    selectionControls: this.widget.enableInteractiveSelection
                                        ? MaterialUtils.materialTextSelectionControls
                                        : null,
                                    enableInteractiveSelection: this.widget.enableInteractiveSelection,
                                    selectionColor: this.widget.selectionColor,
                                    onChanged: text => {
                                        var isTextEmpty = text.Length > 0;
                                        if (this._isHintTextHidden != isTextEmpty) {
                                            this.setState(() => { this._isHintTextHidden = isTextEmpty; });
                                        }

                                        if (this.widget.onChanged != null) {
                                            this.widget.onChanged(text);
                                        }
                                    },
                                    onSubmitted: this.widget.onSubmitted
                                )
                            ),
                            clearButton
                        }
                    )
                )
            );
        }

        Widget _buildClearButton(BuildContext context) {
            return new CustomButton(
                onPressed: () => {
                    this._textEditingController.clear();
                    this.widget.onChanged("");
                    FocusScope.of(context).requestFocus(this._focusNode);
                },
                child: new Container(
                    child: new Icon(
                        Icons.cancel,
                        size: 20,
                        color: new Color(0xFFCCCCCC)
                    )
                )
            );
        }
    }
}