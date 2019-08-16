using System;
using ConnectApp.Main;
using UnityEngine;

namespace ConnectApp.Reality {
    public class Phone : MonoBehaviour {
        public Transform onTable;
        public Transform inHand;

        public Transform shell;
        public Transform screen;
        public RectTransform screenRect;
        public Canvas screenCanvas;

        public Vector2 defaultScreenSize = new Vector2(750f, 1334f);
        public ConnectAppPanel appPanel;

        bool m_IsOnTable = false;
        bool m_IsInHand = true;
        bool m_TowardOnTable = false;

        bool m_IsMoving = false;
        float m_LerpDuration = 1f;
        float m_Timer = 0;

        bool m_EnableToCastRay = false;

        void Start() {
            Debug.Assert(this.shell && this.screen && this.screenRect);
            Debug.Assert(this.appPanel);
            this.screenCanvas.renderMode = RenderMode.ScreenSpaceCamera;

            this.SetScreenSize(Screen.width, Screen.height);

            // Debug.Log("Screen Eye Distance: " + this.inHand.localPosition.z);
            // Debug.Log(Screen.width + ", " + Screen.height);
        }

        void SetScreenSize(float width, float height) {
            var widthRatio = width / this.defaultScreenSize.x;
            var heightRatio = height / this.defaultScreenSize.y;

            this.screenRect.sizeDelta = new Vector2(width, height);
            var shellScale = this.shell.transform.localScale;
            shellScale.x *= widthRatio;
            shellScale.y *= heightRatio;
            this.shell.transform.localScale = shellScale;

            var inHandLocalPosition = this.inHand.localPosition;
            inHandLocalPosition.z *= widthRatio > heightRatio ? widthRatio : heightRatio;
            this.inHand.localPosition = inHandLocalPosition;
            this.screenCanvas.planeDistance = inHandLocalPosition.z;

            this.transform.position = this.inHand.position;
            this.transform.rotation = this.inHand.rotation;
        }

        public void TriggerSwitch() {
            this.m_TowardOnTable = !this.m_TowardOnTable;
            if (!this.m_IsMoving) {
                if (!this.m_TowardOnTable) {
                    this.m_Timer = this.m_LerpDuration;
                    this.m_EnableToCastRay = false;
                    this.appPanel.raycastTarget = true;
                }
                else {
                    this.m_Timer = 0f;
                    this.screenCanvas.renderMode = RenderMode.WorldSpace;
                    this.appPanel.raycastTarget = false;
                }
            }

            this.m_IsMoving = true;
        }

        void Update() {
            if (this.m_EnableToCastRay) {
                if (Input.touchCount > 0) {
                    // Debug.Log("Touch");
                    var touch = Input.GetTouch(0);
                    Ray mouseDownCheckRay = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(mouseDownCheckRay, out hitInfo, Mathf.Infinity)) {
                        // Debug.Log(hitInfo.transform.name);
                        if (hitInfo.transform.name == "Phone") {
                            RealityManager.TriggerSwitch();
                        }
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    // Debug.Log("Mouse Down");
                    Ray mouseDownCheckRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(mouseDownCheckRay, out hitInfo, Mathf.Infinity)) {
                        // Debug.Log(hitInfo.transform.name);
                        if (hitInfo.transform.name == "Phone") {
                            RealityManager.TriggerSwitch();
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab)) {
                this.TriggerSwitch();
            }

            if (this.m_IsMoving) {
                if (this.m_TowardOnTable) {
                    this.m_Timer += Time.deltaTime;
                    if (this.m_Timer >= this.m_LerpDuration) {
                        // On Table
                        this.m_IsMoving = false;
                        this.m_IsOnTable = true;
                        this.m_IsInHand = false;

                        this.transform.position = this.onTable.position;
                        this.transform.rotation = this.onTable.rotation;
                        this.m_EnableToCastRay = true;
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
                        this.m_IsOnTable = false;
                        this.m_IsInHand = true;

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