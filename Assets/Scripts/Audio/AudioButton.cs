using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip onHover;
    [SerializeField] float onHoverVolume = .6f;
    [SerializeField] AudioClip clickClip;

    private void Awake()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        audioSource.volume = 1f;
        audioSource.PlayOneShot(clickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.volume = onHoverVolume;
        audioSource.PlayOneShot(onHover);
    }
}
