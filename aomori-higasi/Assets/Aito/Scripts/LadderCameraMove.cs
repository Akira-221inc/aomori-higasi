using UnityEngine;

public class LadderCameraMove : MonoBehaviour
{
    [Header("Climb Settings")]
    public float moveDistance = 350f;   // 上る距離（Y方向）
    public float climbTime = 15f;        // 上るのにかける時間（秒）

    Vector3 startPos;
    float timer = 0f;
    bool isClimbing = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (!isClimbing) return;

        timer += Time.deltaTime;

        // 0〜1に正規化
        float t = timer / climbTime;
        t = Mathf.Clamp01(t);

        // 開始位置から上方向へ移動
        transform.position = startPos + Vector3.up * moveDistance * t;

        // 上り終わり判定
        if (t >= 1f)
        {
            isClimbing = false;
            OnClimbEnd();
        }
    }

    void OnClimbEnd()
    {
        Debug.Log("梯子を上り切った！");
        // ここで次の演出へ
        // 例：銃を表示、操作を有効化、宇宙シーンへ切り替え
    }
}
