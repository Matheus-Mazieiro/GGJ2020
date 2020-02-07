using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Flush : MonoBehaviour
{
    public bool on;
    public float flushValue;
    public Water[] water;
    public float timeOFF;
    [SerializeField] UnityEvent OnFlushed;

    IEnumerator ToEnable()
    {
        yield return new WaitForSeconds(timeOFF);
        on = true;
    }

    public void FlushAct()
    {
        on = false;
        foreach (Water item in water)
        {
            item.StartCoroutine(item.ApplyFlush(flushValue));
        }
        OnFlushed?.Invoke();
        StartCoroutine(ToEnable());
    }

}