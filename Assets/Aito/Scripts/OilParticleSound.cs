using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(ParticleSystem))]
public class OilParticleSound : MonoBehaviour
{
    ParticleSystem ps;
    AudioSource audioSource;

    [Header("Sound Settings")]
    [Range(0f, 2000f)]
    public float volume = 1.0f; // 音量倍率（0〜2000）

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

        // AudioSource初期設定
        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = true;        // ループ再生
            audioSource.volume = volume;    // 初期音量設定
            audioSource.spatialBlend = 0f;  // 2D音にする場合
        }
    }

    void Update()
    {
        if (ps.isPlaying)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.volume = volume; // 再生前に倍率を反映
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
