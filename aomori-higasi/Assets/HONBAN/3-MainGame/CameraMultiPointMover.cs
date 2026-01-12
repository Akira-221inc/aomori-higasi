using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DoorOpenAtPoint
{
    public int pointIndex;    // このポイントに到達する前にドアを開く
    public int[] doorIndices; // AutoDoorController の doors 配列のインデックス
}

public class CameraMultiPointMover : MonoBehaviour
{
    [Header("Points")]
    public List<Transform> points = new List<Transform>();

    [Header("Movement Settings")]
    public float moveDuration = 2f;
    public float waitBeforeMove = 2f;         // ドアが開く時間
    public AutoDoorController doorController; // ドア制御
    public DoorOpenAtPoint[] doorsToOpenAtPoints;

    [Header("Events")]
    public UnityEvent<int> onArrivePoint;     // 移動完了イベント

    private int index = 0;
    private Coroutine moveCo = null;

    void Start()
    {
        StartSequence(0);
    }

    // -------------------------
    // 公開API
    // -------------------------
    public void StartSequence(int fromIndex = 0)
    {
        if (moveCo != null)
            return;

        index = Mathf.Clamp(fromIndex, 0, points.Count - 1);
        moveCo = StartCoroutine(MoveSequence());
    }

    public void ResumeFrom(int fromIndex)
    {
        if (moveCo != null)
            StopCoroutine(moveCo);

        index = Mathf.Clamp(fromIndex, 0, points.Count - 1);
        moveCo = StartCoroutine(MoveSequence());
    }

    public void StopSequence()
    {
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
    }

    // -------------------------
    // 内部処理
    // -------------------------
    private IEnumerator MoveSequence()
    {
        while (index < points.Count)
        {
            // ポイントに応じてドアを開く
            foreach (var dto in doorsToOpenAtPoints)
            {
                if (dto.pointIndex == index && doorController != null)
                {
                    doorController.OpenDoors(dto.doorIndices);
                    yield return new WaitForSeconds(waitBeforeMove);
                }
            }

            // ポイントまで移動
            Transform target = points[index];
            Vector3 startPos = transform.position;
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / moveDuration);
                transform.position = Vector3.Lerp(startPos, target.position, t);
                yield return null;
            }

            transform.position = target.position;

            // 移動完了イベント
            onArrivePoint?.Invoke(index);

            index++;
        }

        moveCo = null;
    }
}
