#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;


namespace UnityStandardAssets.CrossPlatformInput {
    [ExecuteInEditMode]
    public class MobileControlRig : MonoBehaviour
#if UNITY_EDITOR
        , IActiveBuildTargetChanged
#endif
    {
        // this script enables or disables the child objects of a control rig
        // depending on whether the USE_MOBILE_INPUT define is declared.

        // This define is set or unset by a menu item that is included with
        // the Cross Platform Input package.


#if !UNITY_EDITOR
	void OnEnable()
	{
		CheckEnableControlRig();
	}
#else
        public int callbackOrder {
            get { return 1; }
        }
#endif

        void Start() {
#if UNITY_EDITOR
            if (Application.isPlaying
                ) //if in the editor, need to check if we are playing, as start is also called just after exiting play
#endif
            {
                EventSystem system = FindObjectOfType<EventSystem>();

                if (system == null) {
                    //the scene have no event system, spawn one
                    GameObject o = new GameObject("EventSystem");

                    o.AddComponent<EventSystem>();
                    o.AddComponent<StandaloneInputModule>();
                }
            }
        }

#if UNITY_EDITOR

        void OnEnable() {
            EditorApplication.update += this.Update;
        }


        void OnDisable() {
            EditorApplication.update -= this.Update;
        }


        void Update() {
            this.CheckEnableControlRig();
        }
#endif


        void CheckEnableControlRig() {
            this.EnableControlRig(true);

            // this.EnableControlRig(false);
        }


        void EnableControlRig(bool enabled) {
            foreach (Transform t in this.transform) {
                t.gameObject.SetActive(enabled);
            }
        }

#if UNITY_EDITOR
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget) {
            this.CheckEnableControlRig();
        }
#endif
    }
}