using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        CharacterBase person = other.GetComponent<CharacterBase>();

        if (!person) { return; }

        CharacterBase agressor = GetComponentInParent<CharacterBase>();

        person.TookDamage(agressor.damage);
        if (agressor.IsPunching) {
            person.Push(agressor.Avoiding);
            agressor.IsPunching = false;
            agressor.Avoiding = false;
        }

        agressor.OnValidAttack();

        if (!agressor.CompareTag("Player")) { return; }

        agressor.anim_.SetTrigger("Hited");
    }

}