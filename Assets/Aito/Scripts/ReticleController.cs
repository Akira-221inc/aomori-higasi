using UnityEngine;

public class ReticleController : MonoBehaviour
{
    void Update()
    {
        transform.position = Input.mousePosition;
    }
}
