using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput.PlatformSpecific {
    public class MobileInput : VirtualInput {
        void AddButton(string name) {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
        }


        void AddAxes(string name) {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInputManager.RegisterVirtualAxis(new CrossPlatformInputManager.VirtualAxis(name));
        }


        public override float GetAxis(string name, bool raw) {
            if (!this.m_VirtualAxes.ContainsKey(name)) {
                this.AddAxes(name);
            }

            return this.m_VirtualAxes[name].GetValue;
        }


        public override void SetButtonDown(string name) {
            if (!this.m_VirtualButtons.ContainsKey(name)) {
                this.AddButton(name);
            }

            this.m_VirtualButtons[name].Pressed();
        }


        public override void SetButtonUp(string name) {
            if (!this.m_VirtualButtons.ContainsKey(name)) {
                this.AddButton(name);
            }

            this.m_VirtualButtons[name].Released();
        }


        public override void SetAxisPositive(string name) {
            if (!this.m_VirtualAxes.ContainsKey(name)) {
                this.AddAxes(name);
            }

            this.m_VirtualAxes[name].Update(1f);
        }


        public override void SetAxisNegative(string name) {
            if (!this.m_VirtualAxes.ContainsKey(name)) {
                this.AddAxes(name);
            }

            this.m_VirtualAxes[name].Update(-1f);
        }


        public override void SetAxisZero(string name) {
            if (!this.m_VirtualAxes.ContainsKey(name)) {
                this.AddAxes(name);
            }

            this.m_VirtualAxes[name].Update(0f);
        }


        public override void SetAxis(string name, float value) {
            if (!this.m_VirtualAxes.ContainsKey(name)) {
                this.AddAxes(name);
            }

            this.m_VirtualAxes[name].Update(value);
        }


        public override bool GetButtonDown(string name) {
            if (this.m_VirtualButtons.ContainsKey(name)) {
                return this.m_VirtualButtons[name].GetButtonDown;
            }

            this.AddButton(name);
            return this.m_VirtualButtons[name].GetButtonDown;
        }


        public override bool GetButtonUp(string name) {
            if (this.m_VirtualButtons.ContainsKey(name)) {
                return this.m_VirtualButtons[name].GetButtonUp;
            }

            this.AddButton(name);
            return this.m_VirtualButtons[name].GetButtonUp;
        }


        public override bool GetButton(string name) {
            if (this.m_VirtualButtons.ContainsKey(name)) {
                return this.m_VirtualButtons[name].GetButton;
            }

            this.AddButton(name);
            return this.m_VirtualButtons[name].GetButton;
        }


        public override Vector3 MousePosition() {
            return this.virtualMousePosition;
        }
    }
}