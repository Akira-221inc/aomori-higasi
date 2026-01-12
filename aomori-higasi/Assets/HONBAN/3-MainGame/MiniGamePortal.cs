using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamePortal : MonoBehaviour
{
    public string hubSceneName = "HubScene";
    public string miniGameSceneName = "MiniGame01";

    public CameraMultiPointMover mover;
    public int triggerPointIndex = 0;

    void Start()
    {
        mover.onArrivePoint.AddListener(OnArrivePoint);
    }

    void OnArrivePoint(int index)
    {
        // ★ 進行中のポイントと一致したら開始
        if (index == triggerPointIndex &&
            index == MiniGameProgress.nextPointIndex)
        {
            EnterMiniGame();
        }
    }

    void EnterMiniGame()
    {
        mover.StopSequence();

        if (CameraReturnManager.I != null)
            CameraReturnManager.I.SaveCurrentCameraForScene(hubSceneName);

        SceneManager.LoadScene(miniGameSceneName);
    }
}
