using UnityEngine;

public class Donnie : Enemy {

    private void Start() {
        currentHealth = maxHealth;
    }

    protected override void BasicMove() {
        forcaH = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);

        if (!Attacking) {
            if (walkTimer <= Random.Range(1f, 2f)) { return; }

            forcaZ = Random.Range(-1, 2);
            if (Camera.main.WorldToScreenPoint(transform.position).x > 40 && Camera.main.WorldToScreenPoint(transform.position).x < 900) {
                forcaH = Random.Range(-1, 2);
            }
            walkTimer = 0;
        } else {
            forcaZ = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);
        }

        if (Mathf.Abs(PlayerDistance.x) < stopDistance) { forcaH = 0; }

        MoveHandler(forcaH, forcaZ);
    }

    protected override void BasicAttack() {
        bool attack = Mathf.Abs(PlayerDistance.x) < 1.7f && Mathf.Abs(PlayerDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }


}