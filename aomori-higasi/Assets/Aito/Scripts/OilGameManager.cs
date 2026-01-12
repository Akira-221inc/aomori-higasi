using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class OilGameManager : MonoBehaviour
{
    public static OilGameManager Instance;

    [Header("UI")]
    public Slider fuelSlider;

    [Header("Clear")]
    public string nextSceneName = "HubScene";
    public float clearDelay = 1.0f;

    float fuel = 0f;
    bool isCleared = false;   // ★ 多重クリア防止

    void Awake()
    {
        Instance = this;

        fuel = 0f;
        if (fuelSlider != null)
            fuelSlider.value = 0f;

        Time.timeScale = 1f;
    }

    public void AddFuel(float amount)
    {
        if (isCleared) return;

        fuel += amount;
        fuel = Mathf.Clamp(fuel, 0, 100);

        if (fuelSlider != null)
            fuelSlider.value = fuel;

        // ★ パーティクル削除（元の処理そのまま）
        ParticleSystem ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ParticleSystem.Particle[] particles =
                new ParticleSystem.Particle[ps.main.maxParticles];

            int count = ps.GetParticles(particles);

            for (int i = 0; i < count; i++)
            {
                particles[i].remainingLifetime = 0;
            }

            ps.SetParticles(particles, count);
        }

        if (fuel >= 100f)
        {
            GameClear();
        }
    }

    void GameClear()
    {
        if (isCleared) return;

        isCleared = true;
        Debug.Log("🎉 GAME CLEAR");

        // ★ ミニゲーム進行度を進める
        MiniGameProgress.nextPointIndex++;
        Debug.Log("MiniGameProgress.nextPointIndex = " + MiniGameProgress.nextPointIndex);

        // ★ ゲーム停止
        Time.timeScale = 0f;

        // ★ 少し待ってからシーン遷移
        StartCoroutine(ClearAndMoveScene());
    }

    IEnumerator ClearAndMoveScene()
    {
        // timeScale = 0 でも待てる
        yield return new WaitForSecondsRealtime(clearDelay);

        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}
