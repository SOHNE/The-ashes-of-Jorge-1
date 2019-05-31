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
    public float comboTimer;

    private void Start() {
        GameObject.Find("HPB").GetComponent<HealthBar>().MaxHealthPoints = maxHealth;
    }

    void Update() {
        comboTimer += Time.deltaTime;
        if (!combo.Equals(0) && comboTimer >= 3f) { pui.ComboOut(); }
        GameObject.Find("Combo").GetComponentInChildren<TextMeshProUGUI>().text = "COMBO:\n\t" + combo.ToString();
    }

    void FixedUpdate() {
        if (isDead) { PlayerRespawn(); }

        DamageLimiter();
        if (damaged) { return; }

        Vector2 Inp = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        MoveHandler(Inp.x, Inp.y, rb.velocity.y);

        anim.SetBool("Input", !Inp.x.Equals(0) || !Inp.y.Equals(0));

        if (Input.GetButtonDown("Jump") && OnGround) { Jump(); }

        if (Input.GetButtonDown("Fire1") && !OnGround) { Attack(); }

        //JumpControl();
    }

    void PlayerRespawn() {
        if (FindObjectOfType<GameManager>().lives <= 0) { return; }

        FindObjectOfType<GameManager>().lives--;
        anim.Rebind();
        isDead = false;
        GetComponent<CapsuleCollider>().enabled = true;
        GameObject.Find("HPB").GetComponent<HealthBar>().Recover(maxHealth);
        currentHealth = maxHealth;
        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        transform.position = new Vector3(minWidth, 10, -4);
    }

    public void AttackComplete() {
        ComboCounter();

        if (combo < 3) { return; }
        pui.ComboIn();

    }

    protected override void OnDamage(int damage) {
        GameObject.Find("HPB").GetComponent<HealthBar>().Hurt(damage);
        pui.ComboOut();
    }

    protected override void OnAttack(int damage) {
        //ComboCounter();
    }

    protected override void OnRecover(int points) {
        GameObject.Find("HPB").GetComponent<HealthBar>().Recover(points);
    }

    protected void ComboCounter() {
        combo++;
        comboTimer = 0;
    }

    public void ComboReset() {

    }

}