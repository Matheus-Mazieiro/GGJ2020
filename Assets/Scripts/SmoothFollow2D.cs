using UnityEngine;
using System.Collections;
using DG.Tweening;
public class SmoothFollow2D : MonoBehaviour
{

	//offset from the viewport center to fix damping
	public float m_DampTime = 10f;
	public Transform m_Target;
	public float m_XOffset = 0;
	public float m_YOffset = 0;

	public float minY = 8;
	public float maxY = 19;

	private float margin = 0.1f;

	public bool regularMode { get; private set; } = false;
	[Header("Tween pos")]
	public bool useFadeIn = true;
	float endGameDuration = 3f;
	public Transform targetStartgamePos;
	public Transform targetEndgamePos;

	//int fovGame = 65;
	//int fovZoomed = 120;
	Camera cam;

	void Awake()
	{
		regularMode = !useFadeIn;
		cam = GetComponent<Camera>();

		if (useFadeIn) { 
			transform.position = targetEndgamePos.position;
			//cam.fieldOfView = fovZoomed;
			StartGameCamera();
		}
	
		
	}

	void Update()
	{
		if (regularMode && m_Target)
		{
			float targetX = m_Target.position.x + m_XOffset;
			float targetY = m_Target.position.y + m_YOffset;

			targetY = targetY < minY ? minY : targetY;
			targetY = targetY > maxY ? maxY : targetY;

			if (Mathf.Abs(transform.position.x - targetX) > margin)
				targetX = Mathf.Lerp(transform.position.x, targetX, 1 / m_DampTime * Time.deltaTime);

			if (Mathf.Abs(transform.position.y - targetY) > margin)
				targetY = Mathf.Lerp(transform.position.y, targetY, m_DampTime * Time.deltaTime);

			transform.position = new Vector3(targetX, targetY, transform.position.z);
		}
	}

	public void StartGameCamera()
	{
		regularMode = false;
		//cam.DOFieldOfView(fovGame, endGameDuration).SetDelay(2f).OnComplete(OnGameStarted);
		transform.DOMove(targetStartgamePos.position, endGameDuration).SetDelay(2f).OnComplete(OnGameStarted); ;
	}

	void OnGameStarted()
	{
		regularMode = true;
	}

	public void EndGameCamera() {
		var camera = Camera.main;
		regularMode = false;
		camera.DOFieldOfView(100, endGameDuration);
		camera.transform.DOMove(targetEndgamePos.position, endGameDuration);
	}
}