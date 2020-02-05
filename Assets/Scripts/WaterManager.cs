using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField] HoleManager holeManager;
    [SerializeField] Water[] waterListByPriority;

    void Update()
    {
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
