using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMultiPointMover : MonoBehaviour
{
    [Header("Points (順番に移動)")]
    public List<Transform> points = new List<Transform>();

    [Header("Timing")]
    public float moveDuration = 2.0f;     // 1点あたりの移動時間
    public float waitAtPoint = 0.3f;      // 到着後の待機

    [Header("Options")]
    public bool moveRotation = true;      // 回転も合わせる
    public bool playOnStart = true;       // Startで自動再生
    public bool loop = false;             // 最後まで行ったらループ

    [Header("Easing")]
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    int index = 0;
    Coroutine seqCo;

    void Start()
    {
        if (playOnStart) StartSequence(fromIndex: 0);
    }

    // -------------------------
    // 公開API
    // -------------------------
    public void StartSequence(int fromIndex = 0)
    {
        if (points == null || points.Count == 0)
        {
            Debug.LogWarning("[CameraMultiPointMover] points が空です");
            return;
        }

        index = Mathf.Clamp(fromIndex, 0, points.Count - 1);

        if (seqCo != null) StopCoroutine(seqCo);
        seqCo = StartCoroutine(SequenceCoroutine());
    }

    public void StopSequence()
    {
        if (seqCo != null)
        {
            StopCoroutine(seqCo);
            seqCo = null;
        }
    }

    public void GoTo(int targetIndex)
    {
        if (points == null || points.Count == 0) return;
        targetIndex = Mathf.Clamp(targetIndex, 0, points.Count - 1);

        index = targetIndex;

        if (seqCo != null) StopCoroutine(seqCo);
        seqCo = StartCoroutine(MoveTo(points[index]));
    }

    public void Next()
    {
        if (points == null || points.Count == 0) return;

        index++;
        if (index >= points.Count)
        {
            if (loop) index = 0;
            else { StopSequence(); return; }
        }

        if (seqCo != null) StopCoroutine(seqCo);
        seqCo = StartCoroutine(MoveTo(points[index]));
    }

    // -------------------------
    // 内部処理
    // -------------------------
    IEnumerator SequenceCoroutine()
    {
        while (true)
        {
            yield return MoveTo(points[index]);
            if (waitAtPoint > 0f) yield return new WaitForSeconds(waitAtPoint);

            index++;
            if (index >= points.Count)
            {
                if (loop) index = 0;
                else break;
            }
        }

        seqCo = null;
    }

    IEnumerator MoveTo(Transform target)
    {
        if (target == null) yield break;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float t = 0f;
        float inv = (moveDuration <= 0.0001f) ? 99999f : 1f / moveDuration;

        while (t < 1f)
        {
            t += Time.deltaTime * inv;
            float k = ease.Evaluate(Mathf.Clamp01(t));

            transform.position = Vector3.Lerp(startPos, endPos, k);
            if (moveRotation)
                transform.rotation = Quaternion.Slerp(startRot, endRot, k);

            yield return null;
        }

        // 最後に誤差を潰す
        transform.position = endPos;
        if (moveRotation) transform.rotation = endRot;
    }
}
