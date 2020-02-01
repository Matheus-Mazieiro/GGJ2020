using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool isOpen;
    public float hp;
    float startingHp;
    public float fillRatio;
    public Water water;

    private void Start()
    {
        startingHp = hp;
    }

    private void Update()
    {
        if (isOpen)
            water.FillWater(fillRatio);
    }

    public void LoseHp(float hp)
    {
        this.hp -= hp;
        if (this.hp <= 0)
            Destroy(this.gameObject);
    }

    public void RezetHP()
    {
        hp = startingHp;
    }
}
