using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreBoss : MonoBehaviour {
    public Maestria Maestro;
    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) { return; }
        StartCoroutine(Maestro.MuteTransition(20));
    }
}