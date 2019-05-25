using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase {

    protected float walkTimer;
    public float stopDistance = 1f;

    protected int lastDamage;

    protected float forcaZ, forcaH;
    protected Transform Target => GameObject.FindGameObjectWithTag("Player").transform;
    protected Vector3 TargetDistance => Target.position - transform.position;

    private GameObject HB;
    private bool HB_show;

    private void HealthBar() {
        if (HB_show) { return; }
        HB_show = true;
        HB = Instantiate((GameObject) Resources.Load("Prefab/HealthBar"), GameObject.Find("-- World Space").transform);
        HB.transform.SetParent(GameObject.Find("-- World Space").transform, true);
        HB.GetComponentInChildren<HealthBar>().MaxHealthPoints = maxHealth;
    }

    private void Start() {
        //GetComponentInChildren<HealthBar>().MaxHealthPoints = maxHealth;
        //GetComponentInChildren<HealthBar>().Hurt(0);

        maxHealth = 3;
        //nextAttack = 3.5f;
        currentHealth = maxHealth;
    }

    private void Update() {
        walkTimer += Time.deltaTime;
        anim.SetBool("isDead", isDead);

        if (!HB) { return; }
        HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));

        /*
        if (TargetDistance.magnitude > 5f) {
            currentSpeed = 1;
        } else {
            currentSpeed = maxSpeed;
        }
        */
    }

    void FixedUpdate() {
        if (isDead) { return; }
        DamageLimiter();

        forcaH = TargetDistance.x / Mathf.Abs(TargetDistance.x);

        if (walkTimer > Random.Range(1f, 2f)) {
            forcaZ = Random.Range(-1, 2);
            walkTimer = 0;
        }

        if (Mathf.Abs(TargetDistance.x) < stopDistance) { forcaH = 0; }

        MoveHandler(forcaH, forcaZ);

        bool attack = Mathf.Abs(TargetDistance.x) < 1.5f && Mathf.Abs(TargetDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }

        rb.position = new Vector3(
            rb.position.x,
            rb.position.y,
            Mathf.Clamp(rb.position.z, -3.8f, 2.3f));

    }

    public void Recover() { return; } //damaged = damaged; }

    public void Destroy() { return; }

    /// <summary>
    /// Caso morto e colidindo com piso, destruir
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        GetComponent<CapsuleCollider>().enabled = false;
        anim.Play("Die");
        Destroy(gameObject, 2.525f);
        Destroy(HB, .525f);
        rb.isKinematic = true;
    }

    public void ShowLifeBar() => HB.SetActive(true);

    protected override void OnHit(int damage) {
        HealthBar();
        HB.GetComponentInChildren<HealthBar>().Hurt(damage);
        walkTimer = 0;
    }
    //protected override void OnRecover(int damage) { GetComponentInChildren<HealthBar>().Recover(damage); }

}