using UnityEngine;
using UnityEngine.UI;

public class OilGameManager : MonoBehaviour
{
    public static OilGameManager Instance;

    public Slider fuelSlider;
    float fuel = 0;

    void Awake()
    {
        Instance = this;
        fuelSlider.value = 0;
    }

    public void AddFuel(float amount)
    {
        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0, 100);
        fuelSlider.value = fuel;

        ParticleSystem ps = GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // 衝突したパーティクルだけを消す
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
                int count = ps.GetParticles(particles);

                for (int i = 0; i < count; i++)
                {
                    // ここで当たったパーティクルを判定して消す
                    // OnParticleCollision は全衝突を意味するので、とりあえず全部消す
                    particles[i].remainingLifetime = 0;
                }

                ps.SetParticles(particles, count);
            }

        if (fuel >= 100)
        {
            Debug.Log("GAME CLEAR");
            Time.timeScale = 0;
        }
    }
}
