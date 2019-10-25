using UnityEngine;

public class DebugViewer : MonoBehaviour {
    public bool isFreeMode = false;

    public float mouseRotateSpeed = 5f;
    public float rotateSpeed = 2f;

    public float moveSpeed = 5f;

    public bool enableMove;
    public bool enableRotate;

    void DoFreeModeRotation() {
        Vector2 moveDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * this.mouseRotateSpeed;

        this.transform.RotateAround(this.transform.position, this.transform.up, moveDelta.x);
        this.transform.RotateAround(this.transform.position, this.transform.right, -moveDelta.y);

        float qeDelta = Input.GetKey(KeyCode.E) ? -1f : Input.GetKey(KeyCode.Q) ? 1f : 0f;
        if (qeDelta != 0) {
            this.transform.RotateAround(this.transform.position, this.transform.forward, qeDelta * this.rotateSpeed);
        }
    }

    void DoConstrainedRotation() {
        Vector2 moveDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * this.mouseRotateSpeed;

        var horizontalRotation = Quaternion.AngleAxis(moveDelta.x, Vector3.up);
        var verticalRotation = Quaternion.AngleAxis(-moveDelta.y, this.transform.right);

        this.transform.localRotation = horizontalRotation * verticalRotation * this.transform.localRotation;
    }

    void DoJoyStickConstrainedRotation() {
        var vh = Input.GetAxis("ViewHorizontal");
        var vv = Input.GetAxis("ViewVertical");

        if (vh == 0 && vv == 0) {
            return;
        }

        Vector2 moveDelta = new Vector2(vh, vv) * this.mouseRotateSpeed;

        var horizontalRotation = Quaternion.AngleAxis(moveDelta.x, Vector3.up);
        var verticalRotation = Quaternion.AngleAxis(-moveDelta.y, this.transform.right);

        this.transform.localRotation = horizontalRotation * verticalRotation * this.transform.localRotation;
    }

    void Update() {
        if (this.enableRotate) {
            if (Input.GetMouseButton(1)) {
                if (this.isFreeMode) {
                    this.DoFreeModeRotation();
                }
                else {
                    this.DoConstrainedRotation();
                }
            }
        }


        if (this.enableMove) {
            var h = Input.GetAxis("Horizontal");
            var v = Input.GetAxis("Vertical");
            if (v != 0) {
                this.transform.position += v * this.transform.forward * this.moveSpeed * Time.deltaTime;
            }

            if (h != 0) {
                this.transform.position += h * this.transform.right * this.moveSpeed * Time.deltaTime;
            }
        }

        // this.DoJoyStickConstrainedRotation();
    }
}