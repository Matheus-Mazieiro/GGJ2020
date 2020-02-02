using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
	public Text currentTimeText;
	public Text recordTimeText;

	const string RECORD_KEY = "record";

	void Start() {
		RefreshRecord();
	}

	void Update() {
		currentTimeText.text = FormatTime(Time.timeSinceLevelLoad);
	}

	public void RefreshRecord() {
		recordTimeText.text = FormatTime(PlayerPrefs.GetFloat(RECORD_KEY));
	}

	public void SaveRecordIfBigger() {
		if(Time.timeSinceLevelLoad > PlayerPrefs.GetFloat(RECORD_KEY))
			PlayerPrefs.SetFloat(RECORD_KEY, Time.timeSinceLevelLoad);
	}

	static string FormatTime(float totalSeconds) {
		return string.Format("{0}:{1:00}", Mathf.FloorToInt(totalSeconds / 60), Mathf.FloorToInt(totalSeconds % 60));
	}
}