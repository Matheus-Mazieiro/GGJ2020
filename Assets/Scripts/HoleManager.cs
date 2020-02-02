using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public static HoleManager instance;
    public Hole[] holes;
    public float timePerHole;
    public int buracosAbertos = 0;

    float count;

    private void Update()
    {
        count += Time.deltaTime;
        if(count >= timePerHole)
        {
            count = 0;
            CreateNewHole();
        }
    }

    public void CreateNewHole()
    {
        int index =  Random.Range(0, holes.Length - 1);
        buracosAbertos++;
        if(buracosAbertos < holes.Length)
        {
            for (int i = 0; i < holes.Length; i++)
            {
                if (index == i)
                {
                    if (holes[i].isOpen || holes[i].openProcess)
                    {
                        CreateNewHole();
                        return;
                    }
                    else
                    {
                        holes[i].openProcess = true;
                        holes[i].StartOpenProcessEvent.Invoke();
                        return;
                    }
                }
            }
        }
    }
}
