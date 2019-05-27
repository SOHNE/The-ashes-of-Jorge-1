using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBase {
    #region "Vars"
    protected float walkTimer;
    public float stopDistance = 1f;
    protected int lastDamage;

    protected float forcaZ, forcaH;
    protected Transform Target => GameObject.FindGameObjectWithTag("Player").transform;
    protected Vector3 TargetDistance => Target.position - transform.position;

    private GameObject HB;
    private bool HB_show;
    protected int posLife;

    private void HealthBar() {
        if (HB_show) { return; }
        HB_show = true;
        MountHealthBar();
    }
    #endregion

    #region "Code"
    private void Start() {
        //GetComponentInChildren<HealthBar>().MaxHealthPoints = maxHealth;
        //GetComponentInChildren<HealthBar>().Hurt(0);

        maxHealth = 3;
        //nextAttack = 3.5f;
        currentHealth = maxHealth;
    }

    private void Update() {
        if (HB) { HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0)); }

        walkTimer += Time.deltaTime;
        anim.SetBool("isDead", isDead);
    }

    void FixedUpdate() {
        if (isDead) { return; }

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

    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        GetComponent<CapsuleCollider>().enabled = false;
        rb.isKinematic = true;

        //if (posLife >= 2) { anim.SetTrigger("Destroy"); return; }

        string triggerName = "Revive"; //Random.Range(0, 6) <= 1 ? "Destroy" : "Revive";

        anim.SetTrigger(triggerName);
    }

    protected override void OnHit(int damage) {
        HealthBar();
        HB.GetComponentInChildren<HealthBar>().Hurt(damage);
        walkTimer = 0;
    }
    //protected override void OnRecover(int damage) { GetComponentInChildren<HealthBar>().Recover(damage); }
    #endregion

    #region "Animation events"
    public void Recover() { return; } //damaged = damaged; }

    public void Destroy() { Destroy(gameObject, Random.Range(1f, 3f)); Destroy(HB, .525f); }

    public void Revive() {
        posLife++;
        GetComponent<CapsuleCollider>().enabled = true;
        currentSpeed = maxSpeed;
        currentHealth = maxHealth;
        isDead = false;
        Destroy(HB);
        rb.isKinematic = false;

        MountHealthBar();
    }
    #endregion

    #region "HealthBar releted"
    public void ShowLifeBar() {
        if (HB) { return; }
        HB.SetActive(true);
    }

    private void MountHealthBar() {
        GameObject prefab = (GameObject)Resources.Load("Prefab/HealthBar");
        Transform WS = GameObject.Find("-- World Space").transform;

        HB = Instantiate(prefab, WS);
        HB.transform.SetParent(WS, true);

        HB.GetComponentInChildren<HealthBar>().MaxHealthPoints = maxHealth;
        HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));
    }
    #endregion
}