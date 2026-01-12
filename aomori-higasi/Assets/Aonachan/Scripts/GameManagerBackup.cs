using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerBackup : MonoBehaviour
{
    public static GameManagerBackup Instance { get; private set; }

    [Header("UI")]
    [SerializeField] GameObject winPanel;

    [Header("Scene")]
    public string nextSceneName = "HubScene";
    public float clearDelay = 1.0f;

    bool finished = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (winPanel)
            winPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    /// <summary>
    /// クリア確定（1回だけ）
    /// </summary>
    public void WinOnce()
    {
        if (finished) return;

        finished = true;
        Debug.Log("🎉 CLEAR!");

        // ★ 進行度を進める
        MiniGameProgress.nextPointIndex++;
        Debug.Log("MiniGameProgress.nextPointIndex = " + MiniGameProgress.nextPointIndex);

        if (winPanel)
            winPanel.SetActive(true);

        // ゲーム停止
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

    // ===== 既存機能（必要なら使う） =====

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        Time.timeScale = 1f;
        var i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i + 1);
    }
}
