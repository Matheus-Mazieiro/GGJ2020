using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PanelsHandler : MonoBehaviour
{
    [SerializeField] GameObject mainMenuPanel;
    [SerializeField] GameObject[] panels;

    void Start() {}

    public void Activate(GameObject panel)
    {
        SetPanel(panel);
    }

    public void ActivateMainMenu()
    {
        Activate(mainMenuPanel);
    }

    void SetPanel(GameObject panel) {
        foreach(var go in panels)
        {
            go.SetActive(go == panel);
        }
    }

    public void Play() {
        RecordPlayerInput();
        GetComponent<ScenesManager>().LoadNextLevel();
    }

    void RecordPlayerInput() {
        int validPlayerCount = 0;
        var playerPanelArray = FindObjectsOfType<PlayerPanel>();
        System.Array.Sort(playerPanelArray, (pp1, pp2) => pp1.number.CompareTo(pp2.number));
        foreach (var playerPanel in playerPanelArray) {
            if (playerPanel.inputType == PlayerInput.Type.None)
                continue;
            validPlayerCount++;
            PlayerInput.SavePlayerInput(validPlayerCount, playerPanel.inputType);
        }
        for (int i = validPlayerCount; i < 4; i++)
            PlayerInput.SavePlayerInput(i + 1, PlayerInput.Type.None);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
