using System.Collections;
using UnityEngine;

/*
    Leandro Peres, ABR de 2019
    Jorge2.0, beta.2
*/

public class Player : CharacterBase {
    private Transform EnemyChecker;
    public PlayerUI pui;
    public OthersManager others;
    public int combo;
    public bool IsJumping => !OnGround;
    public bool IsKicking = false;
    private float minWidth;
    private HealthBar PHB; //player health bar
    public GameObject bosses;

    private void Start() {
        PHB = GameObject.Find("HPB").GetComponent<HealthBar>();
        PHB.MaxHealthPoints = maxHealth;
        minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 20)).x;
    }

    private void Update() {
        if (Input.GetButtonDown("Jump") && OnGround) { Jump(); }
        if (Input.GetButtonDown("Fire1")) {
            Attack();
            IsKicking = !OnGround;
        }
    }

    private void FixedUpdate() {
        SpaceLimiter();

        if (isDead || damaged) { return; }

        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveHandler(move);
        anim.SetBool("Input", !move.x.Equals(0) || !move.y.Equals(0));
    }

    public void PlayerRespawn() {
        if (gm.lives < 1) { gm.ChangeScreen(); return; }

        GetComponent<CapsuleCollider>().enabled = true;
        rb.isKinematic = false;

        gm.lives--;
        pui.UpdateLifes();

        PHB.Recover(maxHealth);
        currentHealth = maxHealth;

        transform.position = new Vector3(minWidth, 10, -4);
        transform.rotation = Quaternion.identity;

        isDead = false;
        damaged = false;

        anim.Rebind();
        anim.Play("Jumping");
    }

    protected override void OnDamage(int damage) {
        PHB.Hurt(damage);
        pui.ComboOut();
    }

    /// <summary>
    /// Quando um ataque for bem sucedido
    /// </summary>
    public override void OnValidAttack() {
        combo++;
        pui.comboTimer = 0;

        if (combo < 3) { return; }
        pui.ComboIn();
    }

    protected override void OnRecover(int points) => PHB.Recover(points);

    protected override void OnDeath() {
        if (bosses.activeInHierarchy) {
            others.PushAll();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        GetComponent<CapsuleCollider>().enabled = false;
        rb.isKinematic = true;
    }
}