using UnityEngine;

public class Enemy : CharacterBase {
    #region "Vars"

    [SerializeField] protected int PosLife = 2;
    [SerializeField] private int num;
    [SerializeField] private float ReviveProb = .2f;
    protected float walkTimer;
    public float stopDistance = 1f;
    protected int lastDamage;
    public Vector3 PlayerDistance => player.transform.position - transform.position;
    protected GameObject HB;
    private bool HB_show;
    public bool Attacking;

    private void HealthBar() {
        if (HB_show) { return; }
        HB_show = true;
        MountHealthBar();
    }
    #endregion

    #region "Code"
    private void Start() {
        //num = Random.Range(0, 2);
        currentHealth = maxHealth;
    }

    private void Update() => AirUpdate();

    private void FixedUpdate() {
        if (isDead) { return; }

        BasicMove();

        if (!Attacking) { return; }

        BasicAttack();
    }

    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        GetComponent<CapsuleCollider>().enabled = false;
        rb.isKinematic = true;

        if (PosLife >= 2) { anim.SetTrigger("Destroy"); return; }

        string triggerName = Random.value <= ReviveProb ? "Revive" : "Destroy";

        anim.SetTrigger(triggerName);
    }

    protected override void OnDamage(int damage) {
        HealthBar();
        HB.GetComponentInChildren<HealthBar>().Hurt(damage);
        walkTimer = 0;
    }
    //protected override void OnRecover(int damage) { GetComponentInChildren<HealthBar>().Recover(damage); }
    #endregion

    #region "Animation events"
    public void Recover() { return; } //damaged = damaged; }
    public void AnimReset() { anim.Rebind(); }
    public void Destroy() { Destroy(gameObject, Random.Range(1f, 3f)); Destroy(HB, .525f); }

    public void Revive() {
        OnRevive();

        if (CompareTag("Enemy")) { gameObject.layer = 12; }

        PosLife--;
        anim.Rebind();
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

    #region "Basic"
    /*    protected virtual void BasicMove() {
            Vector2 move = default;

            if (Attacking) {
                move.x = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);
                move.y = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);

                if (Mathf.Abs(PlayerDistance.x) <= stopDistance) { move.x = 0; }

            } else {
                if (PlayerDistance.sqrMagnitude < 100 && walkTimer > Random.Range(1f, 2f)) {
                    move.y = Random.Range(-1, 2);
                    walkTimer = 0;

                } else if (PlayerDistance.sqrMagnitude >= 100) {
                    move.x = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);
                }
            }
            MoveHandler(move);
        }
        */
    protected virtual void BasicMove()
    {
        Vector2 move = default;

        move.x = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);

        if (!Attacking)
        {
            if (walkTimer <= Random.Range(1f, 2f)) { return; }

            move.y = Random.Range(-1, 2);
            if (Camera.main.WorldToScreenPoint(transform.position).x > 40 && Camera.main.WorldToScreenPoint(transform.position).x < 900)
            {
                move.x = Random.Range(-1, 2);
            }
            walkTimer = 0;
        }
        else
        {
            move.y = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);
        }

        if (Mathf.Abs(PlayerDistance.x) < stopDistance) { move.x = 0; }

        MoveHandler(move);
    }
    protected virtual void BasicAttack() {
        bool attack = Mathf.Abs(PlayerDistance.x) < 1.5f && Mathf.Abs(PlayerDistance.z) < 1f;
        if (attack && Time.time > nextAttack) {
            Attack();
        }
    }

    protected virtual void AirUpdate() {
        walkTimer += Time.deltaTime;
        anim.SetBool("isDead", isDead);

        if (!HB) { return; }
        HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3, 0));
    }
    #endregion
}