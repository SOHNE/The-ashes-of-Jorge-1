using UnityEngine;

public class Enemy : CharacterBase {
    #region "Vars"
    protected float walkTimer;
    public float stopDistance = 1f;
    protected int lastDamage;

    protected float forcaZ, forcaH;
    protected Transform Target => GameObject.FindGameObjectWithTag("Player").transform;
    public Vector3 TargetDistance => Target.position - transform.position;

    protected GameObject HB;
    private bool HB_show;
    protected int posLife;

    public bool Attacking;

    private void HealthBar() {
        if (HB_show) { return; }
        HB_show = true;
        MountHealthBar();
    }
    #endregion

    #region "Code"
    private void Start() {
        //Target = GameObject.FindGameObjectWithTag("Player").transform;//GameObject.FindGameObjectsWithTag("EnemyPoints")[Random.Range(0, 2)].transform;

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

        if (!Attacking) {
            if (walkTimer > Random.Range(1f, 2f)) {
                forcaZ = Random.Range(-1, 2);
                forcaH = Random.Range(-2, 3);
                walkTimer = 0;
            }
        } else {
            forcaZ = TargetDistance.z / Mathf.Abs(TargetDistance.z);
        }

        if (float.IsNaN(forcaZ)) { forcaZ = 0; }

        if (Mathf.Abs(TargetDistance.x) < stopDistance) { forcaH = 0; }
        //if (Mathf.Abs(TargetDistance.z) < stopDistance) { forcaZ = 0; }

        MoveHandler(forcaH, forcaZ);

        if (!Attacking) { return; }

        bool attack = Mathf.Abs(TargetDistance.x) < 1.5f && Mathf.Abs(TargetDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        GetComponent<CapsuleCollider>().enabled = false;
        rb.isKinematic = true;

        //if (posLife >= 2) { anim.SetTrigger("Destroy"); return; }

        string triggerName = Random.Range(0, 6) <= 4 ? "Destroy" : "Revive";

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

    protected void MountHealthBar() {
        GameObject prefab = (GameObject)Resources.Load("Prefab/HealthBar");
        Transform WS = GameObject.Find("-- World Space").transform;

        HB = Instantiate(prefab, WS);
        HB.transform.SetParent(WS, true);

        HB.GetComponentInChildren<HealthBar>().MaxHealthPoints = maxHealth;
        HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));
    }
    #endregion
}