using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("Fade настройки")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1.1f;

    [Header("Магия")]
    [SerializeField] private ParticleSystem magicParticles;

    [Header("Задержка перед загрузкой")]
    [SerializeField] private float delayBeforeLoad = 0.4f;

    public void StartGame()
    {
        StartCoroutine(TransitionCoroutine());
    }

    private IEnumerator TransitionCoroutine()
    {
        if (magicParticles != null)
            magicParticles.Play();

        yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

        yield return new WaitForSeconds(delayBeforeLoad);

        SceneManager.LoadScene(1); // 1 = Level1
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);

            fadeCanvasGroup.alpha = alpha;

            Image fadeImage = fadeCanvasGroup.GetComponent<Image>();
            if (fadeImage != null)
            {
                Color color = fadeImage.color;
                color.a = alpha;
                fadeImage.color = color;
            }

            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}