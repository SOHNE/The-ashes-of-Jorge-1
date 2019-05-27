using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointer : MonoBehaviour {

    private bool used;
    public bool isUsed { get => used; }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Enemy")) { return; }
        used = true;
    }

    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Enemy")) { return; }
        used = false;
    }

}