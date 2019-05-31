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

        float forcaH = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);
        forcaZ = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);

        //if (Mathf.Abs(PlayerDistance.x) < stopDistance) { forcaH = 0; forcaZ = 0; }

        if (!damaged) { MoveHandler(forcaH, forcaZ); }

        bool attack = Mathf.Abs(PlayerDistance.x) < 3f && Mathf.Abs(PlayerDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }
}