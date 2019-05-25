using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthy : MonoBehaviour {
    public int Points;

    private void OnTriggerEnter(Collider other) {
        if (!other.name.Equals("Player")) { return; }
        CharacterBase person = other.GetComponent<CharacterBase>();

        if (person.HP <= 0) { return; }

        person.Recover(5);
        Destroy(gameObject);
    }

}