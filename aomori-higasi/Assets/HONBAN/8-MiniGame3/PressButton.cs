using UnityEngine;

public class PressButton : MonoBehaviour
{
    public DistributorTaskManager taskManager;

    void OnMouseDown()
    {
        taskManager.OnPress();
    }
}
