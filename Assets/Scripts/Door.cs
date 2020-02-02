using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public bool debug = false;
    [SerializeField] GameObject blockingColliderGO;
    [SerializeField] GameObject meshGO;
    [SerializeField] float distanceToDetect = .3f;

    [HideInInspector] public Collider col;

    Player player;

    private void Start()
    {
        col = GetComponent<Collider>();
        player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (debug)
        {
            Debug.Log("Distance: "+(Vector3.Distance(player.transform.position, transform.position) < distanceToDetect));
        }

        if (Vector3.Distance(player.transform.position, transform.position) < distanceToDetect &&
            Input.GetKeyDown(KeyCode.Space)
            )
        {
            isOpen = !isOpen;
            blockingColliderGO.SetActive(!isOpen);
            meshGO.SetActive(!isOpen);
        }
    }
    //public Dor dor;
}
