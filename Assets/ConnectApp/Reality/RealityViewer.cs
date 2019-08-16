using UnityEngine;

public class RealityViewer : MonoBehaviour {
    bool m_cursorIsLocked = false;
    bool m_gyroEnabled = false;
    Gyroscope gyro;

    Quaternion rot;
    public bool isActived = false;
    Camera m_MainCamera;

    float originYAngle;
    Quaternion originRotation;
    Quaternion originAttitude;

    void Start() {
        this.m_MainCamera = Camera.main;
        this.m_MainCamera.clearFlags = CameraClearFlags.SolidColor;
    }

    public void SetActive(bool b) {
        if (b) {
            this.isActived = true;
            this.m_MainCamera.clearFlags = CameraClearFlags.Skybox;
            if (!this.m_gyroEnabled)
                this.m_gyroEnabled = this.EnableGyro();

            this.rot = this.originRotation;
            // this.rot = this.originRotation * this.FlipLeftHandToRight(Quaternion.Inverse(this.gyro.attitude));
        }
        else {
            this.m_MainCamera.clearFlags = CameraClearFlags.SolidColor;
            this.isActived = false;
        }
    }

    bool EnableGyro() {
        if (SystemInfo.supportsGyroscope) {
            this.gyro = Input.gyro;
            this.gyro.enabled = true;

            this.originRotation = Quaternion.Euler(90f, this.transform.eulerAngles.y, 0f); 

            return true;
        }

        return false;
    }

    Quaternion FlipLeftHandToRight(Quaternion origin) {
        origin.z = -origin.z;
        origin.w = -origin.w;
        return origin;
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
    }
}