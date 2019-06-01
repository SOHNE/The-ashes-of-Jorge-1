using TMPro;
using UnityEngine;

/*
    Leandro Peres, ABR de 2019
    Jorge2.0, beta.2
*/

public class Player : CharacterBase {
    private Transform EnemyChecker;
    public PlayerUI pui;
    public int combo;

    private void Start() {
        GameObject.Find("HPB").GetComponent<HealthBar>().MaxHealthPoints = maxHealth;
    }

    void Update() { }

    void FixedUpdate() {
        if (isDead) { PlayerRespawn(); }

        DamageLimiter();
        if (damaged) { return; }

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        MoveHandler(move);

        anim.SetBool("Input", !move.x.Equals(0) || !move.y.Equals(0));

        if (Input.GetButtonDown("Jump") && OnGround) { Jump(); }

        if (Input.GetButtonDown("Fire1")) { Attack(); }

        //JumpControl();
    }

    void PlayerRespawn() {
        if (FindObjectOfType<GameManager>().lives <= 0) { return; }

        anim.Rebind();
        FindObjectOfType<GameManager>().lives--;
        isDead = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GameObject.Find("HPB").GetComponent<HealthBar>().Recover(maxHealth);
        currentHealth = maxHealth;
        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        transform.position = new Vector3(minWidth, 10, -4);
    }

    protected override void OnDamage(int damage) {
        GameObject.Find("HPB").GetComponent<HealthBar>().Hurt(damage);
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

    protected override void OnRecover(int points) => GameObject.Find("HPB").GetComponent<HealthBar>().Recover(points);
}