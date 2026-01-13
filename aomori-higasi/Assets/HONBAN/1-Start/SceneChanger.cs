using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("遷移先シーン名（Build Settingsに登録必須）")]
    [SerializeField]
    private string nextSceneName;

    public void ChangeScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("遷移先シーン名が設定されていません");
        }
    }
}
