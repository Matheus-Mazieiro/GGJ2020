using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    public bool isOpen;
    [SerializeField] GameObject blockingColliderGO;
    [SerializeField] GameObject meshGO;
    [SerializeField] float distanceToDetect = .3f;

    [SerializeField] AudioClip doorOpen;
    [SerializeField] UnityEvent OnDoorOpen;

    [HideInInspector] public Collider col;


    Player[] playerArray;

    private void Start()
    {
        col = GetComponent<Collider>();
        playerArray = FindObjectsOfType<Player>();
    }

    private void Update()
    {
        if (IsPlayerTryingToOpenDoor()){
            isOpen = !isOpen;
            blockingColliderGO.SetActive(!isOpen);
            meshGO.SetActive(!isOpen);
            GetComponent<AudioSource>().PlayOneShot(doorOpen);
            OnDoorOpen?.Invoke();
        }
    }

    bool IsPlayerTryingToOpenDoor() {
        return playerArray.Any((p) => p!=null && p.DoorButtonTriggered && Vector3.Distance(p.transform.position, transform.position) < distanceToDetect);
    }
    //public Dor dor;
}
