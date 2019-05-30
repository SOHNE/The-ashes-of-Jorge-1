using UnityEngine;

public class Donnie : Enemy {

    private void Start() {
        currentHealth = maxHealth;
    }

    protected override void BasicMove() {
        forcaH = PointDistance.x / Mathf.Abs(PointDistance.x);

        if (!Attacking) {
            if (walkTimer <= Random.Range(1f, 2f)) { return; }

            forcaZ = Random.Range(-1, 2);
            if (Camera.main.WorldToScreenPoint(transform.position).x > 40 && Camera.main.WorldToScreenPoint(transform.position).x < 900) {
                forcaH = Random.Range(-1, 2);
            }
            walkTimer = 0;
        } else {
            forcaZ = PointDistance.z / Mathf.Abs(PointDistance.z);
        }

        if (Mathf.Abs(PointDistance.x) < stopDistance) { forcaH = 0; }

        MoveHandler(forcaH, forcaZ);
    }

    protected override void BasicAttack() {
        bool attack = Mathf.Abs(PointDistance.x) < 1.7f && Mathf.Abs(PointDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }


}