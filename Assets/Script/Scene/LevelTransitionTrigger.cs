using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour
{
    [Header("Настройки сцены")]
    [Tooltip("Точное название сцены (уровня), которую нужно загрузить")]
    public string sceneToLoad;

    [Header("Настройки UI")]
    [Tooltip("Черная картинка FadeImage из Canvas")]
    public Image fadeImage;
    [Tooltip("Скорость затухания (чем больше, тем быстрее)")]
    public float fadeSpeed = 1.5f;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Проверяем, что вошел игрок и переход еще не начался
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            StartCoroutine(TransitionSequence(collision.gameObject));
        }
    }

    private IEnumerator TransitionSequence(GameObject player)
    {
        isTransitioning = true;

        // --- 1. ОСТАНОВКА УПРАВЛЕНИЯ ---
        // Отключаем скрипт движения игрока (PlayerMovement должен быть базовым названием вашего скрипта ходьбы)
        PlayerMovement movementScript = player.GetComponent<PlayerMovement>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        // Обнуляем физическую скорость игрока, чтобы он не продолжал скользить по инерции
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // В старых версиях Unity: rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // --- 2. ПЛАВНОЕ ЗАТУХАНИЕ ЭКРАНА ---
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetImageAlpha(alpha);
            yield return null; // Ждем следующий кадр
        }
        SetImageAlpha(1f); // Полная темнота

        // Небольшая микро-пауза в темноте для драматического эффекта
        yield return new WaitForSeconds(0.2f);

        // --- 3. ЗАГРУЗКА НОВОГО УРОВНЯ ---
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Забыли указать имя сцены в инспекторе триггера!");
            isTransitioning = false;
        }
    }

    private void SetImageAlpha(float alpha)
    {
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = Mathf.Clamp01(alpha);
            fadeImage.color = color;
        }
    }
}
