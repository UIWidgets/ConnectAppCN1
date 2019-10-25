using ConnectApp.Reality;
using UnityEngine;

public class RealityViewer : MonoBehaviour {
    bool m_gyroEnabled = false;
    Gyroscope gyro;

    Quaternion rot;
    public bool isActived = false;
    Camera mainCamera;

    float originYAngle;
    Quaternion originRotation;
    Quaternion originAttitude;

    void Start() {
        this.mainCamera = Camera.main;
        this.mainCamera.clearFlags = CameraClearFlags.SolidColor;
    }

    public void SetActive(bool b) {
        if (b) {
            this.isActived = true;
            this.mainCamera.clearFlags = CameraClearFlags.Skybox;

            if (RealityManager.instance.useGyro) {
                if (!this.m_gyroEnabled) {
                    this.m_gyroEnabled = this.EnableGyro();
                }
                else {
                    this.ResetGyro();
                }

                this.rot = this.originRotation;
                // this.rot = this.originRotation * this.FlipLeftHandToRight(Quaternion.Inverse(this.gyro.attitude));
            }
        }
        else {
            this.mainCamera.clearFlags = CameraClearFlags.SolidColor;
            this.isActived = false;
        }
    }

    bool EnableGyro() {
        if (SystemInfo.supportsGyroscope) {
            this.gyro = Input.gyro;

            this.ResetGyro();

            // Debug.Log(this.originRotation.eulerAngles);

            this.originRotation = Quaternion.Euler(90f, this.transform.eulerAngles.y, 0f);

            Debug.Log("Gyro Enabled");

            return true;
        }

        Debug.Log("Gyro Disable");

        return false;
    }

    public void ResetGyro(float? yAngle = null) {
        if (this.m_gyroEnabled) {
            Debug.Log("Reset Gyro");
            this.gyro.enabled = false;
            this.gyro.enabled = true;
        }

        // if (yAngle != null) {
        //     this.rot = Quaternion.Euler(90f, (float) yAngle, 0f);
        // }
    }

    Quaternion FlipLeftHandToRight(Quaternion origin) {
        origin.z = -origin.z;
        origin.w = -origin.w;
        return origin;
    }

    void CheckHit(RaycastHit hitInfo) {
        Debug.Log(hitInfo.transform.name);
        if (hitInfo.transform.name == "Phone Shell") {
            if (RealityManager.instance.miniGame.isPause) {
                RealityManager.TriggerSwitch();
            }
        }

        if (hitInfo.transform.name == "Computer Detector") {
            RealityManager.instance.miniGame.StartGame();
        }
    }

    void Update() {
        if (!this.isActived) {
            return;
        }

        if (this.m_gyroEnabled) {
            var attitude = this.FlipLeftHandToRight(this.gyro.attitude);
            // float angle = 0f;
            // Vector3 axis;
            // attitude.ToAngleAxis(out angle, out axis);
            // Debug.Log(axis.ToString() + "@" + angle);
            this.transform.localRotation = this.rot * attitude;
        }

        if (RealityManager.instance.phone.castable) {
            if (!RealityManager.instance.miniGame.duringGame) {
                if (Input.touchCount > 0) {
                    var touch = Input.GetTouch(0);
                    Ray mouseDownCheckRay = this.mainCamera.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(mouseDownCheckRay, out hitInfo, Mathf.Infinity,
                        LayerMask.GetMask("Reality Detect"))) {
                        this.CheckHit(hitInfo);
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    Ray mouseDownCheckRay = this.mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(mouseDownCheckRay, out hitInfo, Mathf.Infinity,
                        LayerMask.GetMask("Reality Detect"))) {
                        this.CheckHit(hitInfo);
                    }
                }
            }
        }
    }
}