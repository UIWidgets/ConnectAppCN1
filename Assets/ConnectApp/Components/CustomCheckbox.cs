using ConnectApp.constants;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.painting;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.components {
    public class CustomCheckbox : StatefulWidget {
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

        public override State createState() {
            return new _CustomCheckboxState();
        }
    }

    class _CustomCheckboxState : State<CustomCheckbox> {
        bool _value;

        public override void initState() {
            base.initState();
            this._value = this.widget.value;
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            base.didUpdateWidget(oldWidget);
            if (oldWidget is CustomCheckbox customCheckbox) {
                if (this.widget.value != customCheckbox.value) {
                    this.setState(() => this._value = this.widget.value);
                }
            }
        }

        public override Widget build(BuildContext context) {
            var child = this._value ? this._buildCheckbox() : this._buildUnCheckbox();

            return new CustomButton(
                onPressed: () => {
                    if (this.widget.onChanged != null) {
                        this.widget.onChanged(!this._value);
                    }
                },
                padding: this.widget.padding,
                child: child
            );
        }

        Widget _buildCheckbox() {
            return new Container(
                width: this.widget.size,
                height: this.widget.size,
                color: this.widget.activeColor,
                child: new Icon(
                    Icons.check_box,
                    size: this.widget.size,
                    color: CColors.White
                )
            );
        }

        Widget _buildUnCheckbox() {
            return new Container(
                width: this.widget.size,
                height: this.widget.size,
                decoration: new BoxDecoration(this.widget.inactiveColor,
                    border: Border.all(
                        Color.fromRGBO(199, 203, 207, 1)
                    )
                )
            );
        }
    }
}