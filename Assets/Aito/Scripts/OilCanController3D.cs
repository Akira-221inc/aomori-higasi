using UnityEngine;

public class OilCanController3D : MonoBehaviour
{
    public ParticleSystem oilParticle;
    public float tiltAngle = 60f;
    public float height = 3f;

    void Update()
    {
        FollowMouse();

        if (Input.GetMouseButton(0))
        {
            Tilt(true);
            if (!oilParticle.isPlaying)
                oilParticle.Play();
        }
        else
        {
            Tilt(false);
            oilParticle.Stop();
        }
    }

    void FollowMouse()
{
    Vector3 mouse = Input.mousePosition;

    // カメラからの距離（ここを調整）
    mouse.z = 5f;

    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouse);
    transform.position = worldPos;
}


    void Tilt(bool isTilting)
    {
        float angle = isTilting ? tiltAngle : 0f;
        transform.rotation = Quaternion.Euler(angle, 0, 0);
    }
}
