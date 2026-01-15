using UnityEngine;

public class TestWater : MonoBehaviour
{
    public ParticleSystem ps;

    void Update()
    {
        var emission = ps.emission;

        if (Input.GetMouseButton(0))
        {
            emission.rateOverTime = 100;
            ps.Play();
        }
        else
        {
            emission.rateOverTime = 0;
        }
    }
}
