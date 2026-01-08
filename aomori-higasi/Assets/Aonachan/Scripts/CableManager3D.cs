using UnityEngine;
using System.Collections.Generic;

public class CableManager3D : MonoBehaviour {
    [Header("Line")]
    public Material lineMaterial;
    public float lineWidth = 200f;            // 基本の太さ（開始地点の見た目に合わせたい太さ）

    [Header("Raycast / Drag")]
    public LayerMask socketMask;
    public float rayMaxDistance = 100f;
    public float dragPlaneZ = 0f;

    [Header("Feel (optional)")]
    public bool smoothFollow = true;           // 先端が少し遅れて追従
    public float smoothSpeed = 12f;            // 大きいほどキビキビ
    public float maxDragLength = 0f;           // 0=無制限

    [Header("Snapping")]
    public float snapDistance = 0.5f;          // 近ければ同色右ソケットへ自動スナップ
    public bool  verboseLog   = true;          // 失敗理由のログ表示

    [Header("Thickness (screen-consistent)")]
    public bool keepScreenThickness = true;    // 開始地点の見た目太さを維持する
    public bool roundCaps = true;              // 丸キャップで太い線を綺麗に

    [System.Serializable]
    public class Connection {
        public CableSocket3D left, right;
        public LineRenderer line;
        public CableColor color;
    }
    public List<Connection> connections = new();

    Camera cam;
    CableSocket3D draggingFrom;
    LineRenderer draggingLine;
    Vector3 startPos;
    Vector3 smoothedEnd;                       // 追従用
    float referenceDistance = 1f;              // 開始地点〜カメラの距離（基準）

    void Awake() {
#if UNITY_2023_1_OR_NEWER
        cam = Camera.main ?? Object.FindFirstObjectByType<Camera>();
#else
        cam = Camera.main ?? Object.FindObjectOfType<Camera>();
#endif
        if (socketMask == 0) socketMask = ~0; // 未設定なら全レイヤー
    }

    void Update() {
        // ドラッグ開始
        if (Input.GetMouseButtonDown(0) && draggingFrom == null) {
            var s = RaycastSocket();
            if (s && s.isLeftSide && !s.occupied) BeginDrag(s);
        }

        // ドラッグ中の更新（常に直線）
        if (draggingLine) {
            var target = MouseOnPlaneZ(dragPlaneZ);

            // 長さ制限（任意）
            if (maxDragLength > 0f) {
                var v = target - startPos;
                if (v.magnitude > maxDragLength) target = startPos + v.normalized * maxDragLength;
            }

            // 位置更新
            Vector3 endPos;
            if (smoothFollow) {
                smoothedEnd = Vector3.Lerp(smoothedEnd, target, 1f - Mathf.Exp(-smoothSpeed * Time.deltaTime));
                endPos = smoothedEnd;
            } else {
                endPos = target;
            }
            SetLine(draggingLine, startPos, endPos);

            // 太さ補正（見た目の一貫性）
            if (keepScreenThickness) UpdateLineWidthForScreen(draggingLine, startPos, endPos);
        }

        // ドロップ確定
        if (Input.GetMouseButtonUp(0) && draggingFrom != null) {
            var hitSocket = RaycastSocket();
            TryCompleteDrag(hitSocket);
        }
    }

