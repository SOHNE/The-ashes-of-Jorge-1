using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    Leandro Peres, ABR de 2019
    Jorge2.0, beta.2
*/

public class Player : CharacterBase {
    private Transform EnemyChecker;
    private HealthBar P_UI;
    public int combo;
    public float comboTimer;

    private void Start() {
        GameObject.Find("HPB").GetComponent<HealthBar>().MaxHealthPoints = maxHealth;
        //P_UI = GameObject.Find("HPB").GetComponent<HealthBar>();
    }

    void Update() {
        comboTimer += Time.deltaTime;
        //if (!combo.Equals(0) && comboTimer >= 3f) { GameObject.FindObjectOfType<UIManager>().ComboOut(); }
        GameObject.Find("Combo").GetComponentInChildren<TextMeshProUGUI>().text = "COMBO:\r\n\t" + combo.ToString();
    }

    void FixedUpdate() {
        if (isDead) { PlayerRespawn(); }
        DamageLimiter();
        if (damaged) { return; }

        Vector3 I = new Vector3(Input.GetAxis("Horizontal"), rb.velocity.y, Input.GetAxis("Vertical"));

        MoveHandler(I.x, I.z, I.y);

        anim.SetBool("Input", !I.x.Equals(0) || !I.z.Equals(0));

        if (Input.GetButtonDown("Jump") && OnGround) { Jump(); }

        if (Input.GetButtonDown("Fire1")) { Attack(); }

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
        //GameObject.FindObjectOfType<UIManager>().ComboIn();

    }

    protected override void OnHit(int damage) {
        GameObject.Find("HPB").GetComponent<HealthBar>().Hurt(damage);
        //GameObject.FindObjectOfType<UIManager>().ComboOut();
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