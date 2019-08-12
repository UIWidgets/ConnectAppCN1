using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RSG;
using Unity.UIWidgets.animation;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.rendering;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;
using UnityEngine;

namespace ConnectApp.Components {

    public class EnsureVisibleWhenFocused : StatefulWidget {
        public EnsureVisibleWhenFocused(
            Widget child,
            FocusNode focusNode,
            Curve curve = null,
            TimeSpan? duration = null,
            Key key = null
        ) : base(key: key) {
            this.child = child;
            this.focusNode = focusNode;
            this.curve = curve ?? Curves.ease;
            this.duration = duration ?? TimeSpan.FromMilliseconds(100);
        }

        public readonly Widget child;
        public readonly FocusNode focusNode;
        public readonly Curve curve;
        public readonly TimeSpan? duration;

        public override State createState() {
            return new _EnsureVisibleWhenFocusedState();
        }
    }

    class _EnsureVisibleWhenFocusedState : State<EnsureVisibleWhenFocused>, WidgetsBindingObserver {
        public override void initState() {
            base.initState();
            this.widget.focusNode.addListener(() => this._ensureVisible());
            WidgetsBinding.instance.addObserver(this);
        }

        public override Widget build(BuildContext context) {
            return this.widget.child;
        }

        public override void dispose() {
            WidgetsBinding.instance.removeObserver(this);
            this.widget.focusNode.removeListener(() => this._ensureVisible());
            base.dispose();
        }
        
        async Task _keyboardToggled() {
            Debug.Log(">>>> _keyboardToggled start");
            if (this.mounted) {
                var edgeInsets = MediaQuery.of(this.context).viewInsets;
                while (this.mounted && MediaQuery.of(this.context).viewInsets == edgeInsets) {
                    await Task.Run(() => Promise.Delayed(TimeSpan.FromMilliseconds(10)));
                }
                Debug.Log(">>>> _keyboardToggled");
            }
            Debug.Log(">>>> _keyboardToggled end");
        }

        async Task _ensureVisible() {
            await Task.Run(() => Promise.Delayed(TimeSpan.FromMilliseconds(300)).Then(() => this._keyboardToggled()));
            if (!this.widget.focusNode.hasFocus) {
                return;
            }
            Debug.Log(">>>> _ensureVisible");

            var renderObject = this.context.findRenderObject();
            var viewport = RenderViewportUtils.of(renderObject);
            Debug.Assert(viewport != null);

            var scrollableState = Scrollable.of(context: this.context);
            Debug.Assert(scrollableState != null);

            var position = scrollableState.position;
            float alignment;

            Debug.Log($">>>> alignment = 0, pixels {position.pixels} offset: {viewport.getOffsetToReveal(renderObject, 0).offset}");

            if (position.pixels > viewport.getOffsetToReveal(renderObject, 0).offset) {
                Debug.Log(">>>> _ensureVisible alignment 0");
                alignment = 0;
            }
            else if (position.pixels < viewport.getOffsetToReveal(renderObject, 1).offset) {
                Debug.Log(">>>> _ensureVisible alignment 1");
                alignment = 1;
            }
            else {
                Debug.Log(">>>> _ensureVisible alignment other");
                return;
            }
            
            Debug.Log(">>>> _ensureVisible end");

            position.ensureVisible(
                renderObject: renderObject,
                alignment: alignment,
                duration: this.widget.duration,
                curve: this.widget.curve
            );
        }

        public void didChangeMetrics() {
            if (this.widget.focusNode.hasFocus) {
                this._ensureVisible();
            }
        }

        public void didChangeTextScaleFactor() {
        }

        public void didChangePlatformBrightness() {
        }

        public void didChangeLocales(List<Locale> locale) {
        }

        public IPromise<bool> didPopRoute() {
            return Promise<bool>.Resolved(false);
        }

        public IPromise<bool> didPushRoute(string route) {
            return Promise<bool>.Resolved(false);
        }
    }
}