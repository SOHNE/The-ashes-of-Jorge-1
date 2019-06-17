using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthy : MonoBehaviour {
    [SerializeField] private int Points = 5;

    private void OnTriggerStay(Collider other) {
        Player person = other.GetComponent<Player>();

        if (!person || person.IsMaxLife || person.IsDead) { return; }

        person.Recover(Points);
        Destroy(gameObject);
    }

}