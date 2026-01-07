using System;
using UnityEngine;

[System.Flags]
public enum Dir {
    None = 0,
    N = 1 << 0,
    E = 1 << 1,
    S = 1 << 2,
    W = 1 << 3
}

public static class DirUtil {
    public static Vector2Int ToOffset(Dir d) {
        switch (d) {
            case Dir.N: return new Vector2Int(0, 1);
            case Dir.E: return new Vector2Int(1, 0);
            case Dir.S: return new Vector2Int(0, -1);
            case Dir.W: return new Vector2Int(-1, 0);
            default: return Vector2Int.zero;
        }
    }
    public static Dir Opposite(Dir d) {
        switch (d) {
            case Dir.N: return Dir.S;
            case Dir.E: return Dir.W;
            case Dir.S: return Dir.N;
            case Dir.W: return Dir.E;
            default: return Dir.None;
        }
    }
    public static Dir RotateCW(Dir mask) {
        Dir r = Dir.None;
        if ((mask & Dir.N) != 0) r |= Dir.E;
        if ((mask & Dir.E) != 0) r |= Dir.S;
        if ((mask & Dir.S) != 0) r |= Dir.W;
        if ((mask & Dir.W) != 0) r |= Dir.N;
        return r;
    }
    public static bool Has(this Dir mask, Dir d) => (mask & d) != 0;
}
