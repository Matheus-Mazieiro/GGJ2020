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
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        //if(nextScene == null) { Debug.Log("No scene available next"); return; }
        LoadSceneAsync(nextScene);
        //SceneManager.LoadScene(nextScene);
    }

    void LoadSceneAsync(int scene)
    {
        StartCoroutine(Internal_LoadAsyncScene(scene));
    }

    IEnumerator Internal_LoadAsyncScene(int scene)
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
    }


}
