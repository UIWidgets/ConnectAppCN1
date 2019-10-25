using UnityEngine;

public class FloatingItem : MonoBehaviour {
    public Transform model;

    public float floatingFrequency = 3f;
    public float floatingAmplitude = 0.3f;
    public float rotateSpeed = 100f;
    float floatingOffset;
    Vector3 startOffset;
    float floatingTimer = 0f;

    public void SpinAnimation() {
        var rotation = this.model.eulerAngles;
        rotation.y += this.rotateSpeed * Time.deltaTime;
        this.model.rotation = Quaternion.Euler(rotation);

        this.floatingTimer += Time.deltaTime;
        if (this.floatingTimer > 2 * this.floatingFrequency) {
            this.floatingTimer = 0f;
        }

        var position = this.model.position;
        this.floatingOffset =
            Mathf.Cos(Mathf.PI * (this.floatingTimer - this.floatingFrequency) / this.floatingFrequency) *
            this.floatingAmplitude;
        position = this.transform.position + this.startOffset + this.floatingOffset * this.transform.up;
        this.model.position = position;
    }

    void Start() {
        this.startOffset = this.model.position - this.transform.position;
    }

    public void Update() {
        this.SpinAnimation();
    }
}