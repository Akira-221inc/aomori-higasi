using UnityEngine;

[System.Serializable]
public class Door
{
    public Transform doorTransform;    // ドアオブジェクト
    public Vector3 moveDirection;      // 開く方向
    public float moveDistance = 3f;    // 移動量
    public float moveDuration = 2f;    // 開く時間

    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public float elapsed = 0f;
}

public class AutoDoorController : MonoBehaviour
{
    public Door[] doors;

    void Start()
    {
        foreach (var door in doors)
        {
            if (door.doorTransform != null)
                door.startPos = door.doorTransform.position;
        }
    }

    public void OpenDoors(int[] indices)
    {
        foreach (int i in indices)
        {
            if (i < 0 || i >= doors.Length) continue;

            Door d = doors[i];
            d.elapsed = 0f;
            d.isMoving = true;
            if (d.doorTransform != null)
                d.startPos = d.doorTransform.position;
        }
    }

    void Update()
    {
        foreach (var door in doors)
        {
            if (!door.isMoving || door.doorTransform == null) continue;

            door.elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(door.elapsed / door.moveDuration);
            door.doorTransform.position = door.startPos + door.moveDirection.normalized * door.moveDistance * t;

            if (t >= 1f) door.isMoving = false;
        }
    }
}
