using UnityEngine;

public class TargetMover : MonoBehaviour
{
    public float moveSpeed = 5f;   // 近づく速さ
    public Transform player;       // プレイヤーの位置

    void Start()
    {
        if (player == null)
        {
            // メインカメラを自動でターゲットに
            player = Camera.main.transform;
        }
    }

    void Update()
    {
        // プレイヤーに向かって移動
        Vector3 dir = (player.position - transform.position).normalized;
        transform.position += dir * moveSpeed * Time.deltaTime;

        // 少し回転して自然な動きに
        transform.Rotate(Vector3.up * 50f * Time.deltaTime, Space.World);
    }
}
