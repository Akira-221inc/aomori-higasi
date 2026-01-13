using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class TileBase : MonoBehaviour {
    [Tooltip("このタイルの開口方向（導通のある辺）")]
    public Dir connections = Dir.None;

    public Vector2Int GridPos { get; private set; }
    public bool Powered { get; private set; } = false;

    protected virtual void Awake() {
        SnapToGrid();
    }

    protected virtual void OnValidate() {
        SnapToGrid();
    }

    protected void SnapToGrid() {
        // グリッド：1ユニット、XZ 平面
        var p = transform.position;
        int gx = Mathf.RoundToInt(p.x);
        int gz = Mathf.RoundToInt(p.z);
        GridPos = new Vector2Int(gx, gz);
        transform.position = new Vector3(gx, p.y, gz);
    }

    public virtual bool IsSource => false;

    public virtual IEnumerable<Dir> OutgoingDirs() {
        if (connections.Has(Dir.N)) yield return Dir.N;
        if (connections.Has(Dir.E)) yield return Dir.E;
        if (connections.Has(Dir.S)) yield return Dir.S;
        if (connections.Has(Dir.W)) yield return Dir.W;
    }

    public virtual bool AcceptsFrom(Dir incoming) {
        // 隣から来た電流を受け取れるか（逆側に開口があるか）
        return connections.Has(DirUtil.Opposite(incoming));
    }

    public void SetPowered(bool on) {
        if (Powered == on) return;
        Powered = on;
        OnPowerChanged(on);
    }

    protected virtual void OnPowerChanged(bool on) { /* 派生で可視化 */ }

    // クリック時のフック（回転タイルなどで使う）
    protected virtual void OnMouseDown() { }
}
