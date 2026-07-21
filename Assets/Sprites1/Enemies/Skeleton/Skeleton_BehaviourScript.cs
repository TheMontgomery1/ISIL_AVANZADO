using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skeleton_Behaviour : MonoBehaviour
{
    public Transform player;
    public float detection_radius = 5.0f;
    public float speed = 2.0f;
    float life = 20;
    private bool dead;
    private bool playerAlive;

    bool canMove = true;
    public Rigidbody2D rb_skeleton;
    private float movementX;

    private bool getDamage;
    public float rebote_force;

    Animator anim_skeleton;
    private SpriteRenderer sr;
    public GameManager game_manager;



    void Start()
    {
        rb_skeleton = GetComponent<Rigidbody2D>();
        anim_skeleton = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {   
        if (!canMove) return;
        playerAlive = player.GetComponent<PlayerBehaviour>().isAlive;
        if (playerAlive && !dead)
        {
            move();  
        }
        else
        {
            movementX = 0;
        }


    }

    private void move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detection_radius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movementX = direction.x;

        }
        else
        {
            movementX = 0;
        }

        SetLookDirection(movementX);

        anim_skeleton.SetBool("hit_skeleton", getDamage);

        void SetLookDirection(float movement)
        {
            if (movement > 0)
            {
                sr.flipX = false;
            }
            else if (movement < 0)
            {
                sr.flipX = true;
            }

        }
    }

    void FixedUpdate()
    {
        if (!getDamage) 
        {
            rb_skeleton.linearVelocity = new Vector2(movementX * speed, rb_skeleton.linearVelocity.y);
        }
            anim_skeleton.SetBool("move_skeleton", Mathf.Abs(movementX) > 0.01f);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 directionDamage = new Vector2(transform.position.x, 0);
            PlayerBehaviour playerScript = collision.gameObject.GetComponent<PlayerBehaviour>();
            playerScript.take_damage(directionDamage, 20);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_sword"))
        {
            Vector2 directionDamage = new Vector2(collision.gameObject.transform.position.x, 0);
            take_damage(directionDamage, 10);
            GetComponent<skeleton_SFX>().PlaySkeletonHit();
        }
    }

    public void take_damage(Vector2 direction, int damageAmount)
    {
        if (!getDamage)
        {
            life -= damageAmount;
            getDamage = true;
            if (life <=0)
            {
                rb_skeleton.linearVelocity = new Vector2(0, rb_skeleton.linearVelocity.y);
                canMove = false;
                dead = true;
                movementX = 0;
                rb_skeleton.linearVelocity = Vector2.zero;

                anim_skeleton.SetBool("dead_skeleton", true);
            }
            else
            {
                rb_skeleton.linearVelocity = new Vector2(0, rb_skeleton.linearVelocity.y);
                Vector2 rebote = new Vector2(transform.position.x - direction.x, 1).normalized;
                rb_skeleton.AddForce(rebote * rebote_force, ForceMode2D.Impulse);
            }
               
        }
    }

    public void RecoverFromDamage()
    {
        getDamage = false;
        rb_skeleton.linearVelocity = Vector2.zero;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detection_radius);
    }
}