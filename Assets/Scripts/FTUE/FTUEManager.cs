using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FTUEManager : MonoBehaviour
{
    [SerializeField] HoleManager holeManager;
    [SerializeField] WaterManager waterManager;
    [SerializeField] PanelsHandler canvasController;

    const string FTUE_KEY = "FTUE";

    enum TutorialState {
        started,
        startedDone,
        walked,
        openedDoor,
        flushed,
        fixedHole,
        done
    };

    [SerializeField] GameObject InitialPanel;
    [SerializeField] GameObject WalkPanel;
    [SerializeField] GameObject OpenDoorPanel;
    [SerializeField] GameObject FlushToiletPanel;
    [SerializeField] GameObject FixHolePanel;
    [SerializeField] GameObject TrainingDonePanel;

    Dictionary<TutorialState, GameObject> stateToPanelDic;

    [SerializeField] TutorialState state = TutorialState.started;

    public float firstMessageTime = 5f;
    public float targetTimeToWait = 15f;
    public float currentTime;

    public bool walkedTooEarly;
    float earlyWalkTime;
    float remainingMessageTime;

    private void Awake() {
        stateToPanelDic = new Dictionary<TutorialState, GameObject>() {
            {TutorialState.started, InitialPanel },
            {TutorialState.startedDone, WalkPanel },
            {TutorialState.walked, OpenDoorPanel },
            {TutorialState.openedDoor, FlushToiletPanel },
            {TutorialState.flushed, FixHolePanel },
            {TutorialState.fixedHole, TrainingDonePanel },
            {TutorialState.done, TrainingDonePanel }
        };

        EnablePanelForState(state);

        ResetTime(firstMessageTime);

        //move to menu
        if (PlayerPrefs.GetInt(FTUE_KEY, 0) != 0) {
        } else {
            StartGame();
        }
    }
        
    void StartGame() {
        //canvasController.StartTimer();
        waterManager.EnableWater();
    }

    void Update() {

        currentTime -= Time.deltaTime;


        if(currentTime <= 0) {
            Debug.Log("State: " + state);
            ResetTime();
            HandleFirstMessage();
            HandleEndOfTutorial();
            EnablePanelForState(state);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            FinishTutorial(true);
        }

        if(state == TutorialState.started || state == TutorialState.startedDone) {
            CheckHasWalked();
        }

    }

    void HandleFirstMessage() {
        if(state != TutorialState.started) { return; }
        state = TutorialState.startedDone;
        RemovePanel();
    }

    void HandleEndOfTutorial() {
        if (state == TutorialState.fixedHole) {
            FinishTutorial();
        }

    }

    void EnablePanelForState(TutorialState state) {

        if(state == TutorialState.walked &&
            walkedTooEarly &&
            (Time.realtimeSinceStartup - earlyWalkTime) < remainingMessageTime) {
            return;
        }

        canvasController.Activate(stateToPanelDic[state]);
    }

    void RemovePanel() {
        canvasController.Activate(null);
    }

    void CheckHasWalked() {
        if(state >= TutorialState.walked) { return; }
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {

            if(state == TutorialState.started) {
                walkedTooEarly = true;
                earlyWalkTime = Time.realtimeSinceStartup;
                remainingMessageTime = currentTime;
                Debug.Log("Remaining: " + remainingMessageTime);
            } else {
                RemovePanel();
            }

            ResetTime();
            state = TutorialState.walked;
        }
    }

    public void DoorOpened(){
        if(state >= TutorialState.openedDoor) { return; }
        state = TutorialState.openedDoor;
        RemovePanel();
        ResetTime();
    }

    public void Flushed() {
        if(state >= TutorialState.flushed) { return; }
        holeManager.ForceCreateNewHole();
        state = TutorialState.flushed;
        RemovePanel();
        ResetTime();
    }

    public void FixedHole() {
        if(state >= TutorialState.fixedHole) { return; }
        state = TutorialState.fixedHole;
        EnablePanelForState(state);
        ResetTime(firstMessageTime);
    }

    void ResetTime(float valueOverride = 0f) {
        currentTime = valueOverride > 0 ? valueOverride : targetTimeToWait;
    }

    void FinishTutorial(bool forced = false) {
        PlayerPrefs.SetInt(FTUE_KEY, 1);
        FindObjectOfType<ScenesManager>().LoadNextLevel();
    }
}
