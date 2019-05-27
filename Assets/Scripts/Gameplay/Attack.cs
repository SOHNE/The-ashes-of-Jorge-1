using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    private int damage;

    private void Awake() {
        if (!GetComponent<Boomerang>()) {
            damage = GetComponentInParent<CharacterBase>().damage;
        } else {
            damage = 1;
        }

    }

    void OnTriggerEnter(Collider other) {
        //Destroy(other.gameObject);

        if (!other.GetComponent<CharacterBase>()) { return; }
        if (other.GetComponent<CharacterBase>().HP <= 0) { return; }
        
        other.GetComponent<CharacterBase>().TookDamage(damage);

        if (!gameObject.CompareTag("Player")) { return; }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().AttackComplete();
        GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>().anim_.SetTrigger("Hited");

    }

}