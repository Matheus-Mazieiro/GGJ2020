using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnEnableDelayed : MonoBehaviour
{
    [SerializeField] float delay;

    Coroutine WaitRoutine;
    private void OnEnable() {
        WaitRoutine = StartCoroutine(DelayToPlay());
    }

    IEnumerator DelayToPlay() {
        yield return new WaitForSeconds(delay);
        GetComponent<AudioSource>().Play();
        WaitRoutine = null;
    }

    private void OnDisable() {
        if(WaitRoutine != null) {
            StopCoroutine(WaitRoutine);
        }
    }
}
