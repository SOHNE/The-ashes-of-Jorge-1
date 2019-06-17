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
    [Header("Sounds")]
    public AudioClip Hit;
    public AudioClip Dying;

    [Header("Character")]
    protected bool isEntering;
    public float maxSpeed = 8f;
    public float damageTime = 0.5f;
    public int maxHealth = 10;
    public int damage = 1;
    protected int original_damage;
    public float jumpForce = 250f;
    public float fallMultiplier = 2.5f;
    public float attackRate = 1f;
    [SerializeField] protected int currentHealth;
    public int HP => currentHealth;
    public bool IsDead => currentHealth <= 0;
    public float currentSpeed;
    protected bool jump;
    protected Rigidbody rb;
    protected CapsuleCollider coll;
    protected GameManager gm;
    protected Animator anim;
    public Animator anim_ {
        get => anim;
    }
    protected bool facingRight = true;
    protected bool isDead = false;
    public bool damaged = false;
    protected float damageTimer;
    protected float nextAttack;
    protected new AudioSource audio;
    public bool IsPunching;
    public bool Avoiding;
    [Header("OnGround")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Vector3 bottomOffset;
    [SerializeField] protected float collisionRadius = .25f;
    [SerializeField] protected bool OnGround => Physics.OverlapSphere(transform.position + bottomOffset, collisionRadius, groundLayer).Length > 0;
    [Header("Misc")]
    [SerializeField] protected ComboManager comboManager;
    protected GameObject player;
    [SerializeField] protected LayerMask PlayerLayer = 8;
    [SerializeField] protected Vector3 BackOff = new Vector3(-1, 1, 0);
    protected bool BackCheck => Physics.OverlapSphere(transform.position + BackOff, collisionRadius * 3f, PlayerLayer).Length > 0;
    #endregion

    #region Code

    /// <summary>
    /// Function called when machine is created.
    /// get game components
    /// </summary>
    public void Awake() {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        gm = FindObjectOfType<GameManager>();
        comboManager = FindObjectOfType<ComboManager>();

        currentHealth = maxHealth;
        currentSpeed = maxSpeed;

        original_damage = damage;

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
        if (isDead || IsPunching || Avoiding || isEntering) { return; }
        if (CompareTag("Player") && damaged) { return; }

        PlaySong(Hit);
        anim.Play("Damaged");

        currentSpeed = 0;
        rb.velocity = Vector3.zero;

        nextAttack = Time.time + attackRate;

        StartCoroutine(DamageLimiter(damageTime));

        currentHealth -= damage;

        OnDamage(damage);

        if (currentHealth > 0) { return; }
        currentHealth = 0;

        PlaySong(Dying);

        Push();

        isDead = true;
        OnDeath();
        gameObject.layer = 30; // change layer to dead
    }

    public void Attack(int dmg = 0) {
        if (damaged || isDead) { return; } // or Time.time < nextAttack

        OnAttack(dmg);

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

        BackOff.x *= -1;

    }

    //
    // Summary:
    /// Function for audioclip execution
    //
    // Parameters:
    // clip:AduioClip
    public void PlaySong(AudioClip clip) {
        audio.clip = clip;
        audio.Play();
    }

    /// <summary>
    /// Jumps the character.
    /// </summary>
    public void Jump() {
        if (damaged || isDead) { return; }

        rb.AddForce(Vector3.up * jumpForce);
        PlaySong(Resources.Load<AudioClip>("SFX/felixyadomi__jump"));
        anim.SetTrigger("Jump");
    }

    public void Push(bool avoid = false) {
        if (isDead) { return; }

        //Vector3 direction = rb.transform.position - transform.position;

        //int a = !facingRight ? 1 : -1;
        rb.AddRelativeForce(new Vector3(-4.2f, 3.5f, 0), ForceMode.Impulse);

        if (avoid) { return; }
        PlaySong(Dying);

        anim.Play("Fall");
        isDead = true;
        damaged = false;

        if (CompareTag("Enemy")) { gameObject.layer = 30; }
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
    /// x:float
    /// z:float
    /// </summary>
    protected void MoveHandler(Vector2 move) {
        if ((move.x > 0 && !facingRight) || (move.x < 0 && facingRight)) { Flip(); }

        move.x *= currentSpeed;
        move.y = float.IsNaN(move.y) ? 0 : (OnGround ? move.y * currentSpeed : move.y);
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.y);

        AnimSpeed();
        JumpControl();
    }

    protected void SpaceLimiter() {
        float LimitX;

        if (CompareTag("Player")) {
            // Limita a movimentação do eixo x do personagem para apenas o mundo visivel pela câmera
            var minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            var maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

            LimitX = Mathf.Clamp(rb.position.x, minWidth + .75f, maxWidth - .75f);

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
    public void ZeroSpeed() {
        if (isEntering) { PlaySong(Resources.Load<AudioClip>("SFX/thunder")); } else if (gameObject.layer == 30) { gameObject.layer = 12; }

        currentSpeed = 0;
    }

    /// <summary>
    /// Function called when character speed needs a reset
    /// </summary>
    public void ResetSpeed() { currentSpeed = maxSpeed; IsPunching = false; Avoiding = false; }

    public void Punch() { IsPunching = true; Avoiding = false; }
    public void PunchToAvoid() { IsPunching = true; Avoiding = true; }

    IEnumerator DamageLimiter(float time) {
        damaged = true;
        yield return new WaitForSeconds(time);

        if (HP > 0 && damaged) {
            damaged = false;
            anim.Play("Idle");
            currentSpeed = maxSpeed;
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

    #region Editor
    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere(transform.position + BackOff, collisionRadius * 1.5f);
    }
    #endregion
}
#endregion