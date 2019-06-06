using System.Collections;
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
    public int damage = 1;
    public float jumpForce = 250f;
    public float fallMultiplier = 2.5f;
    public float attackRate = 1f;
    [SerializeField] protected int currentHealth;
    public int HP => currentHealth;
    public bool IsDead => currentHealth <= 0;
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

    [Header("OnGround")]
    public LayerMask groundLayer;
    protected Vector3 bottomOffset;
    public float collisionRadius = 0.25f;
    protected bool OnGround => Physics.OverlapSphere(transform.position + bottomOffset, collisionRadius, groundLayer).Length > 0;
    protected GameObject player;
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

        if (CompareTag("Enemy")) { player = GameObject.FindGameObjectWithTag("Player"); }
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
        if (isDead) { return; }
        if (CompareTag("Player") && damaged) { return; }

        PlaySong(Resources.Load<AudioClip>("SFX/punch"));
        anim.Play("Damaged");

        OnDamage(damage);

        currentSpeed = 0;
        rb.velocity = Vector3.zero;

        nextAttack = Time.time + attackRate;

        StartCoroutine(DamageLimiter(damageTime));

        currentHealth -= damage;    

        if (currentHealth > 0) { return; }
        if (currentHealth < 0) { currentHealth = 0; }

        PlaySong(Resources.Load<AudioClip>("SFX/dying"));

        //anim.SetTrigger("Fall");
        anim.Play("Fall");

        isDead = true;
        if (CompareTag("Enemy")) {
            rb.AddRelativeForce(new Vector3(-4.25f, 3.5f, 0), ForceMode.Impulse);
            gameObject.layer = 30;
        }
    }

    public void Attack(bool kick = false) {
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
        PlaySong(Resources.Load<AudioClip>("SFX/felixyadomi__jump"));
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

        if (CompareTag("Enemy")) { return; }

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
    protected void MoveHandler(Vector2 move) {
        if (damaged) { return; }

        if ((move.x > 0 && !facingRight) || (move.x < 0 && facingRight)) { Flip(); }

        move.x *= currentSpeed;
        move.y = float.IsNaN(move.y) ? 0 : (OnGround ? move.y * currentSpeed : move.y);
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.y);

        AnimSpeed();
        JumpControl();

        float LimitX;

        if (CompareTag("Player")) {

            // Limita a movimentação do eixo x do personagem para apenas o mundo visivel pela câmera
            var minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            var maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

            LimitX = Mathf.Clamp(rb.position.x, minWidth + .75f, maxWidth - .75f);
            //CheckGroundStatus();

        } else {
            LimitX = rb.position.x;
        }

        rb.position = new Vector3(
            LimitX,
            rb.position.y,
            Mathf.Clamp(rb.position.z, -8.75f, 7.35f));
    }

    /* 
     * Animator event
     */

    /// <summary>
    /// Function called when character stop moving
    /// </summary>
    public void ZeroSpeed() => currentSpeed = 0;

    /// <summary>
    /// Function called when character speed needs a reset
    /// </summary>
    public void ResetSpeed() => currentSpeed = maxSpeed;

    IEnumerator DamageLimiter(float time) {
        damaged = true;
        yield return new WaitForSeconds(time);

        if (HP > 0 && damaged) {
            damaged = false;
            anim.Play("Idle");
        }
    }

    #region Meta
    protected virtual void OnDamage(int damage) { return; }
    protected virtual void OnAttack(int damage) { return; }
    public virtual void OnValidAttack() { return; } // Called in Attack.cs
    protected virtual void OnRecover(int health) { return; }
    protected virtual void OnRevive() { return; }
    protected virtual void OnDeath() { return; }
    #endregion

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + bottomOffset, collisionRadius);
    }
}
#endregion