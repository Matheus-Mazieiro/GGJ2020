using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    AudioSource audioSourceBGM;
    public void SetAudioBGM(AudioSource audioSource) {
        audioSourceBGM = audioSource;
    }

    public void FadeOutBGM(float fadeDuration)
    {
        audioSourceBGM.DOFade(0, fadeDuration).SetEase(Ease.Linear);
        audioSourceBGM = null;
    }
}
