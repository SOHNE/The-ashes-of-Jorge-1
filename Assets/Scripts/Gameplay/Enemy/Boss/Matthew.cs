using System.Collections;
using UnityEngine;

public class Matthew : Enemy {

    #region Vars
    public EventHandler BossWave;
    private BossUI BUI;
    private HealthBar BHB; // Boss Health Bar

    [Header("Boss related")]
    /// <summary>
    /// 0: Default
    /// </summary>
    [SerializeField] private int MoveMode;
    [SerializeField] private int AttackMode;

    [Header("AI Control")]
    [SerializeField] private float UltChance;
    [SerializeField] private float CallChance;
    [SerializeField] private Vector2 MoveStop = new Vector2(2.25f, .8f);
    [SerializeField] private int Attacks;
    [SerializeField] private float UltAttacks;
    [SerializeField] private int Calls;
    [SerializeField] private int ConsecutiveHits;

    #endregion

    #region MonoBehaviour
    private void Start() {
        BUI = FindObjectOfType<BossUI>();
        BUI.TurnOn();

        BHB = BUI.gameObject.GetComponentInChildren<HealthBar>();
        BHB.MaxHealthPoints = maxHealth;
        Intro();
    }

    private void Intro() {
        gameObject.layer = 30;
        isEntering = true;
        StartCoroutine(Wait(2f));
    }

    IEnumerator Wait(float time) {
        yield return new WaitForSeconds(time);
        isEntering = false;
    }

    private void Update() {
        if (isDead) { return; }

        AttackMode = Random.Range(0, 2);

        if (Time.time > nextAttack) {
            ChangeAttackMode();
        }

        if (!AttackMode.Equals(2)) {
            float va = Random.value;
            if (va <= UltChance && UltAttacks == 0) {
                AttackMode = 2;
                attackRate = .57525f;
            } else {
                AttackMode = Random.Range(0, 2);
            }

        } else {
            UltChance = .05f;
            AttackMode = Random.Range(0, 2);
        }

        if (HP < maxHealth / 2) {
            if (UltChance.Equals(0)) {
                UltChance = 1;
            }
        }

        if (Random.value <= .05f) { UltAttacks -= UltAttacks > 0 ? 1 : 0; }
    }
    #endregion

    #region Move
    protected override void BasicMove() {

        Vector2 move = default;

        move.x = PlayerDistance.x / Mathf.Abs(PlayerDistance.x);
        move.y = PlayerDistance.z / Mathf.Abs(PlayerDistance.z);

        if (Mathf.Abs(PlayerDistance.x) < MoveStop.x) { move.x = 0; }
        if (Mathf.Abs(PlayerDistance.z) < MoveStop.y) { move.y = 0; }

        MoveHandler(move);
    }
    #endregion

    #region Attack
    protected override void BasicAttack() {
        if (isDead || IsPunching || Avoiding || isEntering) { return; }
        ChangeAttackMode();
        PointPlayer();
        if (Time.time <= nextAttack) { return; }

        switch (AttackMode) {
            case 0:
                AttackBase(MoveStop, original_damage);
                break;

            case 1:
                AttackBase(MoveStop, original_damage, "Attack_2");
                break;

            case 2:
                AttackBase(new Vector2(5.5f, 3.25f), 5, "Attack_3");
                UltAttacks++;
                break;
        }

    }

    /// <summary>
    /// Base calculation for attack. includes animation, attacks distance, ...
    /// </summary>
    /// <param name="stop"></param>
    /// <param name="dmg"></param>
    /// <param name="triggerName"></param>
    private void AttackBase(Vector2 stop, int dmg = default, string triggerName = "Attack") {
        if (damaged || isDead) { return; }

        bool attack = Mathf.Abs(PlayerDistance.x) < stop.x && Mathf.Abs(PlayerDistance.z) < stop.y;
        if (!attack) { return; }

        damage = dmg.Equals(0) ? original_damage : dmg;

        OnAttack(dmg);

        if (OnGround) { currentSpeed = 0; }

        anim.PlayInFixedTime(triggerName);
        nextAttack = Time.time + attackRate;
    }

    private void ChangeAttackMode() {

        switch (AttackMode) {
            case 0:
                if (Attacks >= 4) { AttackMode = 1; Attacks = 0; }
                if (ConsecutiveHits >= 6) { anim.Play("Avoiding"); ConsecutiveHits = 0; }
                break;

            case 1:
                if (Attacks >= 3) { AttackMode = 0; Attacks = 0; }
                if (ConsecutiveHits >= 6) { anim.Play("Avoiding"); ConsecutiveHits = 0; }

                break;

            case 2:
                if (ConsecutiveHits >= 2) {
                    anim.Play("Avoiding");
                    ConsecutiveHits = 0;
                }

                break;
        }

    }
    #endregion

    #region Meta
    protected override void OnDamage(int damage) {
        BHB.Hurt(damage);
        ConsecutiveHits++;
        walkTimer = 0;
    }

    protected override void OnDeath() {
        BUI.gameObject.SetActive(false);
        BossWave.Desblo();
    }

    public override void OnValidAttack() {
        ConsecutiveHits = 0;
        Attacks++;
        if (AttackMode == 2) { UltChance = .025f; AttackMode = Random.Range(0, 2); }
    }

    public override void ShowLifeBar() { return; }

    protected override void OnRevive() {

    }
    #endregion
}