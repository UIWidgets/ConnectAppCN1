using Unity.UIWidgets.foundation;

namespace ConnectApp.components.refresh
{
    public class RefreshController : ValueNotifier<int>
    {
        const int HEAD_START_REFRESH = 4;
        const int HEAD_END_REFRESH = 1;
        const int FOOT_START_REFRESH = 2;
        const int FOOT_END_REFRESH = 3;
        
        public RefreshController(int value) : base(value)
        {
        }
        
        void startHeadRefresh() {
            _ensureListeners();
            value = HEAD_START_REFRESH;
        }

        void _ensureListeners()
        {
            D.assert(
                hasListeners,
                "The RefresherController must attach to a widget first the message is showing probably becourse the method is called before build . ");
        }

        void startFootRefresh() {
            _ensureListeners();
            value = HEAD_END_REFRESH;
        }

        void endHeadRefresh() {
            _ensureListeners();
            value = HEAD_START_REFRESH;
        }

        void endFootRefresh() {
            _ensureListeners();
            value = FOOT_END_REFRESH;
        }
         
    }
}