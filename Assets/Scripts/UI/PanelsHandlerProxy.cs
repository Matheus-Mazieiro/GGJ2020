using UnityEngine;

public class PanelsHandlerProxy : MonoBehaviour
{
    void Start(){ }

    public void ActivateMainMenu()
    {
        FindObjectOfType<PanelsHandler>().ActivateMainMenu();
    }
}
