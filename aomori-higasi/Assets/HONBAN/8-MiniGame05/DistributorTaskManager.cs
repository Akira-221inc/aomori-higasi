using UnityEngine;

public class DistributorTaskManager : MonoBehaviour
{
    public DistributorDial[] dials;
    int currentIndex = 0;

    void Start()
    {
        ActivateCurrentDial();
    }

    void ActivateCurrentDial()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].isActive = (i == currentIndex);
        }
    }

    public void OnPress()
    {
        if (currentIndex >= dials.Length) return;

        bool success = dials[currentIndex].CheckSuccess();

        if (success)
        {
            currentIndex++;

            if (currentIndex >= dials.Length)
            {
                Debug.Log("TASK CLEAR!");
            }
            else
            {
                ActivateCurrentDial();
            }
        }
    }
}
