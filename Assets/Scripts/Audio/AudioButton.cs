using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip onHover;
    [SerializeField] AudioClip clickClip;

    private void Awake()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        audioSource.PlayOneShot(clickClip);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(onHover);
    }
}
