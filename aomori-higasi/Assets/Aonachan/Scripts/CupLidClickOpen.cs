using System.Collections;
using UnityEngine;

public class CupLidClickOpen : MonoBehaviour
{
    public Camera cam;            // クリックRayを飛ばすカメラ
    public Transform cupPivot;    // 回転させる支点（CupPivot）
    public Collider cupCollider;  // クリック対象のCollider（CupのBoxCollider）

    public Vector3 closedEuler = new Vector3(0, 0, 0);
    public Vector3 openEuler   = new Vector3(-90f, 0, 0); // 開く角度（ここ調整）
    public float animTime = 0.25f;

    bool isOpen = false;
    bool isAnimating = false;

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (isAnimating) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
            {
                // CupのColliderをクリックしたら反応
                if (hit.collider == cupCollider)
                {
                    StartCoroutine(AnimateLid(!isOpen));
                }
            }
        }
    }

    IEnumerator AnimateLid(bool open)
    {
        isAnimating = true;

        Quaternion from = cupPivot.localRotation;
        Quaternion to = Quaternion.Euler(open ? openEuler : closedEuler);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, animTime);
            cupPivot.localRotation = Quaternion.Slerp(from, to, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        cupPivot.localRotation = to;
        isOpen = open;
        isAnimating = false;
    }
}
