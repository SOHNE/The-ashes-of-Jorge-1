using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        //Destroy(other.gameObject);
        CharacterBase person = other.GetComponent<CharacterBase>();

        if (!person) { return; }

        CharacterBase agressor = GetComponentInParent<CharacterBase>();

        person.TookDamage(agressor.damage);
        
        if (person.HP <= 0) { return; }
        if (!CompareTag("Player")) { return; }
        
        agressor.OnValidAttack();
        agressor.anim_.SetTrigger("Hited");
    }
}