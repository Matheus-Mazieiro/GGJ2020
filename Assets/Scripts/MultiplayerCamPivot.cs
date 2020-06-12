using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Camera focus who also controls camera Z distance
/// </summary>
public class MultiplayerCamPivot : MonoBehaviour {
    Player[] playerArray;
    SmoothFollow2D smoothFollow2D;
    Camera cam;
    [SerializeField] AnimationCurve zCameraPerMaxPlayerDistanceRange;

    Vector3 Position {
        get {
            Vector3 ret = Vector3.zero;
            //foreach (var player in playerArray) //remove
            //    ret += player.transform.position; //remove
            return new Vector3(
                (playerArray.Max((p) => p.transform.position.x) + playerArray.Min((p) => p.transform.position.x)) / 2,
                (playerArray.Max((p) => p.transform.position.y) + playerArray.Min((p) => p.transform.position.y)) / 2,
                0
            );
        }
    }

    IEnumerator Start() {
        yield return null; // For everything load
        playerArray = FindObjectsOfType<Player>();
        if (playerArray.Length < 2) {
            Destroy(gameObject);
            yield break;
        }
        cam = Camera.main;
        smoothFollow2D = FindObjectOfType<SmoothFollow2D>();
        smoothFollow2D.m_Target = transform;
        yield return new WaitUntil(() => smoothFollow2D.regularMode); 
        yield return MainRoutine();
    }

    IEnumerator MainRoutine() {
        while (true) {
            transform.position = Position;
            cam.transform.position = new Vector3(
                cam.transform.position.x, cam.transform.position.y, zCameraPerMaxPlayerDistanceRange.Evaluate(GetMaxXDistanceBetweenPlayers())
            );
            //maxDistance = GetMaxXDistanceBetweenPlayers(); //remove
            yield return null;
        }
    }

    float GetMaxXDistanceBetweenPlayers() {
        float ret = 0;
        for (int i = 0; i < playerArray.Length-1; i++)
            for (int j = i+1; j < playerArray.Length; j++)
                ret = Mathf.Max(ret, Mathf.Abs(playerArray[i].transform.position.x - playerArray[j].transform.position.x));
        return ret;
    }
}
