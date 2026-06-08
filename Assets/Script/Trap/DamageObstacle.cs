using UnityEngine;

public class DamageObstacle : MonoBehaviour
{
    public int damageAmount = 1;

    // —работает, если коллайдер шипов Ч триггер
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
        }
    }

    // —работает, если коллайдер шипов твердый (физический)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
        }
    }
}
