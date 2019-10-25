using ConnectApp.Reality;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UnityChanController : MonoBehaviour {
    public float speed = 6f;
    public float rotateSpeed = 5f;

    Vector3 movement; // The vector to store the direction of the player's movement.
    public Animator animator; // Reference to the animator component.
    Rigidbody playerRigidbody; // Reference to the player's rigidbody.

    public bool isMoving {
        get { return this._isMoving; }
        set {
            this._isMoving = value;
            this.animator.SetBool("isRunning", this._isMoving);
        }
    }

    bool _isMoving;
    public bool enableToMove = false;

    void Awake() {
        // Set up references.
        Debug.Assert(this.animator);
        this.playerRigidbody = this.GetComponent<Rigidbody>();
    }

    void Update() {
        if (this.enableToMove) {
            this.CheckMove();
        }
    }

    void SmoothRotate(float yAngle) {
        var lerpQuaternion = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0, yAngle, 0),
            this.rotateSpeed * Time.deltaTime);
        this.transform.rotation = lerpQuaternion;
    }

    public void CheckMove() {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        if (h == 0) {
            h = CrossPlatformInputManager.GetAxis("Horizontal");
        }

        if (v == 0) {
            v = CrossPlatformInputManager.GetAxis("Vertical");
        }

        var movementVector = (new Vector2(h, v)).normalized;
        var yAngle = Vector2.Angle(Vector2.right, movementVector);
        if (movementVector.y < 0) {
            yAngle = 360 - yAngle;
        }

        yAngle = 180 - yAngle - 45;

        if (h != 0 || v != 0) {
            this.isMoving = true;
            this.SmoothRotate(yAngle);
            this.playerRigidbody.MovePosition(this.transform.position +
                                              this.transform.forward * this.speed * Time.deltaTime);
        }
        else {
            this.isMoving = false;
        }
    }

    void OnTriggerEnter(Collider other) {
        RealityManager.instance.miniGame.AddTime();
        Destroy(other.gameObject);
    }
}