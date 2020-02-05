using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hole : MonoBehaviour
{
    public bool isOpen;
    public float hp;
    float startingHp;
    public float fillRatio = 1;
    public Water water;
    public HoleManager holeManager;

    public UnityEvent StartOpenProcessEvent = new UnityEvent();

    [HideInInspector] public bool openProcess;

    private void Start()
    {
        startingHp = hp;
    }

    public void UpdateHole()
    {
        if (isOpen)
            water.CacheWater(fillRatio, Dor.Side.HOLE);
            //water.FillWater(fillRatio);
    }

    public void LoseHp(float hp)
    {
        this.hp -= hp;
        if (this.hp <= 0)
        {
            //Destroy(this.gameObject);
            openProcess = false;
            transform.GetChild(0).gameObject.SetActive(false);
            holeManager.buracosAbertos--;
        }
    }

    public void RezetHP()
    {
        hp = startingHp;
    }

    public void StartOpenning()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Openned() // Chamar no animation event quando o buraco abrir
    {
        isOpen = true;
    }

}
