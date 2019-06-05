using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private CameraFollow cf;
    public FreeParallax parallax;

    private void Awake() => cf = GetComponent<CameraFollow>();

    void Update() => parallax.Speed = (Input.GetAxis("Horizontal") > .0f && !cf.IsBlock && cf.CheckXMargin()) ? -1.25f : .0f;
}