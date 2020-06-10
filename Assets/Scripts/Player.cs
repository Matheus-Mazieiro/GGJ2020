using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public int number;
    [SerializeField] Material myMaterial;
    PlayerInput input;

    bool isAtStair = false;
    bool taMorreno = false;
    bool closingHole = false;
    bool flushing = false;
    bool walking = false;
    bool climbing = false;
    bool isAtDoorTrigger = false;
    public bool isUnderWater;

    public float redutor;
    public float speed;
    public float stairSpeed;
    public float fixRate;
    public float folego;
    public float flushTimer;
    float flushCounter;

    internal Collider myCollider;
    Rigidbody myBigidbody;
    Hole hole;

    public AnimController animCtrl;

    Door hoverDoor;
    public GameObject walk;

    public Door[] portas;

    public AudioClip step;
    public AudioClip waterStep;

    [Header("Callbacks")]
    [SerializeField] UnityEvent onPlayerDead;

    internal bool isDying = false;

    public bool AnyAxisInput => input.HorizontalInputAxis != 0 || input.VerticalInputAxis != 0;
    public bool DoorButtonTriggered => input.DoorButtonTriggered;

    PlayerInput.Type PlayerInputType {
        get {
            return Input.GetJoystickNames().Length > 0 && number != 1 ? PlayerInput.Type.Joystick1 : PlayerInput.Type.Keyboard;
        }
    }

    void Awake() {
        if (Input.GetJoystickNames().Length + 1 < number)
            Destroy(gameObject);
        myCollider = GetComponent<Collider>();
    }

    void Start()
    {
        myBigidbody = GetComponent<Rigidbody>();
        if(myMaterial != null)
            ApplyMaterialInAllRenderers(myMaterial);
        IgnoreAllPlayersColliders();

        //Time.timeScale = 10f;
        //StartCoroutine(GoToMenu());
    }

    void ApplyMaterialInAllRenderers(Material mat) {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>(true)) {
            if (renderer.materials.Length == 0)
                continue;
            renderer.materials = Enumerable.Repeat(mat, renderer.materials.Length).ToArray();
        }
    }

    void IgnoreAllPlayersColliders() {
        foreach (var player in FindObjectsOfType<Player>()) {
            if (player == this)
                continue;
            Physics.IgnoreCollision(myCollider, player.myCollider);
        }
    }

    void FixedUpdate()
    {
        if (isDying) { return; }

        transform.position = new Vector3(transform.position.x, transform.position.y, 8.85f);
        float speedY = 0;
        if (isAtStair && !walking)
            speedY = input.VerticalInputAxis * stairSpeed;
        if(speedY != 0)
            myBigidbody.velocity = new Vector3(0, speedY, 0);
        else
            myBigidbody.velocity = new Vector3(input.HorizontalInputAxisRaw * (speed - redutor), myBigidbody.velocity.y, 0);

        if (myBigidbody.velocity.x != 0)
            walking = true;
        else walking = false;

        if (isAtStair && !walking)
            climbing = true;
        else climbing = false;

        if (taMorreno)
            animCtrl.PlayAnim(4);
        else if (closingHole)
            animCtrl.PlayAnim(3);
        else if (flushing)
        {
            animCtrl.PlayAnim(2);
        }
        else if (walking)
        {
            animCtrl.PlayAnim(1);
            walk.transform.eulerAngles = new Vector3(0, -90f * input.HorizontalInputAxis - 90, 0);
        }
        else if (climbing)
        {
            animCtrl.PlayAnim(5);
            walk.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            animCtrl.PlayAnim(0);
            walk.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (!isUnderWater && folego <= 10)
            folego += Time.deltaTime * 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            myBigidbody.velocity = new Vector3(myBigidbody.velocity.x, 0, 0);
            isAtStair = true;
            myBigidbody.useGravity = false;
        }

        if (other.gameObject.layer == 9)
        {
            hole = other.gameObject.GetComponent<Hole>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if (input.CloseHoleButtonTriggered && other.GetComponent<Hole>().isOpen)
            {
                closingHole = true;
                hole.LoseHp(fixRate);
            }
            else
            {
                closingHole = false;
                hole.RezetHP();
            }
        }
        if (other.gameObject.layer == 10)
        {
            isUnderWater = true;
            if (other.transform.parent.GetComponent<Water>() && other.transform.parent.GetComponent<Water>().fillAmount >= 30)
            {
                redutor = speed * .4f;
                transform.GetComponentInChildren<AnimController>().anims[1].gameObjects.GetComponent<AudioSource>().clip = waterStep;
            }
            else transform.GetComponentInChildren<AnimController>().anims[1].gameObjects.GetComponent<AudioSource>().clip = step;

            if (other.transform.parent.GetComponent<Water>() && other.transform.parent.GetComponent<Water>().fillAmount >= 80)
            { 
                folego -= Time.deltaTime;
                if (folego <= 0)
                {
                    taMorreno = true;
                    StartCoroutine(GoToMenu());
                }
            }
        }
        if (other.gameObject.layer == 11)
        {
            if (input.FlushButtonTriggered)
            {
                flushing = true;
                flushCounter += Time.deltaTime;
                if (flushCounter >= flushTimer && other.GetComponent<Flush>())
                {
                    other.GetComponent<Flush>().FlushAct();
                }
            }
            else flushing = false;
        }
        /*
        if(other.gameObject.layer == 12)
        {
            if (Input.GetKeyDown(KeyCode.Space) && other.GetComponent<Door>())
            {
               Door dorComp = other.GetComponent<Door>();
                if (dorComp.isOpen)
                    dorComp.isOpen = false;
                else dorComp.isOpen = true;
                dorComp.col.enabled = dorComp.isOpen;
            }
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            myBigidbody.velocity = new Vector3(myBigidbody.velocity.x, 0, 0);
            myBigidbody.useGravity = true;
            isAtStair = false;
        }
        if (other.gameObject.layer == 9)
        {
            hole.RezetHP();
        }
        if (other.gameObject.layer == 10)
        {
            redutor = 0;
            isUnderWater = false;
        }
        //if (other.gameObject.layer == 12)
        //{
        //    hoverDoor = null;
        //    isAtDoorTrigger = true;
        //}
    }

    public IEnumerator GoToMenu()
    {
        if (isDying)
        {
            yield break;
        }

        isDying = true;

        // Only continues the flow if there is no player alive
        foreach (Player player in FindObjectsOfType<Player>())
            if(!player.isDying)
                yield break;

        FindObjectOfType<CanvasController>().SaveRecordIfBigger();
        FindObjectOfType<CanvasController>().RefreshRecord() ;
        speed = 0;
        stairSpeed = 0;
        Time.timeScale = 1;
        yield return new WaitForSeconds(5);
        onPlayerDead?.Invoke();
        FindObjectOfType<ScenesManager>().LoadMainMenu();
        //UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
