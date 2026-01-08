// GridManager.cs
using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour {
    public static GridManager Instance { get; private set; }

    // グリッド座標(Vector2Int: x,z) -> タイル
    private Dictionary<Vector2Int, TileBase> map = new Dictionary<Vector2Int, TileBase>();

    void Awake() {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // ★ Awakeではマップ構築をしない（他オブジェクトのAwake順の影響を避ける）
    }

    void Start() {
        RebuildMap();   // 全Awake完了後に構築
        Propagate();    // 初期伝播
    }

    public void RebuildMap() {
        map.Clear();
#if UNITY_2023_1_OR_NEWER
        var tiles = FindObjectsByType<TileBase>(FindObjectsSortMode.None);
#else
        var tiles = FindObjectsOfType<TileBase>();
#endif
        foreach (var t in tiles) {
            // Transform からグリッド座標を算出（Awake順に依存しない）
            Vector3 p = t.transform.position;
            Vector2Int gp = new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.z));
            map[gp] = t;
        }
    }

    // ★ここが無かったのでエラーになっていました
    public void Propagate() {
        // 1) リセット
        foreach (var kv in map) kv.Value.SetPowered(false);

        // 2) ソースを起点にキューへ
        Queue<TileBase> q = new Queue<TileBase>();
        HashSet<TileBase> visited = new HashSet<TileBase>();
        foreach (var kv in map) {
            var tile = kv.Value;
            if (tile.IsSource) {
                tile.SetPowered(true);
                q.Enqueue(tile);
                visited.Add(tile);
            }
        }

        // 3) BFS 伝播
        while (q.Count > 0) {
            var cur = q.Dequeue();

            // 現在タイルのグリッド座標（Transformから都度算出）
            Vector3 cp = cur.transform.position;
            Vector2Int cgp = new Vector2Int(Mathf.RoundToInt(cp.x), Mathf.RoundToInt(cp.z));

            foreach (var dir in cur.OutgoingDirs()) {
                Vector2Int np = cgp + DirUtil.ToOffset(dir);
                if (!map.TryGetValue(np, out var next)) continue;

                // 隣がこの方向から受け取れるか
                if (!next.AcceptsFrom(dir)) continue;

                if (!next.Powered) next.SetPowered(true);
                if (visited.Add(next)) q.Enqueue(next);
            }
        }
    }

    // （お好み）どこからでも安全に伝播要求できるラッパー
#if UNITY_2023_1_OR_NEWER
    public static void RequestPropagate() {
        (Instance ?? FindFirstObjectByType<GridManager>())?.Propagate();
    }
#else
    public static void RequestPropagate() {
        (Instance ?? FindObjectOfType<GridManager>())?.Propagate();
    }
#endif
}
