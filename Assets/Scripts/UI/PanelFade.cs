using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PanelFade : MonoBehaviour
{
    int multiplier = -1;

    private void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width * transform.localScale.x, 0);
    }

    void Start(){
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
        Fade(new Vector2(multiplier * Screen.width * transform.localScale.x, 0), duration);
        multiplier *= -1;
    }

    public void Fade(Vector2 targetPos, float duration){
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOAnchorPos(targetPos, duration).SetEase(Ease.Linear);
    }
}
