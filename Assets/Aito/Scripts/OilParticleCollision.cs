using UnityEngine;

public class OilParticleCollision : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Engine"))
        {
            OilGameManager.Instance.AddFuel(0.2f);
        }
    }
}
