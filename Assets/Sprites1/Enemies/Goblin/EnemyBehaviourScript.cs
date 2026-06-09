using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public Transform player;
    public float detection_radius = 5.0f;
    public float speed = 2.0f;
    float life = 30;
    private bool dead;
    private bool playerAlive;

    bool canMove = true;
    public Rigidbody2D rb_goblin;
    private Vector2 movement;

    private bool getDamage;
    public float rebote_force;

    Animator anim_goblin;
    private SpriteRenderer sr;
    public GameManager game_manager;



    void Start()
    {
        rb_goblin = GetComponent<Rigidbody2D>();
        anim_goblin = GetComponent<Animator>();
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
            movement = Vector2.zero;
        }


    }

    private void move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detection_radius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, 0);

        }
        else
        {
            movement = Vector2.zero;
        }

        SetLookDirection(movement.x);

        anim_goblin.SetBool("hit_gob", getDamage);

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
            rb_goblin.MovePosition(rb_goblin.position + movement * speed * Time.deltaTime);
        }
            anim_goblin.SetBool("move_gob", Mathf.Abs(movement.x) > 0.01f);
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 directionDamage = new Vector2(transform.position.x, 0);
            PlayerBehaviour playerScript = collision.gameObject.GetComponent<PlayerBehaviour>();
            playerScript.take_damage(directionDamage, 10);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_sword"))
        {
            Vector2 directionDamage = new Vector2(collision.gameObject.transform.position.x, 0);
            take_damage(directionDamage, 10);
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
                canMove = false;
                dead = true;
                movement = Vector2.zero;
                rb_goblin.linearVelocity = Vector2.zero;
                
                anim_goblin.SetBool("dead_gob", true);
            }
            else
            {
                rb_goblin.linearVelocity = new Vector2(0, rb_goblin.linearVelocity.y);
                Vector2 rebote = new Vector2(transform.position.x - direction.x, 1).normalized;
                rb_goblin.AddForce(rebote * rebote_force, ForceMode2D.Impulse);
            }
               
        }
    }

    public void RecoverFromDamage()
    {
        getDamage = false;
        rb_goblin.linearVelocity = Vector2.zero;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detection_radius);
    }
}