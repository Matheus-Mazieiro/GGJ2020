using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] HoleManager holeManager;
    [SerializeField] Water[] waterListByPriority;
    //todo initialize water dripping variable rates

    [SerializeField] bool isEnabled = false;

    public void EnableWater() {
        isEnabled = true;
    }

    void Update()
    {
        if (!isEnabled) { return; }
        //creates new holes and resolves all water coming from them
        holeManager.UpdateHoleManager();
        UpdateWater();
        ResetWater();
    }

    void UpdateWater() {
        for (int i = 0; i < waterListByPriority.Length; i++) {
            waterListByPriority[i].UpdateWater();
        }
    }

    void ResetWater() {
        for (int i = 0; i < waterListByPriority.Length; i++) {
            waterListByPriority[i].Reset();
        }
    }
}
