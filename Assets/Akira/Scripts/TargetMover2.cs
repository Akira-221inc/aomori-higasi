using UnityEngine;

public class TargetMover2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float destroyZ = -10f;

    void Update()
    {
        // 的を前（手前）に動かす
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

        // 一定距離に来たら破壊
        if (transform.position.z < destroyZ)
        {
            Destroy(gameObject);
        }
    }
}
