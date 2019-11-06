using System.Collections.Generic;
using System.Globalization;
using ConnectApp.Constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.gestures;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.service;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;
using Color = Unity.UIWidgets.ui.Color;

namespace ConnectApp.Components {
    public enum InputFieldClearButtonMode {
        never,
        hasText,
        whileEditing,
        always
    }

    public class CharacterCountTextInputFormatter : TextInputFormatter {
        public CharacterCountTextInputFormatter(int? maxLength) {
            D.assert(maxLength == null || maxLength == -1 || maxLength > 0);
            this.maxLength = maxLength;
        }

        public readonly int? maxLength;

        public override TextEditingValue formatEditUpdate(TextEditingValue oldValue, TextEditingValue newValue) {
            var stringInfo = new StringInfo(value: newValue.text);
            if (this.maxLength != null && this.maxLength > 0 && stringInfo.LengthInTextElements > this.maxLength) {
                if (Input.compositionString.Length > 0) {
                    return newValue;
                }

                var truncated = stringInfo.SubstringByTextElements(0, lengthInTextElements: this.maxLength.Value);

                TextSelection newSelection = newValue.selection.copyWith(
                    baseOffset: Mathf.Min(a: newValue.selection.start, b: truncated.Length),
                    extentOffset: Mathf.Min(a: newValue.selection.end, b: truncated.Length)
                );

                return new TextEditingValue(
                    text: truncated,
                    selection: newSelection,
                    composing: TextRange.empty
                );
            }

            return newValue;
        }
    }

