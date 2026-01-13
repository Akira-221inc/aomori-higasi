using UnityEngine;

public class WaterFlowController : MonoBehaviour
{
    public Camera cam;
    public ParticleSystem waterParticle;

    void Update()
    {
        var emission = waterParticle.emission;

        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // デバッグ
                Debug.Log("Hit: " + hit.collider.name);

                waterParticle.transform.position = hit.point + Vector3.up * 0.1f;

                emission.rateOverTime = 100;
                waterParticle.Play();
            }
            else
            {
                Debug.Log("Raycast NOT HIT");
            }
        }
        else
        {
            emission.rateOverTime = 0;
        }
    }
}
