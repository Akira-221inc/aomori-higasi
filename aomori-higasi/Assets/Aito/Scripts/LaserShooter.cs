using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    [Header("Laser Settings")]
    public LineRenderer lineRenderer;      // レーザー表示用 LineRenderer
    public float laserLength = 50f;        // レーザーの長さ
    public float laserDisplayTime = 0.05f; // レーザー表示時間

    [Header("Sound Settings")]
    public AudioClip explosionSE;          // 隕石爆発時の音
    [Range(0f, 20f)]
    public float explosionSEVolume = 1.0f; // 音量倍率（Inspectorで調整可能）

    void Update()
    {
        // マウス左クリックでレーザー発射
        if (Input.GetMouseButtonDown(0))
        {
            ShootLaser();
        }
    }

    void ShootLaser()
    {
        // カメラからマウス位置に向かうレイを作成
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // デバッグ用に赤い線を表示（Sceneビュー）
        Debug.DrawRay(ray.origin, ray.direction * laserLength, Color.red, 1f);

        // レーザー表示
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * laserLength);

        // レイが何かに当たったか判定
        if (Physics.Raycast(ray, out RaycastHit hit, laserLength))
        {
            // 当たったオブジェクトが Meteor タグか確認
            if (hit.collider.CompareTag("Meteor"))
            {
                // 💥 爆発音を倍率付きで再生
                if (explosionSE != null)
                {
                    AudioSource.PlayClipAtPoint(
                        explosionSE,
                        hit.collider.transform.position,
                        explosionSEVolume
                    );
                }

                // 隕石オブジェクトを破壊
                Destroy(hit.collider.gameObject);

                // スコア加算
                GameManagershot.Instance.AddScore();
            }
        }

        // レーザーを短時間表示して非表示にする
        Invoke(nameof(HideLaser), laserDisplayTime);
    }

    void HideLaser()
    {
        lineRenderer.enabled = false;
    }
}
