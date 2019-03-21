using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The main Character class
/// Contains all methods for performing the basic existence
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[System.Serializable]
public class CharacterBase : MonoBehaviour
{
    [Header("Character")]
    public bool isPlayer;
    public float maxSpeed;
    public float damageTime = 0.5f;
    public int maxHealth;
    public float jumpForce = 250f;
    public float fallMultiplier = 2.5f;
    public float attackRate = 1f;

    public int currentHealth;
    protected float currentSpeed;
    protected bool jump;
    protected Rigidbody rb;
    protected GameManager gManager;
    protected Animator anim;
    protected Transform EnemyLimiter;
    protected int EnemyCounter;
    protected Transform groundCheck;
    protected bool facingRight;
    protected Transform target;
    protected bool isDead = false;

    protected float forcaZ;
    protected float walkTimer;
    public float stopDistance = 1.0f;

    protected bool damaged = false;
    protected float damageTimer;
    protected float nextAttack;
    protected new AudioSource audio;

    protected SpriteRenderer spr;

    protected bool OnGround => Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
    protected Vector3 TargetDistance => target.position - transform.position;

    public void Awake()
    /// <summary>
    /// Function called when machine is created.
    /// get game components
    /// </summary>
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        gManager = FindObjectOfType<GameManager>();
        groundCheck = transform.Find("GroundCheck");
        currentHealth = maxHealth;
        currentSpeed = maxSpeed;
        spr = GetComponent<SpriteRenderer>();

        if (!isPlayer)
        {
            target = FindObjectOfType<Player>().transform;
            facingRight = TargetDistance.x > 0;
        }
        else
        {
            EnemyLimiter = transform.Find("EnemyLimiter");
            facingRight = true;
        }
    }

    public void TookDamage(int damage)
    /// <summary>
    /// Function called when character get damage.
    /// </summary>
    {
        if (!isDead)
        {
            damaged = true;
            currentHealth -= damage;
            //anim.SetTrigger("HitDamage");

            if (currentHealth <= 0)
            {
                isDead = true;
                rb.AddRelativeForce(new Vector3(3, 5, 0), ForceMode.Impulse);
                anim.SetBool("Dead", true);
                Destroy(gameObject, 3f);
            }
        }
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
        nextAttack = Time.time + attackRate;
    }

    public void Flip()
    /// <summary>
    /// Function called when character face another direction.
    /// </summary>
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void PlaySong(AudioClip clip)
    /// <summary>
    /// Function for audioclip execution
    /// </summary>
    {
        audio.clip = clip;
        audio.Play();
    }

    protected void Jump()
    /// <summary>
    /// Jumps the character.
    /// </summary>
    {
        rb.AddForce(Vector3.up * jumpForce);
    }

    protected void JumpControl()
    /// <summary>
    /// Function called when character is in jump state.
    /// Helps fall faster.
    /// </summary>
    {
        rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
    }

    protected void AnimSpeed()
    /// <summary>
    /// Function called for update anim parameters.
    /// </summary>
    {
        anim.SetBool("isGround", OnGround);
        anim.SetBool("isJumping", !anim.GetBool("isGround"));
        anim.SetFloat("Speed", anim.GetBool("isGround")? Mathf.Abs(rb.velocity.magnitude) : 0);
    }

    protected void MoveHandler (float x, float z)
    /// <summary>
    /// Function called when character needs to move.
    /// Gets X and Z floats and apply in the world.
    /// </summary>
    {
        x *= currentSpeed;
        z *= currentSpeed;
        rb.velocity = new Vector3(x, rb.velocity.y, z);

        if (x > 0 && !facingRight)
        {
            Flip();
        }
        else if (x < 0 && facingRight)
        {
            Flip();
        }

        // if (rb.velocity.y < 0)
        if (anim.GetBool("isJumping"))
            JumpControl();

        AnimSpeed();

        // Limita a movimentação do eixo x do personagem para apenas o mundo visivel pela câmera
        float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
        float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;

        rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + .25f, maxWidth - .25f),
            rb.position.y, rb.position.z);
    }

    protected void DisableCharacter ()
    {
        Destroy(gameObject);
    }


    /* 
     * Animator event
    */
     public void StayDead()
    {
        anim.SetBool("StillDead", true);
        anim.SetBool("Dead", false);
    }
}
