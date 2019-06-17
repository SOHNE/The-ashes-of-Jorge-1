using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class attached to [ Limits ] - End Object
/// </summary>
public class EndGame : MonoBehaviour {
    private GameManager gm;

    private void Awake() => gm = FindObjectOfType<GameManager>();

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Player")) { return; }
        gm.ChangeScreen(1);
    }
}