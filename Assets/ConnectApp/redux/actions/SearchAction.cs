namespace ConnectApp.redux.actions {
    public class SearchArticleAction : RequestAction {
        public string keyword;
        public int pageNumber = 0;
    }

    public class SearchArticleSuccessAction : BaseAction {
    }
}