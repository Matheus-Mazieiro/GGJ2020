using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Dor;

[System.Serializable]
public class Dor {
    public Door door;

    public enum Side {
        RIGHT,
        LEFT,
        DOWN,
        UP,
        HOLE,
    };
    public Side direction;
    public Water water;
}

public class Water : MonoBehaviour {
    public bool endGame;
    public bool goingIntoMenu; // Probably unnecessary
    public float fillSpeed;

    public float fillAmount;
    public Dor[] dores;
    float auxDiff;
    float over;

    float cachedWaterAmmount;
    public bool cachedAmmountChanged = false;

    float minVerticalHeightToDrip = 5;
    public float drySpeed = 1;

    int maxSameFrameLoop = 10;
    int loopCount = 0;

    private void LateUpdate() {
        loopCount = 0;
    }

    Dictionary<Side, bool> sidesVisited = new Dictionary<Side, bool>() {
        {Side.DOWN, false},
        {Side.UP, false},
        {Side.LEFT, false},
        {Side.RIGHT, false},
    };

    public bool hasInteractedUp, hasInteractedDown, hasInteractedLeft, hasInteractedRight;

    //void Update() {
     //   transform.localScale = new Vector3(1, Mathf.Min(fillAmount / 100, 1), 1);
        //FillWater(0);
    //}

    public IEnumerator ApplyFlush(float value) {

        while (fillAmount > value + 5) {
            yield return new WaitForEndOfFrame();
            fillAmount -= (fillAmount - value) * 0.25f;
        }
    }

    public void CacheWater(float ammount, Side sideFrom) {
        cachedWaterAmmount += ammount;
        cachedAmmountChanged = true;

        if (sideFrom == Side.HOLE) { return; }

        sidesVisited[GetOppositeSide(sideFrom)] = true;
    }

    public void UpdateWater() {
        fillAmount += cachedWaterAmmount;

        if (++loopCount >= maxSameFrameLoop) {
            Debug.Log("Last minute crash prevention system...");
            return;
        }

        if (fillAmount > 0) {
            for (int i = 0; i < dores.Length; i++) {
                over = 0;

                switch (dores[i].direction) {
                    case Dor.Side.RIGHT: HandleDoorSideways(Side.RIGHT, dores[i]); break;
                    case Dor.Side.LEFT: HandleDoorSideways(Side.LEFT, dores[i]); break;
                    case Dor.Side.DOWN: HandleDoorDown(dores[i]); break;
                    case Dor.Side.UP: HandleDoorUp(dores[i]); break;
                }

                if (endGame && fillAmount >= 100 && !goingIntoMenu) {
                    goingIntoMenu = true;
                    StartCoroutine(FindObjectOfType<Player>().GoToMenu());
                }

                fillAmount = Mathf.Clamp(fillAmount, -1, 101);
            }
        }

        transform.localScale = new Vector3(1, Mathf.Min(fillAmount / 100, 1), 1);
        cachedWaterAmmount = 0;
    }


    void HandleDoorSideways(Dor.Side originalDir, Dor door) {
        if (WasVisitedFromSide(door.direction)) { return; }

        //Can this give the other side water?
        if (door.door.isOpen && door.water.fillAmount < fillAmount &&
            fillAmount > minVerticalHeightToDrip) {

            auxDiff = fillAmount - door.water.fillAmount;

            //if(auxDiff < 5) {
            //    over = auxDiff / 2f;
            //    fillAmount -= over;
            //    door.water.CacheWater(over, door.direction);
            //    return;
            //}

            over = auxDiff * .1f;
            fillAmount -= over;
            door.water.CacheWater(over, door.direction);
            return;
        }

        //Can the other side give this side water?
        if (door.door.isOpen && door.water.fillAmount > fillAmount &&
            door.water.fillAmount > minVerticalHeightToDrip) {

            auxDiff = door.water.fillAmount - fillAmount;

            //if (auxDiff < 5) {
            //    over = auxDiff / 2f;
            //    fillAmount += over;
            //    door.water.CacheWater(-over, door.direction);
            //    return;
            //}

            over = auxDiff * .1f;
            fillAmount += over;
            door.water.CacheWater(-over, door.direction);
            return;
        }

    }

    void HandleDoorDown(Dor door) {
        if (WasVisitedFromSide(door.direction)) { return; }

        //can the door down give this water?
        if (door.door.isOpen && door.water.fillAmount > 100 && fillAmount < 100 &&
            door.water.fillAmount > minVerticalHeightToDrip) {
            over = door.water.fillAmount - 100;
            door.water.CacheWater(-over, Side.UP);
            fillAmount += over;
            return;
        }

        //can door down receive water from this?
        if (door.door.isOpen && door.water.fillAmount < 100 &&
            fillAmount > minVerticalHeightToDrip) {
            //if (door.water.fillAmount <= 95) {
            over = (fillAmount - minVerticalHeightToDrip) * .1f;
            over = over + door.water.fillAmount > 100 ? (over + door.water.fillAmount) - 100 : over;
            fillAmount -= over;
            door.water.CacheWater(over, Side.UP);
            return;
            //}
        }
    }

    void HandleDoorUp(Dor door) {
        if (WasVisitedFromSide(door.direction)) { return; }

        //can the door up give this water?
        if (door.door.isOpen && door.water.fillAmount > 0 && fillAmount < 100) {
            over = (door.water.fillAmount - minVerticalHeightToDrip) *.1f;
            door.water.CacheWater(-over, Side.UP);
            fillAmount += over;
            return;
        }

        //can door up receive water from this?
        if (door.door.isOpen && door.water.fillAmount < 100 && fillAmount > 100) {
            //if (door.water.fillAmount <= 95) {
            over = fillAmount - 100;
            over = over + fillAmount > 100 ? (over + fillAmount) - 100 : over;
            door.water.CacheWater(over, Side.UP);
            fillAmount -= over;
            return;
            //}
        }
    }

    public void Reset() {
        sidesVisited[Side.DOWN] = false;
        sidesVisited[Side.UP] = false;
        sidesVisited[Side.LEFT] = false;
        sidesVisited[Side.RIGHT] = false;

        //move to another method
        if (!cachedAmmountChanged && fillAmount < minVerticalHeightToDrip) {
            fillAmount -= Time.deltaTime * drySpeed;
            fillAmount = fillAmount <= 0 ? 0 : fillAmount;
        }

        cachedAmmountChanged = false;
    }

    bool WasVisitedFromSide(Side sideFrom) {
        return sidesVisited[GetOppositeSide(sideFrom)];
    }

    Side GetOppositeSide(Side side) {
        switch (side) {
            case Side.DOWN: return Side.UP;
            case Side.UP: return Side.DOWN; 
            case Side.LEFT: return Side.RIGHT;
            default: return Side.LEFT;
        }
    }

}