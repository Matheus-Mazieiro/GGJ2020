using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInDisableList : MonoBehaviour
{
    public GameObject[] disableOnFadeInComplete;
    // Start is called before the first frame update
    void Start()
    {
        ScenesManager.OnFadeInCompleted += OnFadeInCompleted;
    }

    void OnFadeInCompleted() {
        foreach (var go in disableOnFadeInComplete) {
            go.SetActive(false);
            Destroy(go);
        }
    }
}
