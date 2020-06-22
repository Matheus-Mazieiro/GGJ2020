using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PanelFade : MonoBehaviour
{
    int multiplier = -1;
    float initialXAnchor;

    void Start() {
        initialXAnchor = (GetComponentInParent<CanvasScaler>().referenceResolution.x + GetComponent<RectTransform>().rect.width*transform.localScale.x) / 2f;
        GetComponent<RectTransform>().anchoredPosition = new Vector2(initialXAnchor, 0);
        //DontDestroyOnLoad(GetComponentInParent<Canvas>().gameObject);
        //FadeIn(2f);
    }

    public void FadeIn(float duration) {
        //Debug.Log("Fade in");
        Fade(Vector2.zero, duration);
    }
    
    public void FadeOut(float duration) {
        //var vect2 = Screen.width;
        //Debug.Log("Fade out");
        Fade(new Vector2(multiplier * initialXAnchor, 0), duration);
        multiplier *= -1;
    }

    public void Fade(Vector2 targetPos, float duration){
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOAnchorPos(targetPos, duration).SetEase(Ease.Linear);
    }
}
