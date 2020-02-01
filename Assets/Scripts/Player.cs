using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    bool isAtStair = false;
    public bool isUnderWater;
    public float redutor;
    public float speed;
    public float stairSpeed;
    public float fixRate;
    public float folego;
    public float flushTimer;
    float flushCounter;
    Rigidbody myBigidbody;
    Hole hole;


    void Start()
    {
        myBigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float speedY = 0;
        if (isAtStair)
            speedY = Input.GetAxis("Vertical") * stairSpeed;
        if(speedY != 0)
            myBigidbody.velocity = new Vector3(0, speedY, 0);
        else
            myBigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * (speed - redutor), myBigidbody.velocity.y, 0);



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
            if (Input.GetKey(KeyCode.Space))
            {
                hole.LoseHp(fixRate);
            }
            else
            {
                hole.RezetHP();
            }
        }
        else if (other.gameObject.layer == 10)
        {
            isUnderWater = true;
            redutor = speed * .7f;
            if (other.transform.parent.GetComponent<Water>() && other.transform.parent.GetComponent<Water>().fillAmount >= 80)
            {
                folego -= Time.deltaTime;
            }
        }
        else if (other.gameObject.layer == 11)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                flushCounter += Time.deltaTime;
                if(flushCounter >= flushTimer && other.GetComponent<Flush>())
                {
                    other.GetComponent<Flush>().FlushAct();
                }                    
            }
        }
        else if(other.gameObject.layer == 12)
        {
            if (Input.GetKeyDown(KeyCode.Space) && other.GetComponent<Door>())
            {
                Debug.Log("Door");
               Door dorComp = other.GetComponent<Door>();
                dorComp.isOpen = !dorComp.isOpen;
                dorComp.col.enabled = dorComp.isOpen;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            myBigidbody.velocity = new Vector3(myBigidbody.velocity.x, 0, 0);
            myBigidbody.useGravity = true;
            isAtStair = false;
        }
        else if (other.gameObject.layer == 9)
        {
            hole.RezetHP();
        }
        else if (other.gameObject.layer == 10)
        {
            redutor = 0;
            isUnderWater = false;
        }
    }
}
