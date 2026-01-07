using UnityEngine;

public class SinkTile : TileBase {
    [ColorUsage(true,true)]
    public Color onColor = Color.yellow;

    protected override void OnPowerChanged(bool on) {
        var r = GetComponentInChildren<Renderer>();
        if (!r) return;

        var mpb = new MaterialPropertyBlock();
        r.GetPropertyBlock(mpb);
        mpb.SetColor("_EmissionColor", on ? onColor : Color.black);
        r.SetPropertyBlock(mpb);
        if (on) r.material.EnableKeyword("_EMISSION");

        // ★ここを追加：LEDが点いた瞬間に勝利処理を呼ぶ
        if (on) GameManagerBackup.Instance?.WinOnce();
    }
}
