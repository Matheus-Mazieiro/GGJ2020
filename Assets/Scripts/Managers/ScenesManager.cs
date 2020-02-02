using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] bool usesFade;
    [SerializeField] float fadeDuration = 1f;

    void Start()
    {
    }

    public void LoadNextLevel()
    {
        Debug.Log("Cena: " + SceneManager.GetActiveScene().buildIndex);
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
        }

        if (usesFade) { yield return new WaitForSeconds(fadeDuration); }

        yield return new WaitForEndOfFrame();

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(previousScene);
    }


}
