using UnityEngine;
using UnityEngine.UI;

public class SampleInspectionCube : MonoBehaviour
{
    [Header("薬品Cube")]
    public GameObject[] medicineCubes;

    [Header("探し物表示")]
    public Image targetColorImage;

    int currentIndex = 0;
    int correctIndex;

    void Start()
    {
        // 正解をランダムに決める
        correctIndex = Random.Range(0, medicineCubes.Length);

        // 探し物リストに正解の色を表示
        Color targetColor = medicineCubes[correctIndex]
            .GetComponent<Renderer>().material.color;
        targetColorImage.color = targetColor;

        // 最初の表示更新
        UpdateCube();
    }

    // 右ボタン
    public void OnRight()
    {
        currentIndex++;

        if (currentIndex >= medicineCubes.Length)
        {
            currentIndex = 0;
        }

        UpdateCube();
    }

    // 左ボタン
    public void OnLeft()
    {
        currentIndex--;

        if (currentIndex < 0)
        {
            currentIndex = medicineCubes.Length - 1;
        }

        UpdateCube();
    }

    // 決定ボタン
    public void OnDecide()
    {
        if (currentIndex == correctIndex)
        {
            Debug.Log("成功！");
        }
        else
        {
            Debug.Log("失敗！");
        }
    }

    // Cubeの表示切り替え
    void UpdateCube()
    {
        for (int i = 0; i < medicineCubes.Length; i++)
        {
            medicineCubes[i].SetActive(i == currentIndex);
        }
    }
}
