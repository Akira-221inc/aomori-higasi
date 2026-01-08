using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraReturnManager : MonoBehaviour
{
    public static CameraReturnManager I { get; private set; }

    // シーン名ごとにカメラ状態を保存
    private struct CamState
    {
        public Vector3 pos;
        public Quaternion rot;
        public float fov;
    }

    private readonly Dictionary<string, CamState> _saved = new();

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (I == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // 元シーンから出る直前に呼ぶ
    public void SaveCurrentCameraForScene(string sceneName)
    {
        var cam = Camera.main;
        if (!cam)
        {
            Debug.LogWarning("[CameraReturnManager] Camera.main が見つかりません（MainCameraタグが有効なカメラが必要）");
            return;
        }

        _saved[sceneName] = new CamState
        {
            pos = cam.transform.position,
            rot = cam.transform.rotation,
            fov = cam.fieldOfView
        };

        // Debug.Log($"[CameraReturnManager] Saved camera for {sceneName}");
    }

    // シーンロード後に自動で復元（保存があるシーンだけ）
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!_saved.TryGetValue(scene.name, out var s))
            return;

        var cam = Camera.main;
        if (!cam)
        {
            Debug.LogWarning("[CameraReturnManager] Restore対象シーンで Camera.main が見つかりません");
            return;
        }

        cam.transform.SetPositionAndRotation(s.pos, s.rot);
        cam.fieldOfView = s.fov;

        // Debug.Log($"[CameraReturnManager] Restored camera for {scene.name}");
    }
}
