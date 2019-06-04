using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private CameraFollow cf;
    public FreeParallax parallax;

    private void Awake() => cf = GetComponent<CameraFollow>();

    void Update() {

        bool pressed = Input.GetAxis("Horizontal") > .0f;

        if (pressed && cf.CheckXMargin()) {
            parallax.Speed = -1.25f;
        } else {
            parallax.Speed = 0.0f;
        }

    }
}