using UnityEngine;

public class William : Enemy {

    private void Start() {
        currentHealth = maxHealth;
    }

    protected override void BasicAttack() {
        bool attack = Mathf.Abs(PlayerDistance.x) < 1.7f && Mathf.Abs(PlayerDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }


}