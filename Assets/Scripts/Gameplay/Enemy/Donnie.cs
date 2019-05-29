using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Donnie : Enemy {

    private void Start() {
        currentHealth = maxHealth;
        
    }

    private void Update()
    {
        if (HB) { HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0)); }

        walkTimer += Time.deltaTime;
        anim.SetBool("isDead", isDead);
    }

    void FixedUpdate()
    {
        if (isDead) { return; }

        forcaH = TargetDistance.x / Mathf.Abs(TargetDistance.x);

        if (!Attacking)
        {
            if (walkTimer > Random.Range(1f, 2f))
            {
                forcaZ = Random.Range(-1, 2);
                if (Camera.main.WorldToScreenPoint(transform.position).x > 40 && Camera.main.WorldToScreenPoint(transform.position).x < 900)
                {
                    forcaH = Random.Range(-1, 2);
                }
                walkTimer = 0;
            }
        }
        else
        {
            forcaZ = TargetDistance.z / Mathf.Abs(TargetDistance.z);
        }

        if (Mathf.Abs(TargetDistance.x) < stopDistance) { forcaH = 0; }

        MoveHandler(forcaH, forcaZ);

        if (!Attacking) { return; }

        bool attack = Mathf.Abs(TargetDistance.x) < 1.7f && Mathf.Abs(TargetDistance.z) < 1f;
        if (attack && Time.time > nextAttack)
        {
            Attack();
        }
    }

    
}