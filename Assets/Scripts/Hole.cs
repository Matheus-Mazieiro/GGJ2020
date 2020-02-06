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
    AnimController m_animController;
    AnimController animController { get {
            if(m_animController == null) { m_animController = GetComponentInChildren<AnimController>(); }
            return m_animController;
        }
    }

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
            isOpen = false;
            transform.GetChild(0).gameObject.SetActive(false);
            holeManager.CloseHole(this);
        }
    }

    public void RezetHP()
    {
        hp = startingHp;
    }

    public void StartOpenning()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        animController.ResetHoleAnim();
    }

    public void Openned() // Chamar no animation event quando o buraco abrir
    {
        Debug.Log("Open");
        isOpen = true;
    }

}
