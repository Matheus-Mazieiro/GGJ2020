using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimKit
{
    public GameObject gameObjects;
    public bool looped;
}

public class AnimController : MonoBehaviour
{
    int actualAnim = 0;
    public AnimKit[] anims;

    public void ResetHoleAnim() {
        if (anims[0] != null)
            anims[0].gameObjects.SetActive(true);
        if (anims[1] != null)
            anims[1].gameObjects.SetActive(false);
        GetComponent<Animator>().Play("Hole", 0, 0);
        actualAnim = 0;
    }

    public void ChangeAnim()
    {
        if (anims[actualAnim + 1] != null)
            anims[actualAnim].gameObjects.SetActive(false);
        actualAnim++;
        if (anims[actualAnim] != null)
            anims[actualAnim].gameObjects.SetActive(true);
    }

    public void Loop()
    {
        if (anims[actualAnim].looped)
            GetComponent<Animator>().Play("Hole", 0, 0);
    }

    public void StartHoleOpenning()
    {
        GetComponentInParent<Hole>().Openned();
    }

    public void PlayAnim(int anim)
    {
        if(anims[anim].gameObjects.activeSelf == false)
        {
            anims[actualAnim].gameObjects.SetActive(false);
            actualAnim = anim;
            anims[anim].gameObjects.SetActive(true);
        }
    }
}
