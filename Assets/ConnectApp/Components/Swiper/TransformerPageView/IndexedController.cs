using RSG;
using Unity.UIWidgets.foundation;

namespace ConnectApp.Components.Swiper {
    public class IndexController : ChangeNotifier {
        public const int NEXT = 1;
        public const int PREVIOUS = -1;
        public const int MOVE = 0;

        Promise<object> _completer;

        public int index;
        public bool? animation;
        public int evt;

        public Promise<object> move(int index, bool animation = true) {
            this.animation = animation;
            this.index = index;
            this.evt = MOVE;
            this._completer = new Promise<object>();
            this.notifyListeners();
            return this._completer;
        }

        public Promise<object> next(bool animation = true) {
            this.evt = NEXT;
            this.animation = animation;
            this._completer = new Promise<object>();
            this.notifyListeners();
            return this._completer;
        }

        public Promise<object> previous(bool animation = true) {
            this.evt = PREVIOUS;
            this.animation = animation;
            this._completer = new Promise<object>();
            this.notifyListeners();
            return this._completer;
        }

        public void complete() {
            if (!this._completer.isCompleted) {
                this._completer.Resolve(null);
            }
        }
    }
}