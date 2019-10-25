using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameCameraFollower : MonoBehaviour {
    public Transform toTrack;

    Vector3 relativePosition;

    void Start() {
        this.relativePosition = this.toTrack.position - this.transform.position;
    }

    void Update() {
        this.transform.position = this.toTrack.position - this.relativePosition;
    }
}