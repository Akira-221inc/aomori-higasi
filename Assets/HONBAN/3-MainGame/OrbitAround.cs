using UnityEngine;

public class OrbitAround : MonoBehaviour
{
    public Transform center;    // 回転の中心（球体のTransform）
    public float speed = 50f;   // 回転速度（度/秒）
    public Vector3 axis = Vector3.up; // 回転軸（Y軸周り）

    void Update()
    {
        if (center != null)
        {
            // 自身を中心の周りに回転させる
            transform.RotateAround(center.position, axis, speed * Time.deltaTime);
        }
    }
}
