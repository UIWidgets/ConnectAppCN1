using ConnectApp.components;

namespace ConnectApp.redux.actions
{
    public class ShareAction : BaseAction
    {
        public ShareType type;
        public string title;
        public string description;
        public string linkUrl;
        public string imageUrl;
    }
}