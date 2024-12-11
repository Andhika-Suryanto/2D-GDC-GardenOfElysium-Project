using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public float fadeDuration = 1f; // Durasi fade
    private Image fadeImage;
    private bool isFadingOut = false;

    private void Awake()
    {
        // Dapatkan komponen Image dari panel
        fadeImage = GetComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 1); // Mulai dengan layar hitam
    }

    private void Start()
    {
        // Mulai dengan fade in
        StartCoroutine(FadeIn());
    }

    public void StartTransition(string sceneName)
    {
        // Mulai fade out dan pindah scene
        if (!isFadingOut)
        {
            StartCoroutine(FadeOut(sceneName));
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOut(string sceneName)
    {
        isFadingOut = true;
        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}