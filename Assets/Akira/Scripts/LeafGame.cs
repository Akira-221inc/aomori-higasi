using UnityEngine;
using UnityEngine.InputSystem;

public class Leaf : MonoBehaviour
{
    [Header("浮遊設定")]
    [SerializeField] private float floatSpeed = 1f; // 浮遊速度
    [SerializeField] private float floatAmount = 0.5f; // 浮遊の幅
    [SerializeField] private Vector3 movementRange = new Vector3(0.8f, 0.8f, 0.3f); // 各軸の移動範囲
    
    [Header("回転設定")]
    [SerializeField] private Vector3 rotationSpeed = new Vector3(10f, 20f, 15f); // 各軸の回転速度（度/秒）
    [SerializeField] private Vector3 rotationRange = new Vector3(15f, 15f, 30f); // 各軸の回転の幅（度）
    
    [Header("ドラッグ設定")]
    [SerializeField] private float dragDepth = 10f; // ドラッグ時のZ座標距離
    
    private Vector3 startPosition;
    private bool isDragging = false;
    private Vector3 offset;
    private Camera mainCamera;
    private Collider objectCollider;
    
    // ふわふわアニメーション用
    private float randomOffsetX;
    private float randomOffsetY;
    private float randomOffsetZ;
    private float randomSpeedX;
    private float randomSpeedY;
    private float randomSpeedZ;
    
    // 回転アニメーション用
    private float randomRotOffsetX;
    private float randomRotOffsetY;
    private float randomRotOffsetZ;
    private float randomRotSpeedX;
    private float randomRotSpeedY;
    private float randomRotSpeedZ;
    private Quaternion startRotation;
    
    void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Cameraが見つかりません！");
        }
        
        objectCollider = GetComponent<Collider>();
        if (objectCollider == null)
        {
            Debug.LogError("Colliderが必要です！葉っぱにBox Colliderを追加してください。");
        }
        
        startPosition = transform.position;
        startRotation = transform.rotation;
        
        // 各葉っぱで異なる動きをするためのランダムオフセット
        randomOffsetX = Random.Range(0f, 2f * Mathf.PI);
        randomOffsetY = Random.Range(0f, 2f * Mathf.PI);
        randomOffsetZ = Random.Range(0f, 2f * Mathf.PI);
        
        // 各葉っぱで異なる速度を設定
        randomSpeedX = Random.Range(0.7f, 1.3f);
        randomSpeedY = Random.Range(0.8f, 1.5f);
        randomSpeedZ = Random.Range(0.5f, 1.2f);
        
        // 回転用のランダムオフセットと速度
        randomRotOffsetX = Random.Range(0f, 2f * Mathf.PI);
        randomRotOffsetY = Random.Range(0f, 2f * Mathf.PI);
        randomRotOffsetZ = Random.Range(0f, 2f * Mathf.PI);
        randomRotSpeedX = Random.Range(0.6f, 1.4f);
        randomRotSpeedY = Random.Range(0.6f, 1.4f);
        randomRotSpeedZ = Random.Range(0.6f, 1.4f);
        
        Debug.Log($"{gameObject.name} が初期化されました");
    }
    
    void Update()
    {
        // マウス入力の処理（Raycastを使用）
        HandleMouseInput();
        
        if (!isDragging)
        {
            // 3D空間でうようよと浮遊する動き
            float time = Time.time * floatSpeed;
            
            // X軸方向の動き（左右にうようよ）
            float newX = startPosition.x + Mathf.Sin((time + randomOffsetX) * randomSpeedX) * movementRange.x;
            
            // Y軸方向の動き（上下にふわふわ）
            float newY = startPosition.y + Mathf.Sin((time + randomOffsetY) * randomSpeedY) * movementRange.y;
            
            // Z軸方向の動き（前後に少し動く）
            float newZ = startPosition.z + Mathf.Sin((time + randomOffsetZ) * randomSpeedZ) * movementRange.z;
            
            transform.position = new Vector3(newX, newY, newZ);
            
            // 回転の計算
            float rotTime = Time.time;
            
            // 各軸の回転角度を計算（サインカーブで揺らす）
            float rotX = Mathf.Sin((rotTime * rotationSpeed.x * 0.01f + randomRotOffsetX) * randomRotSpeedX) * rotationRange.x;
            float rotY = Mathf.Sin((rotTime * rotationSpeed.y * 0.01f + randomRotOffsetY) * randomRotSpeedY) * rotationRange.y;
            float rotZ = Mathf.Sin((rotTime * rotationSpeed.z * 0.01f + randomRotOffsetZ) * randomRotSpeedZ) * rotationRange.z;
            
            // 基本の回転に揺れを加える
            transform.rotation = startRotation * Quaternion.Euler(rotX, rotY, rotZ);
        }
        else
        {
            // ドラッグ中の処理
            Vector3 mousePos = GetMouseWorldPosition();
            transform.position = mousePos + offset;
        }
    }
    
    void HandleMouseInput()
    {
        // 新しいInput Systemを使用
        var mouse = Mouse.current;
        if (mouse == null) return;
        
        // マウスの左ボタンが押された瞬間
        if (mouse.leftButton.wasPressedThisFrame && !isDragging)
        {
            Ray ray = mainCamera.ScreenPointToRay(mouse.position.ReadValue());
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log($"{gameObject.name} がクリックされました");
                    isDragging = true;
                    Vector3 mousePos = GetMouseWorldPosition();
                    offset = transform.position - mousePos;
                }
            }
        }
        
        // マウスの左ボタンが離された
        if (mouse.leftButton.wasReleasedThisFrame && isDragging)
        {
            Debug.Log($"{gameObject.name} がリリースされました");
            isDragging = false;
            startPosition = transform.position;
        }
    }
    
    private Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null) return Vector3.zero;
        
        var mouse = Mouse.current;
        if (mouse == null) return Vector3.zero;
        
        Vector3 mousePos = mouse.position.ReadValue();
        // カメラからの距離を設定
        mousePos.z = dragDepth;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }
    
    // ゴミ箱から呼ばれる、葉っぱを削除する
    public void Dispose()
    {
        Destroy(gameObject);
    }
    
    // ドラッグ中かどうかを返す（ゴミ箱が判定に使用）
    public bool IsReleased()
    {
        return !isDragging;
    }
}
