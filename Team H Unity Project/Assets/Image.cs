using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage; // Assign your UI Image component here
    public Sprite[] cutsceneSprites; // Drag and drop your cutscene images here in the inspector
    public float displayDuration = 3f; // Time each image stays on screen
    public float fadeDuration = 1f; // Time for fade in/out transitions

    private void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        // Make sure the image is initially transparent
        Color transparent = cutsceneImage.color;
        transparent.a = 0;
        cutsceneImage.color = transparent;

        foreach (var sprite in cutsceneSprites)
        {
            // Set the current image
            cutsceneImage.sprite = sprite;

            // Fade in
            yield return StartCoroutine(FadeImage(1f, fadeDuration));

            // Display image for the set duration
            yield return new WaitForSeconds(displayDuration);

            // Fade out
            yield return StartCoroutine(FadeImage(0f, fadeDuration));
        }

        // End of cutscene (you can add transitions to gameplay here)
        Debug.Log("Cutscene Finished");
    }

    private IEnumerator FadeImage(float targetAlpha, float duration)
    {
        float startAlpha = cutsceneImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            Color color = cutsceneImage.color;
            color.a = newAlpha;
            cutsceneImage.color = color;
            yield return null;
        }
    }
}
