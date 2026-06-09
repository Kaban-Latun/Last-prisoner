using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Параметры здоровья")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Настройки неуязвимости")]
    [Tooltip("Сколько секунд игрок неуязвим после удара")]
    public float iframeDuration = 1.5f;
    [Tooltip("Скорость мигания спрайта при уроне")]
    public float blinkSpeed = 0.1f;
    private bool isInvulnerable = false;

    [Header("Интерфейс (UI)")]
    [Tooltip("Массив картинок сердечек из Canvas (должно быть ровно 3)")]
    public Image[] hearts;
    [Tooltip("Спрайт полного сердечка")]
    public Sprite fullHeart;
    [Tooltip("Спрайт пустого сердечка")]
    public Sprite emptyHeart;

    private SpriteRenderer spriteRenderer;

    public AudioSource audiosource;
    public AudioClip hit;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateHealthUI();
    }

    // Метод получения урона
    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        audiosource.clip = hit;
        audiosource.Play();
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvulnerable());
        }
    }

    // Метод лечения (вызывается из аптечек)
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    // Корутина неуязвимости и мигания спрайта
    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;
        float timer = 0f;

        // Пока идет время неуязвимости, плавно переключаем прозрачность спрайта
        while (timer < iframeDuration)
        {
            // Делаем спрайт полупрозрачным (эффект вспышки/мигания)
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(blinkSpeed);

            // Возвращаем обычный цвет
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(blinkSpeed);

            timer += blinkSpeed * 2;
        }

        // Гарантируем, что после окончания мигания спрайт полностью виден
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        isInvulnerable = false;
    }

    // Обновление картинок сердечек на экране
    private void UpdateHealthUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        // Сюда можно добавить перезапуск уровня: 
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
