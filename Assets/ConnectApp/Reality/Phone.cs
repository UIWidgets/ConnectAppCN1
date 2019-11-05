using ConnectApp.Main;
using UnityEngine;

namespace ConnectApp.Reality {
    public class Phone : MonoBehaviour {
        public Transform onTable;
        public Transform inHand;
        public Transform shell;
        public Transform screen;
        public Canvas screenCanvas;

        public Vector2 defaultScreenSize = new Vector2(750f, 1334f);
        public ConnectAppPanel appPanel;

        bool m_TowardOnTable = false;

        bool m_IsMoving = false;
        float m_LerpDuration = 1f;
        float m_Timer = 0;

        public bool castable = false;

        void Start() {
            Debug.Assert(this.shell && this.screen);
            Debug.Assert(this.appPanel);
            this.screenCanvas.renderMode = RenderMode.ScreenSpaceCamera;

            this.SetScreenSize(Screen.width, Screen.height);

            Debuger.Log("Screen Eye Distance: " + this.inHand.localPosition.z);
            Debuger.Log(Screen.width + ", " + Screen.height);
        }

        void SetScreenSize(float width, float height) {
            Debuger.Log($"SetScreenSize ({width}, {height})");

            var widthRatio = width / this.defaultScreenSize.x;
            var heightRatio = height / this.defaultScreenSize.y;

            Debuger.Log($"W&H Ratio: ({widthRatio}, {heightRatio})");

            var shellScale = this.shell.transform.localScale;
            shellScale.x *= widthRatio;
            shellScale.y *= heightRatio;

            if (widthRatio < heightRatio) {
                shellScale.x *= widthRatio / heightRatio;
                shellScale.y *= widthRatio / heightRatio;
            }

            this.shell.transform.localScale = shellScale;

            var inHandLocalPosition = this.inHand.localPosition;
            Debuger.Log($"distance: {inHandLocalPosition.z}");
            var delta = inHandLocalPosition.z;
            inHandLocalPosition.z *= Mathf.Min(widthRatio, heightRatio);
            delta -= inHandLocalPosition.z;
            this.inHand.localPosition = inHandLocalPosition;
            this.screenCanvas.planeDistance -= delta;

            this.transform.position = this.inHand.position;
            this.transform.rotation = this.inHand.rotation;
        }

        public void TriggerSwitch() {
            RealityManager.instance.enterReality = !RealityManager.instance.enterReality;

            this.m_TowardOnTable = !this.m_TowardOnTable;
            if (!this.m_IsMoving) {
                if (!this.m_TowardOnTable) {
                    this.m_Timer = this.m_LerpDuration;
                    this.castable = false;
                    this.appPanel.raycastTarget = true;
                }
                else {
                    this.m_Timer = 0f;
                    this.screenCanvas.renderMode = RenderMode.WorldSpace;
                    this.appPanel.raycastTarget = false;
                }
            }

            this.m_IsMoving = true;

            if (RealityManager.instance.enterReality) {
                RealityManager.instance.DisplaySceneObjects(true);
                RealityManager.instance.viewer.SetActive(true);
            }
        }

        void Update() {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Tab)) {
                this.TriggerSwitch();
            }
#endif

            if (this.m_IsMoving) {
                if (this.m_TowardOnTable) {
                    this.m_Timer += Time.deltaTime;
                    if (this.m_Timer >= this.m_LerpDuration) {
                        // On Table
                        this.m_IsMoving = false;

                        this.transform.position = this.onTable.position;
                        this.transform.rotation = this.onTable.rotation;
                        this.castable = true;
                    }
                    else {
                        this.transform.position =
                            this.inHand.position + (this.onTable.position - this.inHand.position) *
                            this.m_Timer / this.m_LerpDuration;
                        this.transform.rotation = Quaternion.Slerp(this.inHand.rotation, this.onTable.rotation,
                            this.m_Timer / this.m_LerpDuration);
                    }
                }
                else {
                    this.m_Timer -= Time.deltaTime;
                    if (this.m_Timer <= 0) {
                        // In Hand
                        this.m_IsMoving = false;

                        this.transform.position = this.inHand.position;
                        this.transform.rotation = this.inHand.rotation;
                        this.screenCanvas.renderMode = RenderMode.ScreenSpaceCamera;

                        RealityManager.OnBackToAppFinished();
                    }
                    else {
                        this.transform.position =
                            this.inHand.position + (this.onTable.position - this.inHand.position) *
                            this.m_Timer / this.m_LerpDuration;
                        this.transform.rotation = Quaternion.Slerp(this.inHand.rotation, this.onTable.rotation,
                            this.m_Timer / this.m_LerpDuration);
                    }
                }
            }
        }
    }
}