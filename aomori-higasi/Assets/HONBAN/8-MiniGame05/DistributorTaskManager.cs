using UnityEngine;

public class DistributorTaskManager : MonoBehaviour
{
    public DistributorDial[] dials;

    [Header("Dial Lamps (順番一致)")]
    public DialLamp[] lamps;

    [Header("SE")]
    public AudioClip successSE;
    public AudioClip failSE;

    private AudioSource audioSource;
    private int currentIndex = 0;

    void Start()
    {
        ActivateCurrentDial();

        // AudioSource 取得
        audioSource = GetComponent<AudioSource>();

        // 起動時は全ランプ通常色
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].SetNormal();
        }
    }

    void ActivateCurrentDial()
    {
        for (int i = 0; i < dials.Length; i++)
        {
            dials[i].isActive = (i == currentIndex);
        }
    }

    public void OnPress()
    {
        if (currentIndex >= dials.Length) return;

        bool success = dials[currentIndex].CheckSuccess();

        if (success)
        {
            // 🔊 成功音
            if (successSE != null)
            {
                audioSource.PlayOneShot(successSE);
            }

            // 💡 対応ランプを成功色
            if (currentIndex < lamps.Length)
            {
                lamps[currentIndex].SetSuccess();
            }

            currentIndex++;

            if (currentIndex >= dials.Length)
            {
                Debug.Log("TASK CLEAR!");
            }
            else
            {
                ActivateCurrentDial();
            }
        }
        else
        {
            // 🔊 失敗音
            if (failSE != null)
            {
                audioSource.PlayOneShot(failSE);
            }
        }
    }
}
