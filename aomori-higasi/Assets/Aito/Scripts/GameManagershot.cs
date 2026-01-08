using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagershot : MonoBehaviour
{
    public static GameManagershot Instance;

    public int score = 0;          // 今の破壊数（Inspectorで確認できる）
    public int clearCount = 10;    // クリア条件
    public bool isGameClear = false;

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
        isGameClear = true;
        Debug.Log("🎉 GAME CLEAR 🎉");

        Time.timeScale = 0f; // ゲーム停止

        // 将来用（今はコメントアウト）
        // SceneManager.LoadScene("ClearScene");
    }
}
