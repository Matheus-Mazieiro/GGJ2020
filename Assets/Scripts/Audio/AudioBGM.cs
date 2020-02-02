using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioBGM : MonoBehaviour
{
    void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        FindObjectOfType<AudioManager>().SetAudioBGM(audioSource);
        audioSource.Play();
    }

}
