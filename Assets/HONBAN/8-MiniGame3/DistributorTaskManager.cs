using System.Collections;
using UnityEngine;

public class DistributorTaskManager : MonoBehaviour
{
    public DistributorDial[] dials;

    [Header("Dial Lamps (順番一致)")]
    public DialLamp[] lamps;

    [Header("SE")]
    public AudioClip successSE;
    public AudioClip failSE;

    [Header("Sound Settings")]
    [Range(0f, 10f)]
    public float successSEVolume = 1.0f; // 成功音の倍率
    [Range(0f, 10f)]
    public float failSEVolume = 1.0f;    // 失敗音の倍率

    [Header("MiniGame Exit")]
    public MiniGameExit miniGameExit;

    [Header("Clear Delay")]
    public float clearDelay = 1.0f;   // ★ 戻るまでの待ち時間（秒）

    private AudioSource audioSource;
    private int currentIndex = 0;
    private bool isClearing = false;  // ★ 多重実行防止

    void Start()
    {
        ActivateCurrentDial();

        audioSource = GetComponent<AudioSource>();

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
        if (isClearing) return; // ★ クリア後は入力無効

        bool success = dials[currentIndex].CheckSuccess();

        if (success)
        {
            // ★ 成功音（倍率付き）
            if (successSE != null)
                audioSource.PlayOneShot(successSE, successSEVolume);

            if (currentIndex < lamps.Length)
                lamps[currentIndex].SetSuccess();

            currentIndex++;

            if (currentIndex >= dials.Length)
            {
                Debug.Log("TASK CLEAR!");

                // ★ 次のカメラポイントへ進める
                MiniGameProgress.nextPointIndex++;

                isClearing = true;
                StartCoroutine(ClearAndExit());
            }
            else
            {
                ActivateCurrentDial();
            }
        }
        else
        {
            // ★ 失敗音（倍率付き）
            if (failSE != null)
                audioSource.PlayOneShot(failSE, failSEVolume);
        }
    }

    IEnumerator ClearAndExit()
    {
        // ★ 1秒待つ
        yield return new WaitForSeconds(clearDelay);

        if (miniGameExit != null)
            miniGameExit.OnClear();
        else
            Debug.LogWarning("MiniGameExit が設定されていません");
    }
}
