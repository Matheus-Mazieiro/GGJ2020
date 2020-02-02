﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dor
{
    public Door door;
    public enum Side
    {
        RIGHT,
        LEFT,
        DOWN, 
        UP
    };
    public Side direction;
    public Water water;
}

public class Water : MonoBehaviour
{
    public bool endGame;
    public float fillSpeed;

    public float fillAmount;
    public Dor[] dores;
    float auxDiff;

    int maxSameFrameLoop = 10;
    int loopCount = 0;

    private void LateUpdate()
    {
        loopCount = 0;
    }

    void FixedUpdate()
    {
        transform.localScale = new Vector3(1, Mathf.Min(fillAmount / 100, 1), 1);
        FillWater(0);
    }

    public IEnumerator ApplyFlush(float value)
    {

        while (fillAmount > value + 5)
        {
            yield return new WaitForEndOfFrame();
            fillAmount -= (fillAmount - value) * 0.25f ;
        }
    }

    public void FillWater(float amount)
    {

        fillAmount += amount;

        if(++loopCount >= maxSameFrameLoop)
        {
            //Debug.Log("Last minute crash prevention system...");
            return;
        }

        if (fillAmount > 0)
        {
            for (int i = 0; i < dores.Length; i++)
            {
                float over = 0;
                switch (dores[i].direction)
                {
                    case Dor.Side.DOWN:
                        if (dores[i].door.isOpen && dores[i].water.fillAmount < 100)
                        {
                            //over = (amount != 0 ? amount : fillAmount > 0 ? fillAmount * 0.2f * Time.deltaTime: amount) / 2;
                            over = 0;
                            if (dores[i].water.fillAmount <= 95)
                                over = fillAmount * .1f;
                            dores[i].water.FillWater(over);

                            fillAmount -= over;
                            dores[i].water.FillWater(over);
                        }
                        break;
                    case Dor.Side.RIGHT:
                        if (dores[i].door.isOpen && dores[i].water.fillAmount < fillAmount)
                        {
                            auxDiff = (fillAmount - dores[i].water.fillAmount);

                            over = auxDiff * .25f;
                            if (dores[i].water.fillAmount + over > fillAmount - over &&
                                dores[i].water.fillAmount - over > fillAmount + over
                            )
                            {
                                Debug.Log("Possibly prevented stack overflow on right side.");
                                //Debug.Break();
                                over = (dores[i].water.fillAmount + over) / 2f;
                            }

                            fillAmount -= over;
                            dores[i].water.FillWater(over);
                        }
                        break;
                    case Dor.Side.LEFT:
                        if (dores[i].door.isOpen && dores[i].water.fillAmount < fillAmount)
                        {
                            auxDiff = (fillAmount - dores[i].water.fillAmount);
                            over = auxDiff * .25f;

                            if (dores[i].water.fillAmount + over > fillAmount - over &&
                                dores[i].water.fillAmount - over > fillAmount + over
                                )
                            {
                                Debug.Log("Possibly prevented stack overflow on left side.");
                                over = (dores[i].water.fillAmount + over) / 2f;
                                //Debug.Break();
                            }

                            fillAmount -= over;
                            dores[i].water.FillWater(over);
                        }
                        break;
                    case Dor.Side.UP:
                        if (dores[i].door.isOpen && fillAmount > 100)
                        {
                            if (dores[i].direction == Dor.Side.UP && dores[i].water.fillAmount < 95)
                            {
                                over = (fillAmount - 100);
                                fillAmount -= over;
                                dores[i].water.FillWater(over);
                            }
                        }
                        break;
                }

                if (endGame && fillAmount >= 100)
                    Debug.Log("PERDEU, MERMÃO");

                fillAmount = Mathf.Clamp(fillAmount, -1, 101);
            }
        }
    }
}
