using UnityEngine;

public class Kamikaze : Enemy {

    private void Start() {
        maxHealth = 3;
        currentHealth = maxHealth;
        this.stopDistance = 3f;
    }

    private void Update() {
        walkTimer += Time.time;
        anim.SetBool("isDead", isDead);
    }

    void FixedUpdate() {
        if (isDead) { return; }

        float forcaH = PointDistance.x / Mathf.Abs(PointDistance.x);
        forcaZ = PointDistance.z / Mathf.Abs(PointDistance.z);

        //if (Mathf.Abs(PointDistance.x) < stopDistance) { forcaH = 0; forcaZ = 0; }

        if (!damaged) { MoveHandler(forcaH, forcaZ); }

        bool attack = Mathf.Abs(PointDistance.x) < 3f && Mathf.Abs(PointDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }
}