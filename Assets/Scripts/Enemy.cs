using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase {

    private void Update()
    {
        walkTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            float forcaH = TargetDistance.x / Mathf.Abs(TargetDistance.x);

            if (walkTimer >= Random.Range(0f, 2f))
            {
                forcaZ = Random.Range(-1, 2);
                walkTimer = 0;
            }

            if (Mathf.Abs(TargetDistance.x) < stopDistance)
                forcaH = 0;

            if (!damaged)
                MoveHandler(forcaH, forcaZ);

            if (Mathf.Abs(TargetDistance.x) < 1.5f && Mathf.Abs(TargetDistance.z) < 1f && Time.time > nextAttack)
                Attack();
        }
    }

}
