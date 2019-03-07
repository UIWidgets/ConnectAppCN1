using ConnectApp.models;

namespace ConnectApp.redux {
    public static class Middleware {
        public static Middleware<AppState> Create() {
            return (store) => (next) => next;
        }
    }
}