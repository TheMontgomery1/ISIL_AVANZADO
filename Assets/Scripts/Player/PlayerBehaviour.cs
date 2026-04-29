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
    int lookDirection = 1;
    //Variables para prefab del proyectil
    public GameObject prefab_Projectile;
    public Transform proyectilePosition;
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
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Shoot();
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            GameObject pj =  Instantiate(prefab_Projectile, proyectilePosition.position, transform.rotation);
            pj.GetComponent<ProjectileBehaviour>().SetProjectileDirection(lookDirection);
            GetComponent<SFXPlayer>().PlayArrowShoot();
        }
        
    }

    void Move()
    {
        //Izquierda: -1    0     1 Derecha
        float axisH = Input.GetAxis("Horizontal");
        rb_player.linearVelocity = new Vector2(axisH * speed,rb_player.linearVelocity.y);
        SetLookDirection(axisH);
    }

    void SetLookDirection(float axisHorizontal)
    {
        if (axisHorizontal > 0)
        {
            lookDirection = 1;
            GetComponent<SpriteRenderer>().flipX = false;
            anim_player.SetBool("onMove", true);
        }
        else if (axisHorizontal < 0)
        {
            lookDirection = -1;
            GetComponent<SpriteRenderer>().flipX = true;
            anim_player.SetBool("onMove", true);
        }
        else
        {
            anim_player.SetBool("onMove", false);
        }

    }


    void Jump()
    {
        bool onGround = Physics2D.Linecast(transform.position, tf_GroundDetector.position, layerGround);
        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            rb_player.AddForce(new Vector2(0, jumping_force), ForceMode2D.Impulse);
        }
        
    }

    //Eventos de Colisiones normales

    //Cuando un objeto INICIA la colision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            life -= 25;
            game_manager.UpdateLife(life);
            LifeDetector();
        }
    }
    void LifeDetector()
    {
        if (life <= 0)
        {
            rb_player.Sleep();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            game_manager.GameOver();
        }
    }

    //Cuando un objeto MANTIENE la colision
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            print("Se mantiene la colisi�n");
        }
    }

    //Cuando un objeto FINALIZA la colision
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
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
