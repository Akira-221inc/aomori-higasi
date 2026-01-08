using UnityEngine;

public class MiniGame06Manager : MonoBehaviour
{
    public Medicine[] medicines;
    public string correctId;

    int index = 0;

    void Start()
    {
        UpdateView();
    }

    void UpdateView()
    {
        foreach (var m in medicines)
            m.gameObject.SetActive(false);

        medicines[index].gameObject.SetActive(true);
    }

    public void Next()
    {
        index = (index + 1) % medicines.Length;
        UpdateView();
    }

    public void Prev()
    {
        index--;
        if (index < 0)
            index = medicines.Length - 1;

        UpdateView();
    }

    public void Decide()
    {
        if (medicines[index].medicineId == correctId)
            Debug.Log("成功！");
        else
            Debug.Log("失敗");
    }
}
