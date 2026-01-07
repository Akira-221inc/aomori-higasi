using UnityEngine;


public class RotatableTile : TileBase
{
    public override bool IsSource => false;

    protected override void OnMouseDown()
    {
        Debug.Log("RotatableTile clicked: " + name);
        // 見た目も回す（Y軸 90°）
        transform.Rotate(0f, 90f, 0f);

        // 接続ビットを回転
        connections = DirUtil.RotateCW(connections);

        // 再伝播
        if (GridManager.Instance != null) GridManager.Instance.Propagate();
    }
}
