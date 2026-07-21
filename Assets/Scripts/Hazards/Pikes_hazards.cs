using UnityEngine;

public class Pikes_hazards : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 directionDamage = new Vector2(transform.position.x, 0);
            PlayerBehaviour playerScript = collision.gameObject.GetComponent<PlayerBehaviour>();
            playerScript.take_damage(directionDamage, 100);
        }
    }
}
