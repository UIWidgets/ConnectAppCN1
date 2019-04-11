namespace ConnectApp.redux.actions {
    public class AddCountAction : BaseAction {
        public int number;
    }
    
    public static partial class Actions {
        public static object addCount(int number) {
            return new AddCountAction {number = number};
        }
    }
}