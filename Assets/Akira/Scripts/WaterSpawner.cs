using UnityEngine;

public class WaterSpawner : MonoBehaviour
{
    public GameObject waterPrefab;
    public float spawnInterval = 0.05f;

    private float timer = 0f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnWater();
                timer = 0f;
            }
        }
    }

    void SpawnWater()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 spawnPos;

        // 何かに当たるならそこに出す
        if (Physics.Raycast(ray, out RaycastHit hit, 2000f))
        {
            spawnPos = hit.point + Vector3.up * 0.3f;
        }
        else
        {
            // 何にも当たらない場合は、カメラ前方に固定距離で出す
            spawnPos = cam.transform.position + cam.transform.forward * 10f;
        }

        // 生成
        GameObject obj = Instantiate(waterPrefab, spawnPos, Quaternion.identity);

        // 初速を消す（自然落下にする）
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
