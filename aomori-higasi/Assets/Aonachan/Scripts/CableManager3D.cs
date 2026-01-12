using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CableManager3D : MonoBehaviour
{
    [Header("Line")]
    public Material lineMaterial;
    public float lineWidth = 200f;

    [Header("Raycast / Drag")]
    public LayerMask socketMask;
    public float rayMaxDistance = 100f;
    public float dragPlaneZ = 0f;

    [Header("Snapping")]
    public float snapDistance = 0.5f;

    [Header("MiniGame Exit")]
    public MiniGameExit miniGameExit;

    [Header("Clear Delay")]
    public float clearDelay = 1.0f;

    Camera cam;
    CableSocket3D draggingFrom;
    LineRenderer draggingLine;
    Vector3 startPos;

    bool isClearing = false;   // ★ これが超重要

    void Awake()
    {
        cam = Camera.main;
        if (socketMask == 0) socketMask = ~0;
    }

    void Update()
    {
        if (isClearing) return;

        if (Input.GetMouseButtonDown(0) && draggingFrom == null)
        {
            var s = RaycastSocket();
            if (s && s.isLeftSide && !s.occupied)
                BeginDrag(s);
        }

        if (draggingLine)
        {
            Vector3 pos = MouseOnPlaneZ(dragPlaneZ);
            SetLine(draggingLine, startPos, pos);
        }

        if (Input.GetMouseButtonUp(0) && draggingFrom != null)
        {
            TryCompleteDrag(RaycastSocket());
        }
    }

    CableSocket3D RaycastSocket()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit, rayMaxDistance, socketMask)
            ? hit.collider.GetComponent<CableSocket3D>()
            : null;
    }

    Vector3 MouseOnPlaneZ(float z)
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, new Vector3(0, 0, z));
        plane.Raycast(ray, out float t);
        return ray.GetPoint(t);
    }

    void BeginDrag(CableSocket3D from)
    {
        draggingFrom = from;
        startPos = from.transform.position;

        draggingLine = NewLine(from.color.ToUnityColor());
        SetLine(draggingLine, startPos, startPos);
    }

    void TryCompleteDrag(CableSocket3D to)
    {
        if (to && !to.isLeftSide && !to.occupied && to.color == draggingFrom.color)
        {
            to.occupied = true;
            draggingFrom.occupied = true;

            SetLine(draggingLine,
                draggingFrom.transform.position,
                to.transform.position);

            CheckClear();
        }
        else
        {
            Destroy(draggingLine.gameObject);
        }

        draggingFrom = null;
        draggingLine = null;
    }

    void CheckClear()
    {
        if (isClearing) return;

        var sockets = FindObjectsOfType<CableSocket3D>();
        int goal = 0, ok = 0;

        foreach (var s in sockets)
        {
            if (!s.isLeftSide)
            {
                goal++;
                if (s.occupied) ok++;
            }
        }

        if (goal > 0 && ok == goal)
        {
            Debug.Log("🎉 CABLE CLEAR");

            isClearing = true;
            StartCoroutine(ClearAndExit());
        }
    }

    IEnumerator ClearAndExit()
    {
        yield return new WaitForSeconds(clearDelay);
        MiniGameProgress.nextPointIndex++;

        if (miniGameExit != null)
            miniGameExit.OnClear();
        else
            Debug.LogWarning("MiniGameExit が設定されていません");
    }

    LineRenderer NewLine(Color c)
    {
        var go = new GameObject("Cable");
        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.widthMultiplier = lineWidth;

        var mat = new Material(lineMaterial);
        if (mat.HasProperty("_Color")) mat.color = c;
        lr.material = mat;

        return lr;
    }

    void SetLine(LineRenderer lr, Vector3 a, Vector3 b)
    {
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
    }
}
