using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class MovieEndController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    void Start()
    {
        // 動画が終わったら呼ばれる
        videoPlayer.loopPointReached += OnMovieEnd;
    }

    void Update()
    {
        // マウス左クリック
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                GoToNextScene();
            }

            lastClickTime = Time.time;
        }
    }

    void OnMovieEnd(VideoPlayer vp)
    {
        GoToNextScene();
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
