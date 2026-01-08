using UnityEngine;

public class Meteor : MonoBehaviour
{
    float speed;

    void Start()
    {
        speed = Random.Range(3f, 7f); // 常識的な速度
    }

    void Update()
    {
        Vector3 target = Vector3.zero; // プレイヤー（画面中心）
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );
    }
}
