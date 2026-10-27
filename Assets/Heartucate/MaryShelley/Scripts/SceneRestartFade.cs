
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneRestartFade : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;

    private bool isRestarting = false;

    public void RestartScene()
    {
        if (!isRestarting)
        {
            StartCoroutine(RestartWithFade());
        }
    }

    private IEnumerator RestartWithFade()
    {
        isRestarting = true;

        // Bildschirm schwarz einblenden
        yield return StartCoroutine(Fade(0f, 1f));

        // Aktuelle Scene neu laden
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                elapsed / fadeDuration
            );

            fadeImage.color = new Color(
                color.r, color.g, color.b, alpha
            );

            yield return null;
        }

        fadeImage.color = new Color(
            color.r, color.g, color.b, endAlpha
        );
    }
}
