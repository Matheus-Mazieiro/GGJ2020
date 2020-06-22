using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    bool isPaused = false;
    bool canUpdate = true;

    void Start()
    {
        
    }

    void Update()
    {
        if (!canUpdate) { return; }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isPaused) { UnpauseGame(); } else {
                PauseGame();
            }
        }
    }

    public void PauseGame() {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void UnpauseGame() {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void LoadMainMenu() {
        canUpdate = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        FindObjectOfType<ScenesManager>().LoadMainMenu();
    }

    public void DisablePause() {
        canUpdate = false;
    }
}
