using TMPro;
using UnityEngine;

/*
    Leandro Peres, ABR de 2019
    Jorge2.0, beta.2
*/

public class Player : CharacterBase {
    private Transform EnemyChecker;
    [Header("Misc")]
    public PlayerUI pui;
    public int combo;
    public bool IsJumping => !OnGround;
    public bool IsFallingInRes;

    private void Start() {
        GameObject.Find("HPB").GetComponent<HealthBar>().MaxHealthPoints = maxHealth;
    }

    private void Update() {
        if (!IsFallingInRes) { return; }
        if (!OnGround) { return; } else { IsFallingInRes = false; } 
    }

    void FixedUpdate() {
        if (IsFallingInRes) { return; }

        if (isDead) { PlayerRespawn(); }
        if (damaged) { return; }

        Vector2 move = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        MoveHandler(move);

        anim.SetBool("Input", !move.x.Equals(0) || !move.y.Equals(0));

        if (Input.GetButtonDown("Jump") && OnGround) { Jump(); }

        if (Input.GetButtonDown("Fire1")) { Attack(); }

        //JumpControl();
    }

    void PlayerRespawn() {
        if (FindObjectOfType<GameManager>().lives < 1) { return; }

        FindObjectOfType<GameManager>().lives--;
        pui.UpdateLifes();
        anim.Rebind();
        isDead = false;
        damaged = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GameObject.Find("HPB").GetComponent<HealthBar>().Recover(maxHealth);
        currentHealth = maxHealth;

        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 20)).x;
        transform.position = new Vector3(minWidth, 10, -4);
        transform.rotation = Quaternion.identity;

        anim.Play("Jumping");
        IsFallingInRes = true;
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