using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

/// <summary>
/// The main Character class
/// Contains all methods for performing the basic existence
/// </summary>

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[System.Serializable]
public class CharacterBase : MonoBehaviour
{
    [Header("Character")]
    public float damageTime = 0.5f;
    public int maxHP;
    public int HP = 0;
    public int Speed = 4;
    public float attackRate = 1f;
    public bool Dead = false;
    public float jumpForce = 250f;
    public float fallMultiplier = 2.5f;


    protected Transform Attack;
    protected bool jump;
    protected Rigidbody rb;
    protected Animator anim;
    protected Transform groundCheck;
    protected bool damaged = false;
    protected float damageTimer;
    protected float nextAttack;
    protected new AudioSource audio;

    protected SpriteRenderer spr;

    protected bool OnGround => Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

    public void Awake()
    /// <summary>
    /// Function called when machine is created.
    /// get game components
    /// </summary>
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        spr = GetComponent<SpriteRenderer>();

        groundCheck = transform.Find("GroundCheck");
        //Attack = transform.Find("bow");
    }

    public void TookDamage(int damage)
    /// <summary>
    /// Function called when character get damage.
    /// </summary>
    {
        if (!Dead)
        {
            damaged = true;
            HP -= damage;
            //rb.AddRelativeForce(new Vector3(10.25f, 0, 0), ForceMode.Impulse);
            Dead |= HP <= 0;
        }
    }

    public void Flip(float x)
    /// <summary>
    /// Function called when character face another direction.
    /// </summary>
    {
        if (x < 0) spr.flipX = true;
        else spr.flipX &= x <= 0;
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

    protected void MoveHandler(float x, float z)
    /// <summary>
    /// Function called when character needs to move.
    /// Gets X and Z and apply in the rigidbody.
    /// </summary>
    {
        x *= Speed;
        z *= Speed;
        rb.velocity = new Vector3(x, rb.velocity.y, z);

        Flip(x);

        AnimSpeed();

        // if (rb.velocity.y < 0)
        if (anim.GetBool("isJumping"))
            JumpControl();
    }

    protected void DisableCharacter()
    /// <summary>
    /// Called when the player needs to be deleted
    /// </summary>
    {
        Destroy(gameObject);
    }
}