using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameExit : MonoBehaviour
{
    public string hubSceneName = "HubScene";

    // クリア時に呼ぶ
    public void OnClear()
    {
        SceneManager.LoadScene(hubSceneName);
    }
}
