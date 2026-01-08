using UnityEngine;

public class DistributorDial : MonoBehaviour
{
    public float rotateSpeed = 120f;
    public float successRange = 10f;
    public Transform targetMarker;

    public bool isActive = false;
    public bool isCompleted = false;

    void Update()
    {
        if (!isActive || isCompleted) return;

        // 親（Pivot）を回す
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    public bool CheckSuccess()
    {
        float dialAngle = transform.eulerAngles.z;
        float targetAngle = targetMarker.eulerAngles.z;

        float diff = Mathf.Abs(Mathf.DeltaAngle(dialAngle, targetAngle));

        if (diff <= successRange)
        {
            isCompleted = true;
            isActive = false;
            return true;
        }
        return false;
    }
}
