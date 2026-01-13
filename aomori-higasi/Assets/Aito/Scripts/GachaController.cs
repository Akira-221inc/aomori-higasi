using UnityEngine;
using UnityEngine.SceneManagement;

public enum Rarity
{
    Gray,   // 灰色 - コモン
    Blue,   // 青色 - レア
    Red,    // 赤色 - スーパーレア
    Gold    // 金色 - ウルトラレア
}

public class GachaController : MonoBehaviour
{
    public Transform handle;
    public Transform capsuleSpawnPoint;
    public GameObject capsulePrefab;

    public Material grayMaterial;
    public Material blueMaterial;
    public Material redMaterial;
    public Material goldMaterial;

    [Range(0, 100)] public float grayRate = 70f;
    [Range(0, 100)] public float blueRate = 20f;
    [Range(0, 100)] public float redRate = 8f;
    [Range(0, 100)] public float goldRate = 2f;

    public float rotationSpeed = 360f;

    [Header("Rarity Scene Switch")]
    public bool switchSceneOnCapsule = false;
    public string graySceneName;
    public string blueSceneName;
    public string redSceneName;
    public string goldSceneName;

    private float rotatedAngle = 0f;
    private bool isRotating = false;

    void Update()
    {
        if (!isRotating) return;

        float rotation = rotationSpeed * Time.deltaTime;
        handle.Rotate(Vector3.down, rotation);
        rotatedAngle += rotation;

        if (rotatedAngle >= 360f)
        {
            FinishRotation();
        }
    }

    public void InsertCoin()
    {
        if (isRotating) return;

        isRotating = true;
        rotatedAngle = 0f;
        Debug.Log("回転中");
    }

    void FinishRotation()
    {
        isRotating = false;
        rotatedAngle = 0f;

        SpawnCapsule();
    }

    void SpawnCapsule()
    {
        GameObject capsule =
            Instantiate(capsulePrefab, capsuleSpawnPoint.position, Quaternion.identity);

        // レア度抽選
        Rarity rarity = DetermineRarity();
        Material material = GetMaterialForRarity(rarity);

        // カプセルの色を設定
        Renderer renderer = capsule.GetComponent<Renderer>();
        if (renderer != null && material != null)
        {
            renderer.material = material;
        }

        Rigidbody rb = capsule.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);
        
        Debug.Log("カプセル排出 - レア度: " + rarity);

        if (switchSceneOnCapsule)
        {
            string sceneToShow = GetSceneNameForRarity(rarity);
            if (!string.IsNullOrEmpty(sceneToShow))
            {
                string returnScene = SceneManager.GetActiveScene().name;
                // 2秒間レア度シーンを表示して元のシーンに戻る
                SceneReturner.StartFlow(sceneToShow, 2f, returnScene);
            }
        }
    }

    Rarity DetermineRarity()
    {
        float random = Random.Range(0f, 100f);
        
        if (random < goldRate)
            return Rarity.Gold;
        else if (random < goldRate + redRate)
            return Rarity.Red;
        else if (random < goldRate + redRate + blueRate)
            return Rarity.Blue;
        else
            return Rarity.Gray;
    }

    Material GetMaterialForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Gray: return grayMaterial;
            case Rarity.Blue: return blueMaterial;
            case Rarity.Red: return redMaterial;
            case Rarity.Gold: return goldMaterial;
            default: return grayMaterial;
        }
    }

    string GetSceneNameForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Gray: return graySceneName;
            case Rarity.Blue: return blueSceneName;
            case Rarity.Red: return redSceneName;
            case Rarity.Gold: return goldSceneName;
            default: return null;
        }
    }
}
