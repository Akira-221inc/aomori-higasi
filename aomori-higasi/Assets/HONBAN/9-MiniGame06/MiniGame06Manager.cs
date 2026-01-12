using UnityEngine;

public class MiniGame06Manager : MonoBehaviour
{
    public Medicine[] medicines;
    public string correctId;

    [Header("SE")]
    public AudioSource audioSource;
    public AudioClip clickSE;
    public AudioClip successSE;
    public AudioClip failSE;

    int index = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        UpdateView();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int mask = LayerMask.GetMask("Button");

            if (Physics.Raycast(ray, out hit, 100f, mask))
            {
                var button = hit.collider.GetComponent<Button3D>();
                if (button != null)
                {
                    PlaySE(clickSE);   // ← ボタン押下音
                    OnButtonPressed(button.type);
                }
            }
        }
    }

    void OnButtonPressed(Button3D.ButtonType type)
    {
        Debug.Log("押された: " + type);

        switch (type)
        {
            case Button3D.ButtonType.Left:
                Prev();
                break;
            case Button3D.ButtonType.Right:
                Next();
                break;
            case Button3D.ButtonType.Decide:
                Decide();
                break;
        }
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
        index = (index - 1 + medicines.Length) % medicines.Length;
        UpdateView();
    }

    public void Decide()
    {
        Debug.Log("現在: " + medicines[index].medicineId);

        if (medicines[index].medicineId == correctId)
        {
            Debug.Log("成功！");
            PlaySE(successSE);
        }
        else
        {
            Debug.Log("失敗");
            PlaySE(failSE);
        }
    }

    void PlaySE(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}
