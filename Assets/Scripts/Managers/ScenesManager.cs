using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] bool usesFade;
    [SerializeField] float fadeDuration = 1f;

    public delegate void FadeInCompleted();
    public static event FadeInCompleted OnFadeInCompleted;

    public void LoadMainMenu()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = 0;

        LoadSceneAsync(nextScene, currentScene);
    }

    public void LoadNextLevel()
    {
        Debug.Log("Scene: " + SceneManager.GetActiveScene().buildIndex);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        //if(nextScene == null) { Debug.Log("No scene available next"); return; }
        LoadSceneAsync(nextScene, currentScene);
        //SceneManager.LoadScene(nextScene);
    }

    void LoadSceneAsync(int scene, int previousScene)
    {
        StartCoroutine(Internal_LoadAsyncScene(scene, previousScene));
    }

    IEnumerator Internal_LoadAsyncScene(int scene, int previousScene)
    {
        if (usesFade)
        {
            //fade
            FindObjectOfType<PanelFade>().FadeIn(fadeDuration);
        }
        float audioFadeDuration = usesFade ? fadeDuration : .1f;

        FindObjectOfType<AudioManager>().FadeOutBGM(fadeDuration);

        if (usesFade) { yield return new WaitForSeconds(audioFadeDuration); }

        OnFadeInCompleted?.Invoke();

        var player = FindObjectOfType<Player>();

        yield return new WaitForEndOfFrame();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone){yield return null;}

        FindObjectOfType<PanelFade>().FadeOut(fadeDuration);

        Debug.Log("Unloading scene "+previousScene); 
        SceneManager.UnloadSceneAsync(previousScene);
    }


}
