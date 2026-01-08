// GameManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerBackup : MonoBehaviour {
    public static GameManagerBackup Instance { get; private set; }
    [SerializeField] GameObject winPanel;

    bool finished = false;

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        if (winPanel) winPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void WinOnce() {
        if (finished) return;
        finished = true;
        Debug.Log("🎉 CLEAR!");
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReloadScene() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene() {
        Time.timeScale = 1f;
        var i = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(i + 1);
    }
}

