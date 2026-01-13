using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagershot : MonoBehaviour
{
    public static GameManagershot Instance;

    [Header("Score")]
    public int score = 0;          // 今の破壊数（Inspectorで確認できる）
    public int clearCount = 10;    // クリア条件
    public bool isGameClear = false;

    [Header("Scene")]
    public string nextSceneName = "HubScene";
    public float clearDelay = 1.0f;

    void Awake()
    {
        // シングルトン安全化
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore()
    {
        if (isGameClear) return;

        Debug.Log("AddScore 呼ばれた");

        score++;
        Debug.Log("破壊数: " + score);

        if (score >= clearCount)
        {
            GameClear();
        }
    }

    void GameClear()
    {
        if (isGameClear) return;   // ★ 念のため二重防止

        isGameClear = true;
        Debug.Log("🎉 GAME CLEAR 🎉");

        // ★ 進行度を進める
        MiniGameProgress.nextPointIndex++;
        Debug.Log("MiniGameProgress.nextPointIndex = " + MiniGameProgress.nextPointIndex);

        // ゲーム停止
        Time.timeScale = 0f;

        // ★ コルーチンで遷移
        StartCoroutine(ClearAndMoveScene());
    }

    IEnumerator ClearAndMoveScene()
    {
        // Time.timeScale = 0 でも待てる
        yield return new WaitForSecondsRealtime(clearDelay);

        // 念のため戻す
        Time.timeScale = 1f;

        SceneManager.LoadScene(nextSceneName);
    }
}
