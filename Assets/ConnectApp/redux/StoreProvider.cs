using ConnectApp.models;
using ConnectApp.redux.reducers;
using ConnectApp.redux_logging;

namespace ConnectApp.redux
{
    public static class StoreProvider
    {
        private static Store<AppState> _store;

        public static Store<AppState> store
        {
            get
            {
                if (_store != null) return _store;

                var middleware = new[]
                {
                    ReduxLogging.Create<AppState>(),
                    Middleware.Create()
                };
                _store = new Store<AppState>(
                    AppReducer.Reduce,
                    AppState.initialState(),
                    middleware
                );
                return _store;
            }
        }
    }
}