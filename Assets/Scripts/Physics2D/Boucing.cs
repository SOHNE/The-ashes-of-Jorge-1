using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boucing : MonoBehaviour {
    private Rigidbody rb;
    private void Awake() => rb = GetComponent<Rigidbody>();

    void Update() {
        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

        rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 3f, maxWidth + 4f), rb.position.y, rb.position.z);
    }

    private void OnCollisionEnter(Collision collision) {
        if (!collision.collider.CompareTag("Enemy") || rb.velocity.magnitude < 2.2f) { return; }

        collision.collider.GetComponent<CharacterBase>().TookDamage(10);
    }
}