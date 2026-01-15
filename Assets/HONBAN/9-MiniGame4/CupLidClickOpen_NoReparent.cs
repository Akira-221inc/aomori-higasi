using System.Collections;
using UnityEngine;

public class CupLidClickOpen_NoReparent : MonoBehaviour
{
    public Camera cam;                 // Rayを飛ばすカメラ
    public Transform pivot;            // 回転中心
    public Transform lid;              // フタ本体
    public Collider lidCollider;       // クリック対象

    [Header("Rotation")]
    public Vector3 closedEuler = new Vector3(0, 0, 0);
    public Vector3 openEuler   = new Vector3(-90f, 0, 0);
    public float animTime = 0.25f;

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip openSE;
    [Range(0f, 10f)]
    public float openSEVolume = 1.0f; // ★ 効果音の倍率

    bool isOpen = false;
    bool isAnimating = false;

    Quaternion closedRotRel;
    Vector3 closedPosRel;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        // AudioSource 自動取得（なければ追加）
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (pivot == null || lid == null)
        {
            Debug.LogError("[CupLidClickOpen_NoReparent] pivot または lid が未設定です");
            enabled = false;
            return;
        }

        // 現在の姿勢を閉状態として保存
        closedPosRel = pivot.InverseTransformPoint(lid.position);
        closedRotRel = Quaternion.Inverse(pivot.rotation) * lid.rotation;
    }

    void Update()
    {
        if (isAnimating) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                if (hit.collider == lidCollider)
                {
                    // 開くときだけ効果音
                    if (!isOpen)
                        PlaySE(openSE, openSEVolume);

                    StartCoroutine(AnimateLid(!isOpen));
                }
            }
        }
    }

    IEnumerator AnimateLid(bool open)
    {
        isAnimating = true;

        Quaternion fromRel = Quaternion.Inverse(pivot.rotation) * lid.rotation;
        Quaternion toRel =
            Quaternion.Euler(open ? openEuler : closedEuler) * closedRotRel;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, animTime);

            Quaternion rel = Quaternion.Slerp(
                fromRel,
                toRel,
                Mathf.SmoothStep(0f, 1f, t)
            );

            lid.rotation = pivot.rotation * rel;
            lid.position = pivot.TransformPoint(closedPosRel);

            yield return null;
        }

        lid.rotation = pivot.rotation * toRel;
        lid.position = pivot.TransformPoint(closedPosRel);

        isOpen = open;
        isAnimating = false;
    }

    // ★ 音量倍率付きで再生
    void PlaySE(AudioClip clip, float volume = 1.0f)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip, volume);
    }
}
