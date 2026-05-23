using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public float duration = 1f;

    void Start()
    {
        fadeImage.color = Color.black;

        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float time = 0f;

        Color c = fadeImage.color;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, time / duration);
            fadeImage.color = new Color(0, 0, 0, alpha);

            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }
}