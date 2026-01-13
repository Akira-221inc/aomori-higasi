using UnityEngine;

public class SourceTile : TileBase {
    [Tooltip("常にONの電源。見た目の発光などは任意。")]
    public bool active = true;

    public override bool IsSource => active;

    protected override void OnPowerChanged(bool on) {
        // 電源自身の見た目（任意）
        var r = GetComponentInChildren<Renderer>();
        if (!r) return;
        var mpb = new MaterialPropertyBlock();
        r.GetPropertyBlock(mpb);
        mpb.SetColor("_EmissionColor", on ? Color.cyan * 2f : Color.black);
        r.SetPropertyBlock(mpb);
        if (on) r.material.EnableKeyword("_EMISSION");
    }
}
