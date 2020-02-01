using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    [HideInInspector] public Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }
    //public Dor dor;
}
