using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishRandomRoute : MonoBehaviour {
	[System.Serializable]
	public class Route {
		public Vector3 initialPos;
		public Vector3 finalPos;
	}

	public Route[] routeArray = new Route[0];
	public GameObject[] fishPrefabArray = new GameObject[0];
	public Vector2 speedRange;
	public Vector2 spawnIntervalRange;
	public int limit;
	int count;

	void OnDrawGizmosSelected() {
		for (int i = 0; i < routeArray.Length; i++) {
			Gizmos.color = i== routeArray.Length-1 ? Color.blue : Color.green;
			Gizmos.DrawSphere(routeArray[i].initialPos, 1f);
			Gizmos.color = i == routeArray.Length - 1 ? Color.red : Color.green;
			Gizmos.DrawSphere(routeArray[i].finalPos, 1f);
		}
	}

	IEnumerator Start() {
		// Spawn Routine
		while (true) {
			yield return new WaitWhile(() => count >= limit);
			float timeToNextSpawn = Time.timeSinceLevelLoad + Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
			yield return new WaitForSeconds(timeToNextSpawn);
			StartCoroutine(FishRoutine(
				fishPrefabArray[Mathf.FloorToInt(Random.value)* fishPrefabArray.Length], 
				routeArray[Mathf.FloorToInt(Random.value) * routeArray.Length],
				Random.Range(speedRange.x, speedRange.y)
			));
		}
	}

	IEnumerator FishRoutine(GameObject prefab, Route route, float speed) {
		count++;
		var fish = Instantiate(prefab, route.initialPos, Quaternion.identity);
		fish.transform.LookAt(route.finalPos);
		while (route.finalPos != fish.transform.position) {
			yield return null;
			fish.transform.position = Vector3.MoveTowards(fish.transform.position, route.finalPos, Time.deltaTime*speed);
		}
		Destroy(fish);
		count--;
	}
}