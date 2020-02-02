using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public Text currentTimeText;
	public Text recordTimeText;
    float startTime;

    float CurrentTime => Time.timeSinceLevelLoad - startTime;

    const string RECORD_KEY = "record";

	void Start() {
        startTime = Time.timeSinceLevelLoad;
		RefreshRecord();
	}

	void Update() {
		currentTimeText.text = FormatTime(CurrentTime);
	}

	public void RefreshRecord() {
		recordTimeText.text = FormatTime(PlayerPrefs.GetFloat(RECORD_KEY));
	}

	public void SaveRecordIfBigger() {
		if(CurrentTime > PlayerPrefs.GetFloat(RECORD_KEY))
			PlayerPrefs.SetFloat(RECORD_KEY, CurrentTime);
	}

	static string FormatTime(float totalSeconds) {
		return string.Format("{0}:{1:00}", Mathf.FloorToInt(totalSeconds / 60), Mathf.FloorToInt(totalSeconds % 60));
	}
}