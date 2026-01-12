using UnityEngine;

public class OilParticleSound : MonoBehaviour
{
    ParticleSystem ps;
    AudioSource audioSource;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (ps.isPlaying)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}

