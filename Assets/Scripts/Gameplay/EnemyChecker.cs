using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour {
    [SerializeField] private int Current;
    public int Max;
    public bool isMax;
    public List<Enemy> Fighting;


    private void Disable() {
        GameObject[] all = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < all.Length; i++) {

        }

    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Enemy")) { return; }
        Current++;
        Fighting.Add(other.gameObject.GetComponent<Enemy>());
        Disable();
    }
    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Enemy")) { return; }
        Current--;
        Fighting.Remove(other.gameObject.GetComponent<Enemy>());
        Disable();

    }
}


