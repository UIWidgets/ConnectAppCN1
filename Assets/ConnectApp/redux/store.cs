using System;
using System.Linq;

namespace ConnectApp.redux {
    public delegate object Dispatcher(object action);

    public delegate State Reducer<State>(State previousState, object action);

    public delegate Func<Dispatcher, Dispatcher> Middleware<State>(Store<State> store);

    public delegate void StateChangedHandler<State>(State action);

    public class Store<State> {
        public StateChangedHandler<State> stateChanged;
        private State _state;
        private readonly Dispatcher _dispatcher;
        private readonly Reducer<State> _reducer;

        public Store(Reducer<State> reducer, State initialState = default(State),
            params Middleware<State>[] middlewares) {
            _reducer = reducer;
            _dispatcher = ApplyMiddlewares(middlewares);
            _state = initialState;
        }

        public object Dispatch(object action) {
            return _dispatcher(action);
        }

        public State state => _state;

        private Dispatcher ApplyMiddlewares(params Middleware<State>[] middlewares) {
            return middlewares.Reverse().Aggregate<Middleware<State>, Dispatcher>(InnerDispatch,
                (current, middleware) => middleware(this)(current));
        }

        private object InnerDispatch(object action) {
            _state = _reducer(_state, action);

            if (stateChanged != null) stateChanged(_state);

            return action;
        }
    }
}