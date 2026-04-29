using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    Rigidbody2D rb_Projectile;
    public float speed;
    public GameObject bubble_effect;
    // Start is called before the first frame update
    void Start()
    {
        rb_Projectile = GetComponent<Rigidbody2D>();
        StartCoroutine(ProjectileDestroy());
    }
    IEnumerator ProjectileDestroy()
    {
        //Lo que quiero que se ejecute inmediatamente

        //El tiempo de espera -- OBLIGATORIO
        yield return new WaitForSeconds(5);
        //Lo que quiero que se ejecute luego del tiempo de espera
        Destroy(this.gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        rb_Projectile.linearVelocity = new Vector2(speed,rb_Projectile.linearVelocity.y);

    }

    public void SetProjectileDirection(float direction)
    {
        speed = speed * direction; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            StartCoroutine(StartBubbleEffect(collision.gameObject));
        }
    }

    IEnumerator StartBubbleEffect(GameObject enemy)
    {
        GameObject effect = Instantiate(bubble_effect, this.transform);
        enemy.GetComponent<Rigidbody2D>().Sleep();
        enemy.GetComponent<SpriteRenderer>().enabled = false;
        this.gameObject.GetComponent<Rigidbody2D>().Sleep();
        this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(enemy);
        Destroy(effect);
        Destroy(this.gameObject);

    }
}
