using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        CharacterBase person = other.GetComponent<CharacterBase>();

        if (!person) { return; }

        CharacterBase agressor = GetComponentInParent<CharacterBase>();

        person.TookDamage(agressor.damage);

        if (person.HP <= 0 || !agressor.CompareTag("Player")) { return; }

        if (agressor.GetComponent<Player>().IsKicking) { person.Push(); person.isFalling = true; }

        agressor.OnValidAttack();
        agressor.anim_.SetTrigger("Hited");
    }
}