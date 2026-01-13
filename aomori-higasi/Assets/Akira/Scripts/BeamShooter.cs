using UnityEngine;

public class BeamShooter : MonoBehaviour
{
    [SerializeField] private GameObject beamPrefab;
    [SerializeField] private float shootForce = 1000f;
    private Camera mainCamera; // ← SerializeFieldを外して自動取得に変更

    void Start()
    {
        // 自動的にMainCameraタグのついたカメラを探す
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera が見つかりません。MainCameraタグを設定してください。");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootTowardMouse();
        }
    }

    void ShootTowardMouse()
    {
        if (mainCamera == null) return;

        // クリック位置からRayを飛ばす
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        // 弾をカメラの位置から生成
        GameObject beam = Instantiate(beamPrefab, mainCamera.transform.position, Quaternion.identity);
        Vector3 shootDir = (targetPoint - mainCamera.transform.position).normalized;

        Rigidbody rb = beam.GetComponent<Rigidbody>();
        rb.AddForce(shootDir * shootForce);
    }
}