    CableSocket3D RaycastSocket() {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out var hit, rayMaxDistance, socketMask)
            ? hit.collider.GetComponent<CableSocket3D>() : null;
    }

    Vector3 MouseOnPlaneZ(float z) {
        var ray = cam.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, new Vector3(0, 0, z));
        return plane.Raycast(ray, out float t) ? ray.GetPoint(t) : startPos;
    }

    void BeginDrag(CableSocket3D from) {
        draggingFrom = from;
        startPos = from.transform.position;

        draggingLine = NewLine(from.color.ToUnityColor()); // 線の色を確実に反映
        SetLine(draggingLine, startPos, startPos);         // ゼロ長で開始
        smoothedEnd = startPos;

        // 開始地点のカメラ距離を「基準距離」に
        referenceDistance = (cam.transform.position - startPos).magnitude;
        if (referenceDistance < 0.001f) referenceDistance = 0.001f;

        if (keepScreenThickness) {
            // 開始直後も太さを適用
            UpdateLineWidthForScreen(draggingLine, startPos, startPos);
        }
    }

    void TryCompleteDrag(CableSocket3D to) {
        CableSocket3D final = to;

        // 直接ヒットがなければ、近い同色の右ソケットを探してスナップ
#if UNITY_2023_1_OR_NEWER
        var sockets = Object.FindObjectsByType<CableSocket3D>(FindObjectsSortMode.None);
#else
        var sockets = Object.FindObjectsOfType<CableSocket3D>();
#endif
        if (final == null) {
            float best = Mathf.Infinity;
            Vector3 cursor = MouseOnPlaneZ(dragPlaneZ);
            foreach (var s in sockets) {
                if (s.isLeftSide || s.occupied || s.color != draggingFrom.color) continue;
                float d = Vector3.Distance(s.transform.position, cursor);
                if (d < best) { best = d; final = s; }
            }
            if (best > snapDistance) final = null; // 遠いならスナップしない
        }

        bool ok = (final && !final.isLeftSide && !final.occupied && final.color == draggingFrom.color);

        if (ok) {
            final.occupied = true;
            draggingFrom.occupied = true;

            // 右ソケット位置に直線で確定
            SetLine(draggingLine, draggingFrom.transform.position, final.transform.position);

            // 確定時も太さを最終更新
            if (keepScreenThickness) UpdateLineWidthForScreen(draggingLine, draggingFrom.transform.position, final.transform.position);

            connections.Add(new Connection {
                left = draggingFrom, right = final, line = draggingLine, color = draggingFrom.color
            });

            if (verboseLog) Debug.Log($"✅ Connected {draggingFrom.color}: {draggingFrom.name} -> {final.name}");
            CheckClear();
        } else {
            if (verboseLog) {
                if (to == null) Debug.Log("❌ Drop miss: no socket under cursor");
                else if (to.isLeftSide) Debug.Log("❌ Dropped on LEFT side (need RIGHT)");
                else if (to.occupied) Debug.Log("❌ Dropped on occupied socket");
                else if (to.color != draggingFrom.color) Debug.Log($"❌ Color mismatch: from {draggingFrom.color} -> {to.color}");
            }
            if (draggingLine) Destroy(draggingLine.gameObject);
        }

        draggingFrom = null;
        draggingLine = null;
    }

    void CheckClear() {
        int goal = 0, ok = 0;
#if UNITY_2023_1_OR_NEWER
        var sockets = Object.FindObjectsByType<CableSocket3D>(FindObjectsSortMode.None);
#else
        var sockets = Object.FindObjectsOfType<CableSocket3D>();
#endif
        foreach (var s in sockets) {
            if (!s.isLeftSide) { goal++; if (s.occupied) ok++; }
        }
        if (goal > 0 && ok == goal) {
            Debug.Log("🎉 CLEAR (3D)!");
            // GameManager.Instance?.WinOnce();
        }
    }

    LineRenderer NewLine(Color c) {
        var go = new GameObject("Cable3D");
        var lr = go.AddComponent<LineRenderer>();
        lr.positionCount = 2;                    // 直線
        lr.useWorldSpace = true;
        lr.alignment = LineAlignment.View;       // カメラ向きで見やすく
        lr.widthMultiplier = lineWidth;
        lr.numCornerVertices = roundCaps ? 6 : 0;
        lr.numCapVertices    = roundCaps ? 6 : 0;
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;

        // ラインごとにマテリアルをインスタンス化し、確実に色を反映（URP/Built-in 両対応）
        var mat = new Material(lineMaterial);
        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", c);
        else if (mat.HasProperty("_Color")) mat.SetColor("_Color", c);
        lr.material = mat;

        // 頂点色も設定（シェーダが乗算する場合に備える）
        lr.startColor = c;
        lr.endColor   = c;

        return lr;
    }

    void SetLine(LineRenderer lr, Vector3 a, Vector3 b) {
        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
    }

    // 見かけの太さを開始地点基準で保つ（端ごとに距離補正）
    void UpdateLineWidthForScreen(LineRenderer lr, Vector3 a, Vector3 b) {
        // 端点ごとのカメラ距離
        float d0 = (cam.transform.position - a).magnitude;
        float d1 = (cam.transform.position - b).magnitude;

        // 開始地点距離 : referenceDistance で規格化
        float w0 = lineWidth * (d0 / referenceDistance);
        float w1 = lineWidth * (d1 / referenceDistance);

        // 端ごとに太さを設定（間は補間される）
        lr.startWidth = w0;
        lr.endWidth   = w1;
    }

    public void ResetAll() {
#if UNITY_2023_1_OR_NEWER
        var sockets = Object.FindObjectsByType<CableSocket3D>(FindObjectsSortMode.None);
#else
        var sockets = Object.FindObjectsOfType<CableSocket3D>();
#endif
        foreach (var c in connections) if (c.line) Destroy(c.line.gameObject);
        connections.Clear();
        foreach (var s in sockets) s.occupied = false;

        draggingFrom = null;
        if (draggingLine) Destroy(draggingLine.gameObject);
        draggingLine = null;
    }
}
