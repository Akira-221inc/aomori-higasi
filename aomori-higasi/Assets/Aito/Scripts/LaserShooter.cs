using UnityEngine;

public class LaserShooter : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public AudioClip explosionSE;  

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

        Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, 1f);

        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, ray.origin);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * 50f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50f))
        {
            if (hit.collider.CompareTag("Meteor"))
            {
                // 💥 爆発音
                AudioSource.PlayClipAtPoint(
                    explosionSE,
                    hit.collider.transform.position
                );

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