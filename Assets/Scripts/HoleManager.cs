using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HoleManager : MonoBehaviour
{
    public static HoleManager instance;
    public Hole[] holes;

    public List<Hole> allHoles;
    public List<Hole> openHoles;
    public List<Hole> closedHoles;

    public float timePerHole;

    float count;
    private System.Random random = new System.Random();

    private void Start()
    {
        allHoles = holes.ToList();
        closedHoles = allHoles;
        InitHolePool();
        CreateNewHole();
    }

    void InitHolePool() {
        closedHoles = allHoles;
        Shuffle(closedHoles);
    }

    public void UpdateHoleManager()
    {
        CreateHole();
        UpdateHoles();
    }

    void CreateHole() {
        count += Time.deltaTime;
        if (count >= timePerHole) {
            count = 0;
            CreateNewHole();
        }
    }

    void UpdateHoles() {
        for (int i = 0; i < openHoles.Count; i++) {
            openHoles[i].UpdateHole();
        }
    }

    public void CreateNewHole()
    {
        if(closedHoles.Count > 0)
        {
            var hole = closedHoles[0];
            closedHoles.Remove(hole);
            openHoles.Add(hole);
            hole.openProcess = true;
            hole.StartOpenning();
        }
    }

    public void CloseHole(Hole hole) {
        openHoles.Remove(hole);
        closedHoles.Add(hole);
    }

    public void Shuffle<T>(IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

}
