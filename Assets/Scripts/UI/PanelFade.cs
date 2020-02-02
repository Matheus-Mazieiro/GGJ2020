using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class PanelFade : MonoBehaviour
{
    //[SerializeField] Vector2 targetPos;

    private void Awake()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(Screen.width * transform.localScale.x, 0);
    }

    void Start(){
        //DontDestroyOnLoad(GetComponentInParent<Canvas>().gameObject);
        //FadeIn(2f);
    }

    public void FadeIn(float duration) {
        Fade(Vector2.zero, duration);
    }
    
    public void FadeOut(float duration) {
        //var vect2 = Screen.width;
        Fade(new Vector2(-Screen.width * transform.localScale.x, 0), duration);
    }

    public void Fade(Vector2 targetPos, float duration){
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOAnchorPos(targetPos, duration).SetEase(Ease.Linear);
    }
}
