using System.Collections;
using UnityEngine;

public class CupLidClickOpen_NoReparent : MonoBehaviour
{
    public Camera cam;                 // Rayを飛ばすカメラ
    public Transform pivot;            // 回転中心（あなたのpivot）
    public Transform lid;              // 回す対象（あなたのCaseTop）
    public Collider lidCollider;       // クリック対象（CaseTopのCollider）

    // pivotを基準にした「閉→開」の回転角（ローカルっぽく考えてOK）
    public Vector3 closedEuler = new Vector3(0, 0, 0);
    public Vector3 openEuler   = new Vector3(-90f, 0, 0);
    public float animTime = 0.25f;

    bool isOpen = false;
    bool isAnimating = false;

    // アニメ用：閉状態の基準姿勢（pivot基準）
    Quaternion closedRotRel;
    Vector3 closedPosRel;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        if (pivot == null || lid == null)
        {
            Debug.LogError("[CupLidClickOpen_NoReparent] pivot または lid が未設定です");
            enabled = false;
            return;
        }

        // 現在の姿勢を「閉状態」として保存
        // pivot空間での相対位置・相対回転を覚える
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
                    StartCoroutine(AnimateLid(!isOpen));
                }
            }
        }
    }

    IEnumerator AnimateLid(bool open)
    {
        isAnimating = true;

        // 開閉の目標相対回転（pivot基準）
        Quaternion fromRel = Quaternion.Inverse(pivot.rotation) * lid.rotation;
        Quaternion toRel = Quaternion.Euler(open ? openEuler : closedEuler) * closedRotRel;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, animTime);

            Quaternion rel = Quaternion.Slerp(fromRel, toRel, Mathf.SmoothStep(0f, 1f, t));

            // pivotを基準に、相対姿勢→ワールドへ戻す
            lid.rotation = pivot.rotation * rel;
            lid.position = pivot.TransformPoint(closedPosRel);

            yield return null;
        }

        lid.rotation = pivot.rotation * toRel;
        lid.position = pivot.TransformPoint(closedPosRel);

        isOpen = open;
        isAnimating = false;
    }
}
