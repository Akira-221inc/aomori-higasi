using UnityEngine;

public class VoxelWaterTank : MonoBehaviour
{
    public Transform waterSurface;   // 上昇させる水面
    public float waterRisePerParticle = 0.005f; // 1粒あたり上昇量
    public float maxWaterHeight = 1.0f; // 最大高さ

    private float currentHeight = 0f;

    void OnTriggerEnter(Collider other)
    {
        // 粒子が入ったか判断
        if (other.CompareTag("Water"))
        {
            AddWater();
            Destroy(other.gameObject, 0.1f); // タンクの中に入ったら粒は消す
        }
    }

    void AddWater()
    {
        currentHeight += waterRisePerParticle;
        currentHeight = Mathf.Clamp(currentHeight, 0, maxWaterHeight);

        Vector3 pos = waterSurface.localPosition;
        pos.y = currentHeight;
        waterSurface.localPosition = pos;
    }
}
