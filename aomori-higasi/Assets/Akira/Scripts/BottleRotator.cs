using UnityEngine;

public class BottleRotator : MonoBehaviour
{
    public GameObject bottlePrefab;  // 自分のボトルPrefab
    public int count = 5;            // 回す本数
    public float radius = 2f;        // 半径（距離）
    public float rotateSpeed = 30f;  // 回転速度（角度/秒）

    private GameObject[] bottles;
    private float angleOffset;

    void Start()
    {
        bottles = new GameObject[count];

        // 等間隔でボトルを生成
        for (int i = 0; i < count; i++)
        {
            float angle = (360f / count) * i;
            Vector3 pos = transform.position + new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
                0,
                Mathf.Sin(angle * Mathf.Deg2Rad) * radius
            );

            bottles[i] = Instantiate(bottlePrefab, pos, Quaternion.identity, transform);
        }
    }

    void Update()
    {
        // 親（このオブジェクト）をY軸回転 → 全部回る
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
