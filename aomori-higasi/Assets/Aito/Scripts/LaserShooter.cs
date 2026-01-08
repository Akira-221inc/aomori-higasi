using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public LineRenderer lineRenderer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootLaser();
        }
    }

    void ShootLaser()
{
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    // デバッグ用：Sceneビューで確認
    Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, 1f);

    lineRenderer.enabled = true;

    // ★ Inspector の Positions は完全無視される
    lineRenderer.SetPosition(0, ray.origin);
    lineRenderer.SetPosition(1, ray.origin + ray.direction * 50f);

    RaycastHit hit;
    if (Physics.Raycast(ray, out hit, 50f))
    {
        if (hit.collider.CompareTag("Meteor"))
        {
            Destroy(hit.collider.gameObject);
            GameManagershot.Instance.AddScore();
        }
    }

    Invoke(nameof(HideLaser), 0.05f);
}

    void HideLaser()
    {
        lineRenderer.enabled = false;
    }
}
