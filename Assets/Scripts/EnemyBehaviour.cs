
using UnityEngine;
using System.Collections;

public abstract class EnemyBehaviour : MonoBehaviour
{
    Transform player;
    Transform enemie;
   // PlayerHealth playerHealth;
   // EnemyHealth enemyHealth;
    [Tooltip("Velocidad en la que el enemigo se mueve")]
    public float speed;

    public bool isFacingThePlayer;
    public bool isOnRange;
    public bool isAttacking;
    public bool isWalking;

    Animator enemyAnim;
    SpriteRenderer enemySprite;
    GameObject closestEnemy;
    float SightRange = 5;
    int myAction;

    public enum EnemyState
    {
        initializing,
        idle,
        sawPlayer,
        chasing,
        checkForEnemiesAndPlayer,
        attacking,
        damagedealth,
        fleeing,
        dead
    }
    /*This is the currentState of the Enemy, this is what you'll change in the child-Class*/
    public EnemyState currentState;

    public GameObject playerReference;

    void Awake()
    {
        currentState = EnemyState.initializing;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemie = GameObject.FindGameObjectWithTag("Enemy").transform;
        //playerHealth = player.GetComponent<PlayerHealth>();
        enemySprite = GetComponentInChildren<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();
    }

    public virtual void Update()
    {

        ChangeEnemySrotingLayer();

        switch (currentState)
        {
            case EnemyState.initializing:
                /*filling in the player reference for easier access*/
                playerReference = GameObject.FindGameObjectWithTag("Player");
                currentState = EnemyState.idle;
                break;
            case EnemyState.idle:
                Idle();
                break;
            case EnemyState.sawPlayer:
                SawPlayer();
                break;
            case EnemyState.chasing:
                Chasing();
                break;
            case EnemyState.checkForEnemiesAndPlayer:
                CheckForEnemiesAndplayer();
                break;
            case EnemyState.attacking:
                Attacking();
                break;
            case EnemyState.damagedealth:
                DamageDealth();
                break;
            case EnemyState.fleeing:
                Fleeing();
                break;
            case EnemyState.dead:
                Dead();
                break;
            default:
                break;
        }
    }

    /*When you add your own methods here they need to be virtual, this is so you can in override them in your own
    class*/

    public virtual void Idle()
    {
        EnemyStayCurrentPossition();
        FaceThePlayer();
        isOnRange = true;
        isWalking = false;
        isAttacking = false;
        ChangeAnimationStates();
    }
    public virtual void SawPlayer()
    {



        isOnRange = true;
        isWalking = false;
        isAttacking = false;
        ChangeAnimationStates();


    }
    public virtual void Chasing()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerReference.transform.position, speed * Time.deltaTime);


        FaceThePlayer();
        isAttacking = false;
        isOnRange = true;
        isWalking = true;
        ChangeAnimationStates();

    }
    public virtual void CheckForEnemiesAndplayer()
    {
        float distanceToPlayer = Vector2.Distance(enemie.transform.position, player.transform.position);
        if (distanceToPlayer <= SightRange)
        {
            PossitionBehind();
        }
        else
        {
            PossitionFront();
        }
    }
    public virtual void Attacking()
    {
        EnemyStayCurrentPossition();
        isAttacking = true;
        isOnRange = true;
        isWalking = false;
        ChangeAnimationStates();
    }
    public virtual void DamageDealth()
    {
        EnemyStayCurrentPossition();
        ResetBoolsToFalse();
        ChangeAnimationStates();
        enemyAnim.SetTrigger("BeenHitTrigger");
        Debug.Log("animacion de daño hecho por player");
    }
    public virtual void Fleeing()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerReference.transform.position, -speed * Time.deltaTime);
        RunAwayFromPlayer();
        isAttacking = false;
        isOnRange = false;
        isWalking = true;
        ChangeAnimationStates();

    }
    public virtual void Dead()
    {

        ResetBoolsToFalse();
        ChangeAnimationStates();
        enemyAnim.SetTrigger("Dead");
    }


    void Flip()
    {
        isFacingThePlayer = !isFacingThePlayer;
        Vector3 theScale = enemySprite.transform.localScale;
        theScale.x *= -1;
        enemySprite.transform.localScale = theScale;
    }
    void FaceThePlayer()
    {
        float localOffset = transform.localPosition.x - player.transform.localPosition.x;
        if (localOffset < 0 && !isFacingThePlayer)
        {
            Flip();
        }
        else if (localOffset > 0 && isFacingThePlayer)
        {
            Flip();
        }

    }
    void RunAwayFromPlayer()
    {
        float localOffset = transform.localPosition.x - player.transform.localPosition.x;
        if (localOffset > 0 && !isFacingThePlayer)
        {
            Flip();
        }
        else if (localOffset < 0 && isFacingThePlayer)
        {
            Flip();
        }

    }
    void ChangeEnemySrotingLayer()
    {
        enemySprite.sortingOrder = (int)(transform.position.y * -2);
    }
    void ChangeAnimationStates()
    {
        enemyAnim.SetBool("isWalking", isWalking);
        enemyAnim.SetBool("isAttacking", isAttacking);
    }
    void EnemyStayCurrentPossition()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }
    void ResetBoolsToFalse()
    {
        isAttacking = false;
        isOnRange = false;
        isWalking = false;
    }
    void PossitionFront()
    {
        enemySprite.transform.localScale.Equals(1);
    }
    void PossitionBehind()
    {
        enemySprite.transform.localScale.Equals(-1);
    }

}