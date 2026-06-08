using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Точка назначения")]
    public Transform teleportTarget;

    [Header("Настройки Камер")]
    [Tooltip("Камера текущей комнаты")]
    public GameObject currentRoomCamera;
    [Tooltip("Камера следующей комнаты (должна иметь тег MainCamera)")]
    public GameObject nextRoomCamera;

    [Header("Настройки UI")]
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    [Header("Загрузка уровня (опционально)")]
    [Tooltip("Если включено, будет загружен уровень вместо телепортации в рамках текущей сцены")]
    public bool loadNewLevel = false;
    [Tooltip("Имя сцены для загрузки (требуется, если loadNewLevel = true)")]
    public string levelToLoad = "";
    [Tooltip("Режим загрузки сцены")]
    public LoadSceneMode loadMode = LoadSceneMode.Single;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTeleporting)
        {
            StartCoroutine(TeleportSequence(collision.transform));
        }
    }

    private IEnumerator TeleportSequence(Transform playerTransform)
    {
        isTeleporting = true;

        // Анимация затухания
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(1f);

        // Пауза для плавности перехода
        yield return new WaitForSeconds(0.1f);

        if (loadNewLevel && !string.IsNullOrEmpty(levelToLoad))
        {
            // Загрузка нового уровня
            SceneManager.LoadScene(levelToLoad, loadMode);
            // После загрузки сцены корутина остановится автоматически
            yield break;
        }
        else if (teleportTarget != null)
        {
            // Обычная телепортация в рамках текущей сцены
            playerTransform.position = teleportTarget.position;

            if (nextRoomCamera != null)
            {
                nextRoomCamera.SetActive(true);
            }

            if (currentRoomCamera != null)
            {
                Destroy(currentRoomCamera);
            }

            yield return new WaitForEndOfFrame();

            Telekinesis telekinesis = playerTransform.GetComponent < Telekinesis > ();
            if (telekinesis == null && Camera.main != null)
            {
                telekinesis = Camera.main.GetComponent<Telekinesis>();
            }

            if (telekinesis != null)
            {
                telekinesis.RefreshCamera();
            }

            yield return new WaitForSeconds(0.1f);
        }

        // Анимация появления
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(0f);

        isTeleporting = false;
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