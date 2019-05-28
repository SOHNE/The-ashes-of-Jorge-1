using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChecker : MonoBehaviour {
    [SerializeField] private int Current;
    public int Max;
    public bool isMax;

    Transform GetClosestEnemy(GameObject[] enemies) {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies) {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist) {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void Awake() {
        GameObject[] a = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void Update() {
        //Debug.Log(GetClosestEnemy(GameObject.FindGameObjectsWithTag("Enemy")).gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Enemy")) { return; }
        if (Current >= 2) { return; }
        Current++;

        other.GetComponent<Enemy>().Attacking = true;
    }
    private void OnTriggerExit(Collider other) {
        if (!other.CompareTag("Enemy")) { return; }
        Current--;

        other.GetComponent<Enemy>().Attacking = false;
    }

}