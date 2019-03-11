using Unity.UIWidgets.foundation;

namespace ConnectApp.components.refresh {
    public class RefreshController : ValueNotifier<int> {
        private const int HEAD_START_REFRESH = 4;
        private const int HEAD_END_REFRESH = 1;
        private const int FOOT_START_REFRESH = 2;
        private const int FOOT_END_REFRESH = 3;

        public RefreshController(int value) : base(value) {
        }

        private void startHeadRefresh() {
            _ensureListeners();
            value = HEAD_START_REFRESH;
        }

        private void _ensureListeners() {
            D.assert(
                hasListeners,
                "The RefresherController must attach to a widget first the message is showing probably becourse the method is called before build . ");
        }

        private void startFootRefresh() {
            _ensureListeners();
            value = HEAD_END_REFRESH;
        }

        private void endHeadRefresh() {
            _ensureListeners();
            value = HEAD_START_REFRESH;
        }

        private void endFootRefresh() {
            _ensureListeners();
            value = FOOT_END_REFRESH;
        }
    }
}