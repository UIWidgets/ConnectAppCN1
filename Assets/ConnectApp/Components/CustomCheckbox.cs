using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomCheckbox : StatefulWidget{
        public CustomCheckbox(
            bool value,
            ValueChanged<bool> onChanged,
            Color activeColor = null,
            Color inactiveColor = null,
            float size = 12,
            EdgeInsets padding = null,
            Key key = null
        ) : base(key) {
            this.value = value;
            this.onChanged = onChanged;
            this.activeColor = activeColor ?? CColors.PrimaryBlue;
            this.inactiveColor = inactiveColor ?? CColors.White;
            this.size = size;
            this.padding = padding ?? EdgeInsets.all(8);
        }

        public readonly bool value;
        public readonly ValueChanged<bool> onChanged;
        public readonly Color activeColor;
        public readonly Color inactiveColor;
        public readonly float size;
        public readonly EdgeInsets padding;
        
        public override State createState() => new _CustomCheckboxState();
    }

    class _CustomCheckboxState : State<CustomCheckbox> {
        private bool _value;

        public override void initState() {
            base.initState();
            _value = widget.value;
        }
        
        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomCheckbox customCheckbox) {
                if (widget.value != customCheckbox.value) {
                    setState(() => _value = widget.value);
                }
            }
        }

        public override Widget build(BuildContext context) {
            var child = _value ? _buildCheckbox() : _buildUnCheckbox();
            
            return new CustomButton(
                onPressed: () => {
                    if (widget.onChanged != null) {
                        widget.onChanged(!_value);
                    }
                },
                padding: widget.padding,
                child: child
            );
        }

        private Widget _buildCheckbox() {
            return new Container(
                width: widget.size,
                height: widget.size,
                color: widget.activeColor,
                child: new Icon(
                    Icons.check_box,
                    size: widget.size,
                    color: CColors.White
                )
            );
        }
        
        private Widget _buildUnCheckbox() {
            return new Container(
                width: widget.size,
                height: widget.size,
                decoration: new BoxDecoration(
                    widget.inactiveColor,
                    border: Border.all(
                        Color.fromRGBO(199, 203, 207, 1)
                    )
                )
            );
        }
    }
}