    public class InputField : StatefulWidget {
        public InputField(
            Key key = null,
            TextEditingController controller = null,
            FocusNode focusNode = null,
            bool obscureText = false,
            bool autocorrect = true,
            bool enabled = true,
            Decoration decoration = null,
            TextStyle style = null,
            TextAlign textAlign = TextAlign.left,
            Alignment alignment = null,
            int? maxLines = 1,
            int? minLines = null,
            int? maxLength = null,
            bool maxLengthEnforced = true,
            bool autofocus = false,
            List<TextInputFormatter> inputFormatters = null,
            Widget prefix = null,
            Widget suffix = null,
            string hintText = null,
            float? hintTextWidth = null,
            TextStyle hintStyle = null,
            string labelText = null,
            TextStyle labelStyle = null,
            Color cursorColor = null,
            float cursorWidth = 2,
            Radius cursorRadius = null,
            TextInputAction textInputAction = TextInputAction.none,
            TextInputType keyboardType = null,
            float? height = 44.0f,
            InputFieldClearButtonMode clearButtonMode = InputFieldClearButtonMode.never,
            bool enableInteractiveSelection = true,
            Color selectionColor = null,
            ValueChanged<string> onChanged = null,
            ValueChanged<string> onSubmitted = null,
            EdgeInsets scrollPadding = null
        ) : base(key: key) {
            D.assert(maxLines == null || minLines == null || maxLines >= minLines);
            this.controller = controller;
            this.textAlign = textAlign;
            this.focusNode = focusNode;
            this.obscureText = obscureText;
            this.autocorrect = autocorrect;
            this.enabled = enabled;
            this.decoration = decoration;
            this.style = style;
            this.textAlign = textAlign;
            this.alignment = alignment ?? Alignment.center;
            this.maxLines = maxLines;
            this.minLines = minLines;
            this.maxLength = maxLength;
            this.maxLengthEnforced = maxLengthEnforced;
            this.autofocus = autofocus;
            this.inputFormatters = inputFormatters;
            this.prefix = prefix;
            this.suffix = suffix;
            this.hintText = hintText;
            this.hintTextWidth = hintTextWidth;
            this.hintStyle = hintStyle;
            this.labelText = labelText;
            this.labelStyle = labelStyle;
            this.height = height;
            this.clearButtonMode = clearButtonMode;
            this.enableInteractiveSelection = enableInteractiveSelection;
            this.selectionColor = selectionColor ?? new Color(0x667FAACF);
            this.cursorColor = cursorColor;
            this.cursorWidth = cursorWidth;
            this.cursorRadius = cursorRadius ?? Radius.circular(1.0f);
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
        public readonly Decoration decoration;
        public readonly bool enabled;
        public readonly TextStyle style;
        public readonly TextAlign textAlign;
        public readonly Alignment alignment;
        public readonly int? maxLines;
        public readonly int? minLines;
        public readonly int? maxLength;
        public readonly bool maxLengthEnforced;
        public readonly bool autofocus;
        public readonly List<TextInputFormatter> inputFormatters;
        public readonly Widget prefix;
        public readonly Widget suffix;
        public readonly string hintText;
        public readonly float? hintTextWidth;
        public readonly TextStyle hintStyle;
        public readonly string labelText;
        public readonly TextStyle labelStyle;
        public readonly Color cursorColor;
        public readonly float cursorWidth;
        public readonly Radius cursorRadius;
        public readonly TextInputAction textInputAction;
        public readonly TextInputType keyboardType;
        public readonly float? height;
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

    class _InputFieldState : AutomaticKeepAliveClientMixin<InputField> {
        readonly GlobalKey<EditableTextState> _editableTextKey = new LabeledGlobalKey<EditableTextState>();

        TextEditingController _textEditingController;
        FocusNode _focusNode;
        bool _isHintTextHidden;
        bool _isFocus;

        EditableTextState _editableText {
            get { return this._editableTextKey.currentState; }
        }

        RenderEditable _renderEditable {
            get { return this._editableText.renderEditable; }
        }

        protected override bool wantKeepAlive {
            get { return this._textEditingController?.text?.isNotEmpty() == true; }
        }

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
            if (this.widget.focusNode == null) {
                this._focusNode.dispose();
            }

            base.dispose();
        }

        void _requestKeyboard() {
            this._editableText?.requestKeyboard();
        }

        void _controllerListener() {
            var isTextEmpty = this._textEditingController.text.Length > 0;
            if (this._isHintTextHidden != isTextEmpty) {
                this.setState(() => { this._isHintTextHidden = isTextEmpty; });
            }

            this.updateKeepAlive();
        }

        void _focusNodeListener() {
            if (this._isFocus != this._focusNode.hasFocus) {
                this.setState(() => { this._isFocus = this._focusNode.hasFocus; });
            }
        }

        void _handleTapDown(TapDownDetails details) {
            this._renderEditable.handleTapDown(details);
        }

        void _handleSingleTapUp(TapUpDetails details) {
            this._renderEditable.selectWordEdge(cause: SelectionChangedCause.tap);
            this._requestKeyboard();
        }

        void _handleDoubleTapDown(TapDownDetails details) {
            this._renderEditable.selectWord(cause: SelectionChangedCause.doubleTap);
        }

        void _handleMouseDragSelectionStart(DragStartDetails details) {
            this._renderEditable.selectPositionAt(
                from: details.globalPosition,
                cause: SelectionChangedCause.drag
            );
        }

        void _handleMouseDragSelectionUpdate(
            DragStartDetails startDetails,
            DragUpdateDetails updateDetails
        ) {
            this._renderEditable.selectPositionAt(
                from: startDetails.globalPosition,
                to: updateDetails.globalPosition,
                cause: SelectionChangedCause.drag
            );
        }

        void _handleMouseDragSelectionEnd(DragEndDetails details) {
            this._requestKeyboard();
        }

        void _handleSelectionChanged(TextSelection selection, SelectionChangedCause cause) {
            if (cause == SelectionChangedCause.longPress) {
                this._editableText?.bringIntoView(selection.basePos);
            }
        }

        public override Widget build(BuildContext context) {
            base.build(context);
            return new IgnorePointer(
                ignoring: !this.widget.enabled,
                child: new Container(
                    decoration: this.widget.decoration,
                    child: new Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: new List<Widget> {
                            this._buildLabelText(),
                            this._buildEditableText(context)
                        }
                    )
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
            if (this.widget.hintText == null || this._isHintTextHidden ||
                this._textEditingController.text.isNotEmpty()) {
                return new Container();
            }

            return new Positioned(
                top: 0,
                left: 0,
                bottom: 0,
                child: new IgnorePointer(
                    ignoring: true,
                    child: new Container(
                        width: this.widget.hintTextWidth,
                        alignment: this.widget.alignment,
                        child: new Text(
                            this.widget.hintText,
                            style: this.widget.hintStyle
                        )
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

            List<TextInputFormatter> formatters = this.widget.inputFormatters ?? new List<TextInputFormatter>();
            if (this.widget.maxLength != null && this.widget.maxLengthEnforced) {
                formatters.Add(new CharacterCountTextInputFormatter(maxLength: this.widget.maxLength));
            }

            Widget editableText = new TextSelectionGestureDetector(
                onTapDown: this._handleTapDown,
                onSingleTapUp: this._handleSingleTapUp,
                onDoubleTapDown: this._handleDoubleTapDown,
                onDragSelectionStart: this._handleMouseDragSelectionStart,
                onDragSelectionUpdate: this._handleMouseDragSelectionUpdate,
                onDragSelectionEnd: this._handleMouseDragSelectionEnd,
                behavior: HitTestBehavior.translucent,
                child: new RepaintBoundary(
                    child: new EditableText(
                        key: this._editableTextKey,
                        maxLines: this.widget.maxLines,
                        minLines: this.widget.minLines,
                        controller: this._textEditingController,
                        focusNode: this._focusNode,
                        autofocus: this.widget.autofocus,
                        inputFormatters: formatters,
                        obscureText: this.widget.obscureText,
                        style: this.widget.style,
                        cursorWidth: this.widget.cursorWidth,
                        cursorColor: this.widget.cursorColor,
                        cursorRadius: this.widget.cursorRadius,
                        cursorOffset: new Offset(-2 / MediaQuery.of(context).devicePixelRatio, 0),
                        autocorrect: this.widget.autocorrect,
                        textInputAction: this.widget.textInputAction,
                        keyboardType: this.widget.keyboardType,
                        textAlign: this.widget.textAlign,
                        scrollPadding: this.widget.scrollPadding,
                        selectionControls: this.widget.enableInteractiveSelection
                            ? new CustomTextSelectionControls()
                            : null,
                        enableInteractiveSelection: this.widget.enableInteractiveSelection,
                        selectionColor: this.widget.selectionColor,
                        onSelectionChanged: this._handleSelectionChanged,
                        paintCursorAboveText: true,
                        cursorOpacityAnimates: true,
                        backgroundCursorColor: new Color(0xFF8E8E93),
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
                )
            );

            return new GestureDetector(
                onTap: () => {
                    if (!this._textEditingController.selection.isValid) {
                        this._textEditingController.selection =
                            TextSelection.collapsed(offset: this._textEditingController.text.Length);
                    }

                    this._requestKeyboard();
                    var focusNode = this.widget.focusNode ?? this._focusNode;
                    FocusScope.of(context).requestFocus(focusNode);
                },
                child: new Container(
                    height: this.widget.height,
                    alignment: Alignment.center,
                    color: CColors.Transparent,
                    child: new Row(
                        children: new List<Widget> {
                            this._buildPrefix(),
                            new Expanded(
                                child: new Stack(
                                    fit: this.widget.height == null ? StackFit.loose : StackFit.expand,
                                    children: new List<Widget> {
                                        new Container(
                                            alignment: this.widget.alignment,
                                            child: editableText
                                        ),
                                        this._buildHintText()
                                    }
                                )
                            ),
                            clearButton
                        }
                    )
                )
            );
        }

        Widget _buildPrefix() {
            if (this.widget.prefix == null) {
                return new Container();
            }

            return this.widget.prefix;
        }

        Widget _buildClearButton(BuildContext context) {
            if (this.widget.suffix != null) {
                return this.widget.suffix;
            }

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