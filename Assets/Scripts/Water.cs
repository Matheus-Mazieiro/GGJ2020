using System.Collections;
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
    public bool goingIntoMenu; // Probably unnecessary
    public float fillSpeed;

    public float fillAmount;
    public Dor[] dores;
    float auxDiff;
    float over;

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
                over = 0;

                switch (dores[i].direction)
                {
                    case Dor.Side.DOWN: HandleDoorDown(dores[i]); break;
                    case Dor.Side.RIGHT: HandleDoorRight(dores[i]); break;
                    case Dor.Side.LEFT: HandleDoorLeft(dores[i]); break;
                    case Dor.Side.UP: HandleDoorUp(dores[i]); break;
                }

                if (endGame && fillAmount >= 100 && !goingIntoMenu) {
                    goingIntoMenu = true;
                    StartCoroutine(FindObjectOfType<Player>().GoToMenu());
                }

                fillAmount = Mathf.Clamp(fillAmount, -1, 101);
            }
        }

        void HandleDoorDown(Dor door) {
            if (door.door.isOpen && door.water.fillAmount < 100) {
                //over = (amount != 0 ? amount : fillAmount > 0 ? fillAmount * 0.2f * Time.deltaTime: amount) / 2;
                over = 0;
                if (door.water.fillAmount <= 95)
                    over = fillAmount * .1f;
                door.water.FillWater(over);

                fillAmount -= over;
                door.water.FillWater(over);
            }
        }

        void HandleDoorRight(Dor door) {
            if (door.door.isOpen && door.water.fillAmount < fillAmount) {
                auxDiff = (fillAmount - door.water.fillAmount);

                over = auxDiff * .25f;
                if (door.water.fillAmount + over > fillAmount - over &&
                    door.water.fillAmount - over > fillAmount + over
                ) {
                    Debug.Log("Possibly prevented stack overflow on right side.");
                    //Debug.Break();
                    over = (door.water.fillAmount + over) / 2f;
                }

                fillAmount -= over;
                door.water.FillWater(over);
            }
        }

        void HandleDoorLeft(Dor door) {
            if (door.door.isOpen && door.water.fillAmount < fillAmount) {
                auxDiff = (fillAmount - door.water.fillAmount);
                over = auxDiff * .25f;

                if (door.water.fillAmount + over > fillAmount - over &&
                    door.water.fillAmount - over > fillAmount + over
                    ) {
                    Debug.Log("Possibly prevented stack overflow on left side.");
                    over = (door.water.fillAmount + over) / 2f;
                    //Debug.Break();
                }

                fillAmount -= over;
                door.water.FillWater(over);
            }
        }

        void HandleDoorUp(Dor door) {
            if (door.door.isOpen && fillAmount > 100) {
                if (door.direction == Dor.Side.UP && door.water.fillAmount < 95) {
                    over = (fillAmount - 100);
                    fillAmount -= over;
                    door.water.FillWater(over);
                }
            }
        }


    }
}
