using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    Rigidbody2D rb_player;
    //Variables para movimiento y salto
    public float speed, jumping_force;
    public LayerMask layerGround;
    public Transform tf_GroundDetector;
    private bool getDamage;
    public float rebote_force;
    //private bool atack;

    int lookDirection = 1;
    //private SpriteRenderer sr;

    public bool isAlive = true;
    bool canMove = true;
    bool onGround;

    //Variables para las animaciones
    Animator anim_player;
    //Variables de scripts externos
    public GameManager game_manager;
    //Variables de salud
    float life = 100;

    // Start is called before the first frame update
    void Start()
    {
        rb_player = GetComponent<Rigidbody2D>();
        anim_player = GetComponent<Animator>();
        //sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        anim_player.SetBool("isDamaged", getDamage);

        onGround = Physics2D.Linecast(transform.position, tf_GroundDetector.position, layerGround);
        anim_player.SetBool("isJumping", !onGround);

        if (!canMove) return;

        Move();
        Jump();
        Atack();

    }

  
    void Move()
    {
        //Izquierda: -1    0     1 Derecha
        float axisH = Input.GetAxis("Horizontal");
        rb_player.linearVelocity = new Vector2(axisH * speed,rb_player.linearVelocity.y);
        anim_player.SetBool("onMove", Mathf.Abs (axisH) > 0.01f && onGround);
        SetLookDirection(axisH);

        void SetLookDirection(float axisH)
        {
            if (axisH > 0)
            {
                transform.localScale = new Vector3(4, 4, 1);
            }
            else if (axisH < 0)
            {
                transform.localScale = new Vector3(-4, 4, 1);
            }

        }
    }
    

    void Jump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb_player.AddForce(new Vector2(0, jumping_force), ForceMode2D.Impulse);
        }

    }

    public void take_damage(Vector2 direction, int damageAmount)
    { 
        if(!getDamage)
        {
            canMove = false;
            getDamage = true;
            life -= damageAmount;
            game_manager.UpdateLife(life);
            if (life<=0)
            {
                isAlive = false;
                canMove = false;
                rb_player.linearVelocity = Vector2.zero;
                anim_player.SetBool("onMove", false);
                anim_player.SetBool("isDead", true);
            }
            else
            {
                rb_player.linearVelocity = new Vector2(0, rb_player.linearVelocity.y);
                Vector2 rebote = new Vector2(transform.position.x - direction.x, 1).normalized;
                rb_player.AddForce(rebote * rebote_force, ForceMode2D.Impulse);
            }
                
        }
    }

    public void RecoverFromDamage()
    {
        getDamage = false;
        canMove = true;
        rb_player.linearVelocity = Vector2.zero;
    }


    void Atack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canMove && onGround)
        {
            anim_player.SetBool("onMove", false);
            canMove = false;
            rb_player.linearVelocity = new Vector2(0, rb_player.linearVelocity.y);
            anim_player.SetTrigger("isAtacking");
        }
    }

    public void NotAtacking()
    {
        canMove = true;
    }






    //Cuando un objeto INICIA la colision
   /* private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            life -= 10;
            game_manager.UpdateLife(life);
            LifeDetector();
        }

    }
    void LifeDetector()
    {
        if (life <= 0)
        {
            canMove = false;
            anim_player.SetBool("onMove", false);
            anim_player.SetBool("isDead", true);
            rb_player.Sleep();
            game_manager.GameOver();
        }
    }*/





    //Cuando un objeto MANTIENE la colision
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("Se mantiene la colisi�n");
        }
    }

    //Cuando un objeto FINALIZA la colision
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            print("Se termin� la colisi�n");
        }
    }


    //Eventos de colisiones de tipo trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Recollectable")
        {
            Destroy(collision.gameObject);
            game_manager.AddCoin();
            GetComponent<SFXPlayer>().PlayCoinCollectAudio();

        }

        if (collision.gameObject.tag == "Castle")
        {
            print("Llegaste a la meta");
        }
    }
}
