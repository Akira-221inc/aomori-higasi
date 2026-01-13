using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CableSocket3D : MonoBehaviour {
    public CableColor color;
    public bool isLeftSide = true;
    [HideInInspector] public bool occupied = false;

    static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    static readonly int ColorID     = Shader.PropertyToID("_Color");

    void OnValidate() {
        ApplyColor();
        gameObject.name = (isLeftSide ? "L_" : "R_") + color;
    }

    void Awake() {
        ApplyColor(); // 実行時生成直後にも反映
    }

    void ApplyColor() {
        var r = GetComponentInChildren<Renderer>();
        if (!r) return;

        var c = color.ToUnityColor();

        // MPBで色を乗せる（material/sharedMaterial は触らない）
        var mpb = new MaterialPropertyBlock();
        r.GetPropertyBlock(mpb);

        // URP系は _BaseColor、内蔵/Unlit 等は _Color のことが多い
        if (r.sharedMaterial && r.sharedMaterial.HasProperty(BaseColorID))
            mpb.SetColor(BaseColorID, c);
        else
            mpb.SetColor(ColorID, c);

        r.SetPropertyBlock(mpb);
    }
}
