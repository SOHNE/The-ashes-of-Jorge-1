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
    public bool IsMaxLife => HP.Equals(maxHealth);
    public bool IsKicking = false;
    private float minWidth_res;
    private HealthBar PHB; //player health bar
    public GameObject bosses;

    private void Start() {
        PHB = GameObject.Find("HPB").GetComponent<HealthBar>();
        PHB.MaxHealthPoints = maxHealth;

        minWidth_res = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 20)).x;
    }

    private void Update() {
        if (Input.GetButtonDown("Jump") && OnGround) { Jump(); }
        if (Input.GetButtonDown("Fire1")) {
            Attack();
            IsKicking = !OnGround;
        }

        //if (Input.GetKeyDown(KeyCode.R)) { Recover(50); }
    }

    private void FixedUpdate() {
        SpaceLimiter();

        if (isDead || damaged) { return; }

        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        MoveHandler(move);
        anim.SetBool("Input", move.x != 0 || move.y != 0);
    }

    public void PlayerRespawn() {
        if (gm.lives < 1) { gm.ChangeScreen(0); return; }

        gameObject.layer = 8; // Layer now is player

        GetComponent<CapsuleCollider>().enabled = true;
        rb.isKinematic = false;

        anim.Rebind();

        gm.lives--;
        pui.UpdateLifes();

        currentHealth = maxHealth;
        PHB.Recover(maxHealth);

        transform.position = new Vector3(minWidth_res, 10, 0);
        transform.rotation = Quaternion.identity;

        isDead = false;
        damaged = false;

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

    }

    private void OnCollisionEnter(Collision other) {
        if (!isDead || !other.gameObject.name.Equals("Ground")) { return; }

        GetComponent<CapsuleCollider>().enabled = false;
        rb.isKinematic = true;
    }
}