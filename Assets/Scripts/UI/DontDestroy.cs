using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public bool oneInstanceOnly = true;
    private void Awake()
    {
        if (FindObjectsOfType<DontDestroy>().Length > 1 && oneInstanceOnly)
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
