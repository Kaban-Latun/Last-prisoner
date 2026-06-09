using UnityEngine;
using UnityEngine.Audio;

public class HeartHeal : MonoBehaviour
{
    [Tooltip("Сколько сердечек восстановит предмет при подборе")]
    public int healAmount = 1;
    public AudioSource audiosource;
    public AudioClip healClip;  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                audiosource.clip = healClip;
                audiosource.Play();
                playerHealth.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
