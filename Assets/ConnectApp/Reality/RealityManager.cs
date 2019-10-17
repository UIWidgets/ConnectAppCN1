using System;
using System.Collections.Generic;
using UnityEngine;

namespace ConnectApp.Reality {
    public class RealityManager : MonoBehaviour {
        public static RealityManager instance;

        public Phone phone;
        public RealityViewer viewer;

        public bool enterReality = false;
        public List<GameObject> sceneObjects = new List<GameObject>();

        void Awake() {
            if (instance == null || instance != this) {
                instance = this;
            }

            Debug.Assert(this.phone);
            Debug.Assert(this.viewer);
        }

        void Start() {
            this.DisplaySceneObjects(false);
        }

        public static void TriggerSwitch() {
            if (instance) {
                

                instance.phone.TriggerSwitch();
            }
        }

        public void DisplaySceneObjects(bool state) {
            foreach (var item in this.sceneObjects) {
                item.SetActive(state);
            }
        }

        public static void OnBackToAppFinished() {
            instance.DisplaySceneObjects(false);
            instance.viewer.SetActive(false);
        }
    }
}