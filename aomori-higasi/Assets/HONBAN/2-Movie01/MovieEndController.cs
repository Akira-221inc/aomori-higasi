using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class MovieEndController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName;
    public Camera targetCamera;
    public float blackTime = 5f;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f;

    void Start()
    {
        // Cameraを黒に
        targetCamera.clearFlags = CameraClearFlags.SolidColor;
        targetCamera.backgroundColor = Color.black;

        // 動画を最初は「完全透明」にする
        videoPlayer.targetCameraAlpha = 0f;
        videoPlayer.Stop();

        videoPlayer.loopPointReached += OnMovieEnd;

        StartCoroutine(PlayMovieAfterBlack());
    }

    IEnumerator PlayMovieAfterBlack()
    {
        // 5秒暗転
        yield return new WaitForSeconds(blackTime);

        // 動画を表示
        videoPlayer.targetCameraAlpha = 1f;
        videoPlayer.Play();
    }

    void Update()
    {
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
