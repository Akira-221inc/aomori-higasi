using UnityEngine;

[RequireComponent(typeof(Collider))]
public class RotatableTile : TileBase
{
    public override bool IsSource => false;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;   // ここにAudioSource（無ければ自動取得）
    [SerializeField] private AudioClip rotateClip;    // 回転SE
    [Range(0f, 1f)]
    [SerializeField] private float volume = 1f;

    void Awake()
    {
        // 未設定なら同じObjectのAudioSourceを拾う
        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();

        // ★保険：再生開始時に勝手に鳴らないようにする
        if (sfxSource != null)
        {
            sfxSource.playOnAwake = false;
            sfxSource.loop = false;

            // 2Dで鳴らしたいなら（3D空間減衰させたくない）
            sfxSource.spatialBlend = 0f;
        }
    }

    protected override void OnMouseDown()
    {
        Debug.Log("RotatableTile clicked: " + name);

        // 見た目も回す（Y軸 90°）
        transform.Rotate(0f, 90f, 0f);

        // 接続ビットを回転
        connections = DirUtil.RotateCW(connections);

        // 再伝播
        if (GridManager.Instance != null) GridManager.Instance.Propagate();

        // ★回転した瞬間にSE
        if (sfxSource != null && rotateClip != null)
        {
            sfxSource.PlayOneShot(rotateClip, volume);
        }
    }
}
