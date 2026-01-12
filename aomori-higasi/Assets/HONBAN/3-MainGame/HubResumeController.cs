
using UnityEngine;

public class HubResumeController : MonoBehaviour
{
    public CameraMultiPointMover mover;

    void Start()
    {
        // ★ 次に進むポイントから再開
        mover.ResumeFrom(MiniGameProgress.nextPointIndex);
    }
}
