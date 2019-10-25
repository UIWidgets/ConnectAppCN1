using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput {
    public class ButtonHandler : MonoBehaviour {
        public string Name;

        void OnEnable() { }

        public void SetDownState() {
            CrossPlatformInputManager.SetButtonDown(this.Name);
        }


        public void SetUpState() {
            CrossPlatformInputManager.SetButtonUp(this.Name);
        }


        public void SetAxisPositiveState() {
            CrossPlatformInputManager.SetAxisPositive(this.Name);
        }


        public void SetAxisNeutralState() {
            CrossPlatformInputManager.SetAxisZero(this.Name);
        }


        public void SetAxisNegativeState() {
            CrossPlatformInputManager.SetAxisNegative(this.Name);
        }

        public void Update() { }
    }
}