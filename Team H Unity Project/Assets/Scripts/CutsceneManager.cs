using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage; // Assign your UI Image component here
    public Sprite[] cutsceneSprites; // Drag and drop your cutscene images here in the inspector
    public float displayDuration = 3f; // Time each image stays on screen
    public float fadeDuration = 1f; // Time for fade in/out transitions
    public float cutsceneDuration; // Total duration of the cutscene
    private float cutsceneTimer = 0f;
    [HideInInspector] public bool isCutscenePlaying = true;
    private void Awake()
    {

        
    }
    private void Start()
    {
        cutsceneDuration = (fadeDuration*2+displayDuration)*cutsceneSprites.Length;

        StartCutscene();
       
    }
    void Update()
    {
        if (isCutscenePlaying)
        {
            // Increment timer
            cutsceneTimer += Time.deltaTime;

            // End cutscene if duration is reached
            if (cutsceneTimer >= cutsceneDuration)
            {
                EndCutscene();
            }
        }
    }

    void StartCutscene()
    {
       
        isCutscenePlaying = true;
        gameObject.SetActive(true);
        StartCoroutine(PlayCutscene());

    }

    public void EndCutscene()
    {
        isCutscenePlaying = false;
        gameObject.SetActive(false); // Hide cutscene UI
        // Trigger game progression, e.g., enable player controls, load next scene
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