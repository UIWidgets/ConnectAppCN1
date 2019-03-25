using System.Collections.Generic;
using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using TextStyle = Unity.UIWidgets.painting.TextStyle;

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
            ValueChanged<string> onChanged = null,
            ValueChanged<string> onSubmitted = null,
            EdgeInsets scrollPadding = null
        ) : base(key) {
            this.controller = controller;
            this.textAlign = textAlign;
            this.focusNode = focusNode;
            this.obscureText = obscureText;
            this.autocorrect = autocorrect;
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
        public readonly ValueChanged<string> onChanged;
        public readonly ValueChanged<string> onSubmitted;

        public readonly EdgeInsets scrollPadding;
//        public readonly Brightness keyboardAppearance;

        public override State createState() {
            return new _InputFieldState();
        }
    }

    internal class _InputFieldState : State<InputField> {
        private TextEditingController _textEditingController;
        private FocusNode _focusNode;
        private bool _isHintTextHidden;
        private bool _isFocus;

        public override void initState() {
            base.initState();
            _textEditingController = widget.controller != null ? widget.controller : new TextEditingController("");
            _focusNode = widget.focusNode != null ? widget.focusNode : new FocusNode();
            _isHintTextHidden = false;
            _isFocus = widget.autofocus;
            _textEditingController.addListener(_controllerListener);
            _focusNode.addListener(_focusNodeListener);
        }

        public override void dispose() {
            _textEditingController.removeListener(_controllerListener);
            _focusNode.removeListener(_focusNodeListener);
            base.dispose();
        }

        private void _controllerListener() {
            var isTextEmpty = _textEditingController.text.Length > 0;
            if (_isHintTextHidden != isTextEmpty)
                setState(() => { _isHintTextHidden = isTextEmpty; });
        }

        private void _focusNodeListener() {
            if (_isFocus != _focusNode.hasFocus)
                setState(() => { _isFocus = _focusNode.hasFocus; });
        }

        public override Widget build(BuildContext context) {
            return new Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: new List<Widget> {
                    _buildLabelText(),
                    new Stack(
                        children: new List<Widget> {
                            _buildEditableText(context), _buildHintText()
                        }
                    )
                }
            );
        }

        private Widget _buildLabelText() {
            if (widget.labelText == null) return new Container();
            if (!_isHintTextHidden) return new Container(height: 20);

            return new Container(
                height: 20,
                alignment: Alignment.bottomLeft,
                child: new Text(widget.labelText,
                    style: widget.labelStyle
                )
            );
        }

        private Widget _buildHintText() {
            if (widget.hintText == null || _isHintTextHidden) return new Container();
            return new Positioned(
                top: 0,
                left: 0,
                bottom: 0,
                child: new GestureDetector(
                    onTap: () => {
                        var focusNode = widget.focusNode ?? _focusNode;
                        FocusScope.of(context).requestFocus(focusNode);
                    },
                    child: new Container(
                        alignment: Alignment.center,
                        child: new Text(widget.hintText,
                            style: widget.hintStyle
                        )
                    )
                )
            );
        }

        private Widget _buildEditableText(BuildContext context) {
            Widget clearButton = new Container();
            if (widget.clearButtonMode == InputFieldClearButtonMode.always) {
                clearButton = _buildClearButton(context);
            }
            else if (widget.clearButtonMode == InputFieldClearButtonMode.hasText) {
                if (_isHintTextHidden) clearButton = _buildClearButton(context);
            }
            else if (widget.clearButtonMode == InputFieldClearButtonMode.whileEditing) {
                if (_isHintTextHidden && _isFocus) clearButton = _buildClearButton(context);
            }

            return new GestureDetector(
                onTap: () => {
                    var focusNode = widget.focusNode ?? _focusNode;
                    FocusScope.of(context).requestFocus(focusNode);
                },
                child: new Container(
                    height: widget.height,
                    alignment: Alignment.center,
                    color: CColors.Transparent,
                    child: new Row(
                        children: new List<Widget> {
                            new Expanded(
                                child: new EditableText(
                                    maxLines: widget.maxLines,
                                    controller: _textEditingController,
                                    focusNode: _focusNode,
                                    autofocus: widget.autofocus,
                                    obscureText: widget.obscureText,
                                    style: widget.style,
                                    cursorColor: widget.cursorColor,
                                    autocorrect: widget.autocorrect,
                                    textInputAction: widget.textInputAction,
                                    keyboardType: widget.keyboardType,
                                    textAlign: widget.textAlign,
                                    scrollPadding: widget.scrollPadding,
                                    onChanged: text => {
                                        var isTextEmpty = text.Length > 0;
                                        if (_isHintTextHidden != isTextEmpty)
                                            setState(() => { _isHintTextHidden = isTextEmpty; });
                                        widget.onChanged(text);
                                    },
                                    onSubmitted: widget.onSubmitted
                                )
                            ),
                            clearButton
                        }
                    )
                )
            );
        }

        private Widget _buildClearButton(BuildContext context) {
            return new CustomButton(
                onPressed: () => {
                    _textEditingController.clear();
                    widget.onChanged("");
                    FocusScope.of(context).requestFocus(_focusNode);
                },
                child: new Container(
                    width: 20,
                    height: 20,
                    decoration: new BoxDecoration(
                        new Color(0xFFCCCCCC),
                        borderRadius: BorderRadius.all(10)
                    ),
                    child: new Icon(
                        Icons.close,
                        size: 20,
                        color: CColors.White
                    )
                )
            );
        }
    }
}