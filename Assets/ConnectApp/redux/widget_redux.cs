using System;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.ui;
using Unity.UIWidgets.widgets;

namespace ConnectApp.redux {
    public class StoreProvider<State> : InheritedWidget {
        private readonly Store<State> _store;

        public StoreProvider(Store<State> store, Widget child, Key key = null) : base(key: key, child: child) {
            D.assert(store != null);
            D.assert(child != null);
            _store = store;
        }

        public static Store<State> of(BuildContext context) {
            var type = _typeOf<StoreProvider<State>>();
            StoreProvider<State> provider = context.inheritFromWidgetOfExactType(type) as StoreProvider<State>;
            if (provider == null) {
                throw new UIWidgetsError("StoreProvider is missing");
            }

            return provider._store;
        }

        private static Type _typeOf<T>() {
            return typeof(T);
        }

        public override bool updateShouldNotify(InheritedWidget old) {
            return !Equals(_store, ((StoreProvider<State>) old)._store);
        }
    }

    public delegate Widget ViewModelBuilder<ViewModel>(BuildContext context, ViewModel vm);

    public delegate ViewModel StoreConverter<State, ViewModel>(State state, Dispatcher dispatcher);

    public delegate bool ShouldRebuildCallback<ViewModel>(ViewModel pre, ViewModel current);

    public class StoreConnector<State, ViewModel> : StatelessWidget {
        public readonly ViewModelBuilder<ViewModel> builder;

        public readonly StoreConverter<State, ViewModel> converter;

        public readonly ShouldRebuildCallback<ViewModel> shouldRebuild;

        public readonly bool distinct;


        public StoreConnector(ViewModelBuilder<ViewModel> builder, StoreConverter<State, ViewModel> converter,
            bool distinct = false, ShouldRebuildCallback<ViewModel> shouldRebuild = null,
            Key key = null) : base(key) {
            D.assert(builder != null);
            D.assert(converter != null);
            this.distinct = distinct;
            this.builder = builder;
            this.converter = converter;
            this.shouldRebuild = shouldRebuild;
        }

        public override Widget build(BuildContext context) {
            return new _StoreListener<State, ViewModel>(
                store: StoreProvider<State>.of(context),
                builder: builder,
                converter: converter,
                distinct: distinct,
                shouldRebuild: shouldRebuild
            );
        }
    }

    public class _StoreListener<State, ViewModel> : StatefulWidget {
        public readonly ViewModelBuilder<ViewModel> builder;

        public readonly StoreConverter<State, ViewModel> converter;

        public readonly Store<State> store;

        public readonly ShouldRebuildCallback<ViewModel> shouldRebuild;

        public readonly bool distinct;

        public _StoreListener(ViewModelBuilder<ViewModel> builder = null,
            StoreConverter<State, ViewModel> converter = null,
            Store<State> store = null,
            bool distinct = false,
            ShouldRebuildCallback<ViewModel> shouldRebuild = null,
            Key key = null) : base(key) {
            D.assert(builder != null);
            D.assert(converter != null);
            D.assert(store != null);
            this.store = store;
            this.builder = builder;
            this.converter = converter;
            this.distinct = distinct;
            this.shouldRebuild = shouldRebuild;
        }

        public override Unity.UIWidgets.widgets.State createState() {
            return new _StoreListenerState<State, ViewModel>();
        }
    }

    internal class _StoreListenerState<State, ViewModel> : State<_StoreListener<State, ViewModel>> {
        private ViewModel latestValue;

        public override void initState() {
            base.initState();
            _init();
        }

        public override void dispose() {
            widget.store.stateChanged -= _handleStateChanged;
            base.dispose();
        }

        public override void didUpdateWidget(StatefulWidget oldWidget) {
            var oldStore = ((_StoreListener<State, ViewModel>) oldWidget).store;
            if (widget.store != oldStore) {
                oldStore.stateChanged -= _handleStateChanged;
                _init();
            }

            base.didUpdateWidget(oldWidget);
        }

        private void _init() {
            widget.store.stateChanged += _handleStateChanged;
            latestValue = widget.converter(widget.store.state, widget.store.Dispatch);
        }

        private void _handleStateChanged(State state) {
            if (Window.hasInstance) {
                _innerStateChanged(state);
            }
            else {
                using (WindowProvider.of(context).getScope()) {
                    _innerStateChanged(state);
                }
            }
        }

        private void _innerStateChanged(State state) {
            var preValue = latestValue;
            latestValue = widget.converter(widget.store.state, widget.store.Dispatch);
            if (widget.shouldRebuild != null) {
                if (!widget.shouldRebuild(preValue, latestValue)) {
                    return;
                }
            }
            else if (widget.distinct) {
                if (Equals(preValue, latestValue)) {
                    return;
                }
            }

            setState(() => { });
        }

        public override Widget build(BuildContext context) {
            return widget.builder(context, latestValue);
        }
    }
}