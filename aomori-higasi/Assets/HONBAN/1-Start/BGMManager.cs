using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    private AudioSource audioSource;

    // 🔽 ここに「BGMを止めたいシーン名」を手打ちで書く
    [SerializeField]
    private string[] stopBGMSenes;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool shouldStop = false;

        foreach (string sceneName in stopBGMSenes)
        {
            if (scene.name == sceneName)
            {
                shouldStop = true;
                break;
            }
        }

        if (shouldStop)
        {
            audioSource.Pause(); // 一時停止（再開可能）
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
