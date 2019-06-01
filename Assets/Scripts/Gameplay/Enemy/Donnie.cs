using UnityEngine;

public class Donnie : Enemy {

    private void Start() {
        currentHealth = maxHealth;
    }

    protected override void BasicMove() {
        Vector2 move = default;

        move.x = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);

        if (!Attacking) {
            if (walkTimer <= Random.Range(1f, 2f)) { return; }

            move.y = Random.Range(-1, 2);
            if (Camera.main.WorldToScreenPoint(transform.position).x > 40 && Camera.main.WorldToScreenPoint(transform.position).x < 900) {
                move.x = Random.Range(-1, 2);
            }
            walkTimer = 0;
        } else {
            move.y = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);
        }

        if (Mathf.Abs(PlayerDistance.x) < stopDistance) { move.x = 0; }

        MoveHandler(move);
    }

    protected override void BasicAttack() {
        bool attack = Mathf.Abs(PlayerDistance.x) < 1.7f && Mathf.Abs(PlayerDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }

}