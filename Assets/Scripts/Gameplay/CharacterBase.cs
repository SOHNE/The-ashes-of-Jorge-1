using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main Character class
/// Contains all methods for performing the basic existence
/// </summary>

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class CharacterBase : MonoBehaviour {

    #region Vars
    [Header("Character")]
    public float maxSpeed = 8f;
    public float damageTime = 0.5f;
    public int maxHealth = 10;
    public int damage = 1; // TODO: alterar permissões
    public float jumpForce = 250f;
    public float fallMultiplier = 2.5f;
    public float attackRate = 1f;

    [SerializeField]
    protected int currentHealth;

    public int HP => currentHealth;

    public float currentSpeed;
    protected bool jump;
    protected Rigidbody rb;
    protected GameManager gm;
    protected Animator anim;
    public Animator anim_ {
        get => anim;
    }

    protected Transform groundCheck;
    protected bool facingRight = true;
    protected bool isDead = false;

    public bool damaged = false;
    protected float damageTimer;
    protected float nextAttack;
    protected new AudioSource audio;

    protected float m_GroundCheckDistance = .125f;
    protected Vector3 m_GroundNormal;
    protected bool m_IsGrounded;

    protected bool OnGround;

    #endregion

    #region Code

    /// <summary>
    /// Function called when machine is created.
    /// get game components
    /// </summary>
    public void Awake() {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        gm = FindObjectOfType<GameManager>();

        groundCheck = transform.Find("GroundCheck");
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;
    }

    public void Recover(int points = 1) {
        currentHealth += points;
        if (currentHealth > maxHealth) { currentHealth = maxHealth; };

        OnRecover(points);
    }

    /// <summary>
    /// Function called when character get damage.
    /// </summary>
    public void TookDamage(int damage) {
        if (isDead || damaged) { return; }
        OnHit(damage);

        currentSpeed = 0;
        rb.velocity = Vector3.zero;
        nextAttack = Time.time + attackRate;
        damaged = true;

        currentHealth -= damage;

        anim.Play("Damaged");
        PlaySong(Resources.Load<AudioClip>("SFX/punch"));

        /*
                if (gameObject.CompareTag("Enemy")) {
                    FindObjectOfType<UIManager>().EnemyUpdate(maxHealth, currentHealth);
                }
        */
        if (currentHealth > 0) { return; }
        if (currentHealth < 0) { currentHealth = 0; }

        PlaySong(Resources.Load<AudioClip>("SFX/dying"));

        //anim.SetTrigger("Fall");
        anim.Play("Fall");

        isDead = true;

        rb.AddRelativeForce(new Vector3(-4.25f, 3.5f, 0), ForceMode.Impulse);
    }

    public void Attack() {
        if (Time.time < nextAttack || damaged) { return; }

        OnAttack(damage);

        if (OnGround) { currentSpeed = 0; }

        anim.SetTrigger("Attack");
        nextAttack = Time.time + attackRate;
    }

    /// <summary>
    /// Function called when character face another direction.
    /// </summary>
    public void Flip() {
        facingRight = !facingRight;

        Quaternion rot = transform.rotation;
        rot.y = facingRight ? 0 : -180;
        transform.rotation = rot;
    }

    //
    // Summary:
    /// Function for audioclip execution
    //
    // Parameters:
    //   clip:AduioClip
    public void PlaySong(AudioClip clip) {
        audio.clip = clip;
        audio.Play();
    }

    /// <summary>
    /// Jumps the character.
    /// </summary>
    public void Jump() {
        rb.AddForce(Vector3.up * jumpForce);
        anim.SetTrigger("Jump");
    }

    /// <summary>
    /// Function called when character is in jump state.
    /// Helps fall faster.
    /// </summary>
    protected void JumpControl() {
        if (rb.velocity.y <= 0) { return; }
        rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Function called for update anim parameters.
    /// </summary>
    protected void AnimSpeed() {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));

        if (this.CompareTag("Enemy")) { return; }

        anim.SetBool("isGround", OnGround);
    }

    /// <summary>
    /// Function called when character needs to move.
    /// Gets X and Z floats and apply in the world.
    ///
    /// Parameters:
    ///   x:float
    ///   z:float
    /// </summary>
    protected void MoveHandler(float x, float z, float y = 0) {
        DamageLimiter();

        if (damaged) { return; }
        if (gameObject.CompareTag("Player")) { CheckGroundStatus(); }


        x *= currentSpeed;
        z *= OnGround ? currentSpeed : 1;
        if (float.IsNaN(z)) { z = 0; }

        rb.velocity = new Vector3(x, y, z);


        if ((x > 0 && !facingRight) || (x < 0 && facingRight)) { Flip(); }

        AnimSpeed();
        JumpControl();

        // Limita a movimentação do eixo x do personagem para apenas o mundo visivel pela câmera
        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, minWidth + .75f, maxWidth - .75f),
            rb.position.y,
            Mathf.Clamp(rb.position.z, -3.5f, 2f));
    }

    protected void DamageLimiter() {
        if (!damaged) { return; }

        damageTimer += Time.deltaTime;

        if (damageTimer < damageTime) { return; }
        damaged = false;
        anim.Play("Idle");
        damageTimer = 0;

    }

    /* 
     * Animator event
     */

    /// <summary>
    /// Function called when character stop moving
    /// </summary>
    void ZeroSpeed() => currentSpeed = 0;

    /// <summary>
    /// Function called when character speed needs a reset
    /// </summary>
    void ResetSpeed() {
        currentSpeed = maxSpeed;
        //damaged = false;
    }

    void CheckGroundStatus() {
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * .1f), transform.position + (Vector3.up * .1f) + (Vector3.down * m_GroundCheckDistance), Color.red);
#endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * .1f), Vector3.down, out RaycastHit hitInfo, m_GroundCheckDistance)) {
            m_GroundNormal = hitInfo.normal;
            OnGround = true;
        } else {
            OnGround = false;
            m_GroundNormal = Vector3.up;
        }
        //        print(m_GroundNormal);
    }

    #region Meta
    protected virtual void OnHit(int damage) { return; }
    protected virtual void OnAttack(int damage) { return; }
    protected virtual void OnRecover(int health) { return; }
    #endregion
}
#endregion