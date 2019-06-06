using UnityEngine;

public class Attack : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        CharacterBase person = other.GetComponent<CharacterBase>();

        if (!person) { return; }

        CharacterBase agressor = GetComponentInParent<CharacterBase>();

        person.TookDamage(agressor.damage);

        if (person.HP <= 0 || !agressor.CompareTag("Player")) { return; }

        agressor.OnValidAttack();
        agressor.anim_.SetTrigger("Hited");
    }
}