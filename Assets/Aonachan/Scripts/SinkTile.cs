using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SinkTile : TileBase
{
    [ColorUsage(true, true)]
    public Color onColor = Color.yellow;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource; // ここにAudioSource（未設定なら自動取得）
    [SerializeField] private AudioClip ledOnClip;   // LED点灯SE
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    bool wasOn = false; // 前フレーム（前回通知）の状態を覚える

    void Awake()
    {
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
        if (sfxSource != null)
        {
            // ★保険：開始時に鳴らないように
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;
            // 2Dで鳴らす（距離で小さくならない）
            sfxSource.spatialBlend = 0f;
        }
    }

    protected override void OnPowerChanged(bool on)
    {
        // ---- LED表示 ----
        var r = GetComponentInChildren<Renderer>();
        if (r)
        {
            var mpb = new MaterialPropertyBlock();
            r.GetPropertyBlock(mpb);
            mpb.SetColor("_EmissionColor", on ? onColor : Color.black);
            r.SetPropertyBlock(mpb);

            if (on) r.material.EnableKeyword("_EMISSION");
            // 必要ならOFF時も明示的に切る（環境によっては残る）
            else r.material.DisableKeyword("_EMISSION");
        }

        // ---- ★SE：OFF→ON の瞬間だけ鳴らす ----
        if (!wasOn && on)
        {
            if (sfxSource != null && ledOnClip != null)
                sfxSource.PlayOneShot(ledOnClip, volume);

            // ★LEDが点いた瞬間に勝利処理
            GameManagerBackup.Instance?.WinOnce();
        }

        wasOn = on;
    }
}
