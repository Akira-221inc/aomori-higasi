using UnityEngine;

public class FuelFill : MonoBehaviour
{
    public Transform fuelMesh; // 燃料メッシュ
    public float maxHeight = 10f; // 高さの最大値（10）

    void Update()
    {
        // ゲージの値（0〜100）を0〜1に変換
        float t = OilGameManager.Instance.fuelSlider.value / 100f;

        // X,Z は常に10、Y だけ 0→10 に変化
        fuelMesh.localScale = new Vector3(10f, t * maxHeight, 10f);
    }
}
