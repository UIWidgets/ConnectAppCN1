using ConnectApp.Reality;
using UnityEngine;

public class RealityViewer : MonoBehaviour {
    public bool isActived = false;

    bool m_GyroEnabled = false;
    bool m_VirtualGyroEnabled = false;
    public Transform gyroOverride;
    Gyroscope m_Gyro;
    Camera m_MainCamera;

    // Gyro Variables
    float m_InitialYAngle = 0f;
    float m_AppliedGyroYAngle = 0f;
    float m_CalibrationYAngle = 0f;

    Quaternion originRotation;

    Transform gyroInstance;

    void Awake() {
        this.m_GyroEnabled = this.InitGyro();
    }

    void Start() {
        this.m_MainCamera = Camera.main;
        this.m_MainCamera.clearFlags = CameraClearFlags.SolidColor;

        this.originRotation = this.transform.rotation;
        this.m_InitialYAngle = this.transform.eulerAngles.y;
    }

    public void SetActive(bool b) {
        if (b) {
            this.isActived = true;
            this.m_MainCamera.clearFlags = CameraClearFlags.Skybox;

            if (RealityManager.instance.useGyro) {
                if (this.m_GyroEnabled) {
                    this.ResetGyro();
                }
                else {
                    this.m_GyroEnabled = this.InitGyro();
                }

                if (!this.m_GyroEnabled && !this.m_VirtualGyroEnabled) {
                    this.m_VirtualGyroEnabled = this.InitVirtualGyro();
                    this.ResetGyro();
                }
            }
        }
        else {
            this.m_MainCamera.clearFlags = CameraClearFlags.SolidColor;
            this.isActived = false;
        }
    }

    bool InitGyro() {
        if (SystemInfo.supportsGyroscope) {
            Debuger.Log("Gyro Enabled");
            this.m_Gyro = Input.gyro;
            Input.gyro.enabled = true;

            this.gyroInstance = new GameObject("Gyro Instance").transform;
            this.gyroInstance.position = this.transform.position;
            this.gyroInstance.rotation = Input.gyro.attitude;
            this.gyroInstance.Rotate(0f, 0f, 180f, Space.Self);
            this.gyroInstance.Rotate(90f, 180f, 0f, Space.World);
            this.m_AppliedGyroYAngle = this.gyroInstance.eulerAngles.y;

            this.m_CalibrationYAngle = this.m_AppliedGyroYAngle - this.m_InitialYAngle;

            this.ApplyGyroRotation();
            this.ApplyCalibration();

            return true;
        }

        Debuger.Log("Gyro Disabled");
        return false;
    }

    bool InitVirtualGyro() {
        if (this.gyroOverride != null) {
            Debuger.Log("Use Virtual Gyro");
            this.gyroInstance = new GameObject("Gyro Instance").transform;
            this.gyroInstance.position = this.transform.position;
            this.gyroInstance.rotation = this.transform.rotation;
            return true;
        }

        Debuger.Log("Virtual Gyro Disabled");
        return false;
    }

    public void ResetGyro() {
        if (this.m_GyroEnabled) {
            Debuger.Log("Reset Gyro");

            this.m_CalibrationYAngle = this.m_AppliedGyroYAngle - this.m_InitialYAngle;

            this.ApplyGyroRotation();
            this.ApplyCalibration();
        }
        else if (this.m_VirtualGyroEnabled) {
            Debuger.Log("Reset Virtual Gyro");
            this.gyroOverride.rotation = this.originRotation;
            this.transform.localRotation = this.gyroOverride.rotation;
        }
        else {
            this.transform.localRotation = this.originRotation;
        }
    }

    void CheckHit(RaycastHit hitInfo) {
        Debuger.Log(hitInfo.transform.name);
        if (hitInfo.transform.name == "Phone Shell") {
            if (RealityManager.instance.miniGame.isPause) {
                RealityManager.TriggerSwitch();
            }
        }

        if (hitInfo.transform.name == "Computer Detector" || hitInfo.transform.name == "Unity Chan") {
            RealityManager.instance.miniGame.StartGame();
        }
    }

    void ApplyGyroRotation() {
        this.transform.rotation = Input.gyro.attitude;
        this.transform.Rotate(0f, 0f, 180f, Space.Self);
        this.transform.Rotate(90f, 180f, 0f, Space.World);
    }

    void ApplyCalibration() {
        this.transform.Rotate(0f, -this.m_CalibrationYAngle, 0f, Space.World);
    }

    void Update() {
        // Update gyro
        if (this.gyroInstance && this.m_GyroEnabled) {
            this.gyroInstance.rotation = Input.gyro.attitude;
            this.gyroInstance.Rotate(0f, 0f, 180f, Space.Self);
            this.gyroInstance.Rotate(90f, 180f, 0f, Space.World);
            this.m_AppliedGyroYAngle = this.gyroInstance.eulerAngles.y;
        }

        if (!this.isActived) {
            return;
        }

        if (this.m_GyroEnabled) {
            this.ApplyGyroRotation();
            this.ApplyCalibration();
        }
        else if (this.m_VirtualGyroEnabled) {
            this.transform.localRotation = this.gyroOverride.rotation;
        }

        if (RealityManager.instance.phone.castable) {
            if (!RealityManager.instance.miniGame.duringGame) {
                if (Input.touchCount > 0) {
                    var touch = Input.GetTouch(0);
                    Ray mouseDownCheckRay = this.m_MainCamera.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(mouseDownCheckRay, out hitInfo, Mathf.Infinity,
                        LayerMask.GetMask("Reality Detect"))) {
                        this.CheckHit(hitInfo);
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    Ray mouseDownCheckRay = this.m_MainCamera.ScreenPointToRay(Input.mousePosition);
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