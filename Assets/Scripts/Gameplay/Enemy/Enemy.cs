using System.Collections;
using UnityEngine;

public class Enemy : CharacterBase {
    #region "Vars"
    [Header("Misc")]
    public float standDelay = .525f;
    [Tooltip("A adição na posição do Health Bar para ficar no topo da cabeça")]
    public Vector3 HBOffset = new Vector3(0, 3, 0);
    [SerializeField] protected int PosLife = 2;
    [SerializeField] private float ReviveProb = .2f;
    protected float walkTimer;
    public float stopDistance = 1f;
    private float _stopDistance;
    public Vector3 PlayerDistance => player.transform.position - transform.position;
    protected GameObject HB;
    protected HealthBar EHB; // Enemy Health bar
    private bool HB_show;
    public bool Attacking;
    #endregion

    #region "Code"
    private void Start() {
        //num = Random.Range(0, 2);
        currentHealth = maxHealth;
        _stopDistance = stopDistance;
    }

    private void Update() => AirUpdate();

    private void FixedUpdate() {
        SpaceLimiter();

        if (isDead || damaged) { return; }

        BasicMove();

        if (!Attacking) { return; }
        PointPlayer();
        BasicAttack();
    }

    private void HealthBar() {
        if (HB_show) { return; }
        HB_show = true;
        MountHealthBar();
    }

    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        coll.enabled = false;
        rb.isKinematic = true;

        string triggerName = HP > 0 ? "Revive" : Random.value <= ReviveProb && PosLife > 0 ? "Revive" : "Destroy";

        anim.SetTrigger(triggerName);
    }

    protected override void OnDamage(int damage) {
        HealthBar();
        EHB.Hurt(damage);
        walkTimer = 0;
    }
    //protected override void OnRecover(int damage) { GetComponentInChildren<HealthBar>().Recover(damage); }
    #endregion

    #region "Animation events"
    public void Recover() { return; } //damaged = damaged; }
    public void AnimReset() { anim.Rebind(); }
    public void Destroy() { Destroy(gameObject, Random.Range(1f, 3f)); Destroy(HB, .525f); comboManager.CalcDeaths(); }

    public void Revive() {
        if (PosLife <= 0) { anim.SetTrigger("Destroy"); }

        gameObject.layer = 12; // layer now is Enemy

        //anim.Rebind();
        coll.enabled = true;
        currentSpeed = maxSpeed;
        isDead = false;
        rb.isKinematic = false;
        damaged = false;
        IsPunching = false;
        Avoiding = false;

        if (HP <= 0) {
            PosLife--;
            currentHealth = maxHealth;
            if (HB) { EHB.Recover(maxHealth); }
        }

        OnRevive();
    }
    #endregion

    #region "HealthBar releted"
    public virtual void ShowLifeBar() {
        if (HB) { return; }
        HB.SetActive(true);
    }

    /// <summary>
    /// Calculations for HB positioning
    /// </summary>
    protected void PositionHB() {
        if (!HB) { return; }
        HB.transform.position = Camera.main.WorldToScreenPoint(transform.position + HBOffset);
    }

    protected void MountHealthBar() {
        GameObject prefab = Resources.Load<GameObject>("Prefab/HealthBar");
        Transform WS = GameObject.Find("-- World Space").transform;

        HB = Instantiate(prefab, WS);
        HB.transform.SetParent(WS, true);

        EHB = HB.GetComponentInChildren<HealthBar>();

        EHB.MaxHealthPoints = maxHealth;
        PositionHB();
    }

    IEnumerator Wait(float time) { yield return new WaitForSeconds(time); }
    #endregion

    #region "Basic"
    /* protected virtual void BasicMove() {
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
    protected virtual void BasicMove() {
        Vector2 move = default;

        move.x = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);

        if (!Attacking) {
            if (walkTimer <= Random.Range(1f, 2f)) { return; }

            move.y = Random.Range(-1, 2);

            float point = Camera.main.WorldToScreenPoint(transform.position).x;
            if (point > 40 && point < 900) {
                move.x = Random.Range(-1, 2);
            }
            walkTimer = 0;
        } else {
            move.y = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);
        }

        if (Mathf.Abs(PlayerDistance.x) < stopDistance) { move.x = 0; }

        MoveHandler(move);
    }
    protected virtual void BasicAttack() {
        if (Time.time <= nextAttack) { return; }

        bool attack = Mathf.Abs(PlayerDistance.x) < 1.5f && Mathf.Abs(PlayerDistance.z) < 1f;
        if (attack) { PointPlayer(); Attack(); }
    }

    protected void PointPlayer() {
        if (BackCheck) {
            Flip();
        }
    }

    protected virtual void AirUpdate() {
        walkTimer += Time.deltaTime;
        anim.SetBool("isDead", isDead);

        PositionHB();
    }
    #endregion
}