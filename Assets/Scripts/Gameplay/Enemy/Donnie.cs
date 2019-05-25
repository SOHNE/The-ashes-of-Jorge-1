using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donnie : Enemy {

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

        float forcaH = TargetDistance.x / Mathf.Abs(TargetDistance.x);
        float forcaZ = TargetDistance.z / Mathf.Abs(TargetDistance.z);

        if (Mathf.Abs(TargetDistance.x) < stopDistance) { forcaH = 0; }
        if (Mathf.Abs(TargetDistance.z) < stopDistance) { forcaZ = 0; }


        if (!damaged) { MoveHandler(forcaH, forcaZ); }

        bool attack = Mathf.Abs(TargetDistance.x) < 3f && Mathf.Abs(TargetDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }
}