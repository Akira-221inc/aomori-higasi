using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MiniGame06Manager : MonoBehaviour
{
    public Medicine[] medicines;
    public string correctId;

    [Header("SE")]
    public AudioSource audioSource;
    public AudioClip clickSE;
    public AudioClip successSE;
    public AudioClip failSE;

    [Header("Sound Settings")]
    [Range(0f, 10f)]
    public float clickSEVolume = 1.0f;   // クリック音倍率
    [Range(0f, 10f)]
    public float successSEVolume = 1.0f; // 成功音倍率
    [Range(0f, 10f)]
    public float failSEVolume = 1.0f;    // 失敗音倍率

    [Header("Clear")]
    public string nextSceneName = "HubScene";
    public float clearDelay = 1.0f;

    int index = 0;
    bool isCleared = false;   // ★ 多重クリア防止

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        UpdateView();
    }

    void Update()
    {
        if (isCleared) return; // ★ クリア後は操作無効

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
                    PlaySE(clickSE, clickSEVolume);
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
            Debug.Log("🎉 成功！");
            PlaySE(successSE, successSEVolume);
            GameClear();
        }
        else
        {
            Debug.Log("失敗");
            PlaySE(failSE, failSEVolume);
        }
    }

    void GameClear()
    {
        if (isCleared) return;

        isCleared = true;

        // ★ ミニゲーム進行度を進める
        MiniGameProgress.nextPointIndex++;
        Debug.Log("MiniGameProgress.nextPointIndex = " + MiniGameProgress.nextPointIndex);

        // ★ 少し待ってからシーン遷移
        StartCoroutine(ClearAndMoveScene());
    }

    IEnumerator ClearAndMoveScene()
    {
        yield return new WaitForSeconds(clearDelay);
        SceneManager.LoadScene(nextSceneName);
    }

    // ★ 音量倍率付きで再生
    void PlaySE(AudioClip clip, float volume = 1.0f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
