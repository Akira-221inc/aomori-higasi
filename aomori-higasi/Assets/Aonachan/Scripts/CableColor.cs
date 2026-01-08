using UnityEngine;

public enum CableColor { Red, Blue, Yellow, Purple }

public static class CableColorUtil {
    public static Color ToUnityColor(this CableColor c) => c switch {
        CableColor.Red    => new Color(0.95f, 0.25f, 0.25f),
        CableColor.Blue   => new Color(0.20f, 0.45f, 0.95f),
        CableColor.Yellow => new Color(0.98f, 0.86f, 0.20f),
        CableColor.Purple => new Color(0.72f, 0.28f, 0.88f),
        _ => Color.white
    };
}
