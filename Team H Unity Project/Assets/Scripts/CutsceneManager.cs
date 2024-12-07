using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using System.Linq;


public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private bool DoesFadeOut;
    public Image cutsceneImage; // Assign your UI Image component here
    public Sprite[] cutsceneSprites; // Drag and drop your cutscene images here in the inspector
    public float displayDuration = 3f; // Time each image stays on screen
    public float fadeDuration = 1f; // Time for fade in/out transitions
    public float cutsceneDuration; // Total duration of the cutscene
    private float cutsceneTimer = 0f;
    public bool isCutscenePlaying = false;

    [SerializeField] private string nextSceneName;

    [Header("Sound GameObjects")]
    [SerializeField] private List<GameObject> soundGameObjects;
    [SerializeField] private List<float> timesToPlaySounds;

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
        PlaySounds();
    }

    public void EndCutscene()
    {
        isCutscenePlaying = false;
        gameObject.SetActive(false); // Hide cutscene UI
        // Trigger game progression, e.g., enable player controls, load next scene
    }

    private void PlaySounds()
    {
        //play cutscene sounds
        if (soundGameObjects.Count == timesToPlaySounds.Count && soundGameObjects.Count != 0)
        {
            for (int i = 0; i < soundGameObjects.Count; i++)
            {
                StartCoroutine(WaitThenInstantiate(soundGameObjects[i], timesToPlaySounds[i]));
            }
        }
    }

    private IEnumerator PlayCutscene()
    {
        // Make sure the image is initially opaque
        Color opaque = cutsceneImage.color;
        opaque.a = 1f;
        cutsceneImage.color = opaque;

        for (int i = 0; i < cutsceneSprites.Length; i++)
        {
            // Set the current image
            cutsceneImage.sprite = cutsceneSprites[i];

            // Display image for the set duration
            yield return new WaitForSeconds(displayDuration);
        }

        if (DoesFadeOut)
        {
            yield return StartCoroutine(FadeImage(0f, fadeDuration)); // Fade out
        }
        
        // End of cutscene (you can add transitions to gameplay here)
        if (nextSceneName != "")
        {
            SceneManager.LoadScene(nextSceneName);
        }
        Debug.Log("Cutscene Finished");
        EndCutscene();
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

    private IEnumerator WaitThenInstantiate(GameObject prefabToInstantiate, float secondsToWait)
    {
        //wait for secondsToWait seconds
        yield return new WaitForSeconds(secondsToWait);

        //create prefabToInstantiate
        Instantiate(prefabToInstantiate);
    }
}
