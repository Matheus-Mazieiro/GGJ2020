using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flush : MonoBehaviour
{

    public bool on;
    public float flushValue;
    public Water[] water;
    public float timeOFF;

    void Update()
    {
    }

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
            item.StartCoroutine("ApplyFlush", flushValue);
        }
        StartCoroutine("ToEnable");
    }

}