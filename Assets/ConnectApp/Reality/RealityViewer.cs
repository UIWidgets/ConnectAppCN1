using ConnectApp.Reality;
using UnityEngine;

public class RealityViewer : MonoBehaviour {
    bool m_GyroEnabled = false;
    Gyroscope gyro;

    public bool isActived = false;
    Camera mainCamera;

    float originYAngle;
    Quaternion originRotation;
    Quaternion originGyroBaseRotation;


    bool m_VirtualGyroEnabled;

    public Transform gyroOverride;
    Quaternion m_ViturlaGyroDefaultRotation;


    void Start() {
        this.mainCamera = Camera.main;
        this.mainCamera.clearFlags = CameraClearFlags.SolidColor;

        this.originRotation = this.transform.rotation;
        this.originGyroBaseRotation = Quaternion.Euler(90f, this.transform.eulerAngles.y, 0f);
    }

    public void SetActive(bool b) {
        if (b) {
            this.isActived = true;
            this.mainCamera.clearFlags = CameraClearFlags.Skybox;

            if (RealityManager.instance.useGyro) {
                if (!this.m_GyroEnabled) {
                    this.m_GyroEnabled = this.InitGyro();
                }

                if (!this.m_GyroEnabled && !this.m_VirtualGyroEnabled) {
                    this.m_VirtualGyroEnabled = this.InitVirtualGyro();
                }

                this.ResetGyro();

                // this.rot = this.originRotation * this.FlipLeftHandToRight(Quaternion.Inverse(this.gyro.attitude));
            }
        }
        else {
            this.mainCamera.clearFlags = CameraClearFlags.SolidColor;
            this.isActived = false;
        }
    }

    bool InitGyro() {
        if (SystemInfo.supportsGyroscope) {
            Debug.Log("Gyro Enabled");
            this.gyro = Input.gyro;
            return true;
        }

        Debug.Log("Gyro Disabled");
        return false;
    }

    bool InitVirtualGyro() {
        if (this.gyroOverride != null) {
            Debug.Log("Use Virtual Gyro");
            this.m_ViturlaGyroDefaultRotation = this.gyroOverride.rotation;
            return true;
        }

        Debug.Log("Virtual Gyro Disabled");
        return false;
    }

    public void ResetGyro() {
        if (this.m_GyroEnabled) {
            Debug.Log("Reset Gyro");
            this.gyro.enabled = false;
            this.gyro.enabled = true;

            var attitude = this.FlipLeftHandToRight(this.gyro.attitude);
            this.transform.localRotation = this.originGyroBaseRotation * attitude;
        }
        else if (this.m_VirtualGyroEnabled) {
            Debug.Log("Reset Virtual Gyro");
            this.gyroOverride.rotation = this.m_ViturlaGyroDefaultRotation;

            // this.transform.localRotation = this.gyroOverride.rotation;
        }
        else {
            this.transform.localRotation = this.originRotation;
        }
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

        if (hitInfo.transform.name == "Computer Detector" || hitInfo.transform.name == "Unity Chan") {
            RealityManager.instance.miniGame.StartGame();
        }
    }

    void Update() {
        if (!this.isActived) {
            return;
        }

        if (this.m_GyroEnabled) {
            var attitude = this.FlipLeftHandToRight(this.gyro.attitude);
            // float angle = 0f;
            // Vector3 axis;
            // attitude.ToAngleAxis(out angle, out axis);
            // Debug.Log(axis.ToString() + "@" + angle);
            this.transform.localRotation = this.originGyroBaseRotation * attitude;
        }
        else if (this.m_VirtualGyroEnabled) {
            this.transform.localRotation = this.gyroOverride.rotation;
            // float angle = 0f;
            // Vector3 axis;
            // this.gyroOverride.rotation.ToAngleAxis(out angle, out axis);
            // Debug.Log(axis.ToString() + "@" + angle);
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