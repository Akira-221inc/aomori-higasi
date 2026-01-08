using UnityEngine;

/// <summary>
/// Spawns fuel cubes on mouse click. Cubes fall with gravity and can be detected
/// by EngineHoleFuelReceiver to add fuel.
/// </summary>
public class FuelCubeSpawner : MonoBehaviour
{
    [Header("Cube Settings")]
    [Tooltip("Prefab of the fuel cube to spawn (should have Rigidbody and Collider)")]
    public GameObject fuelCubePrefab;

    [Tooltip("If true, spawn at cursor position; if false, use spawnPoint")]
    public bool spawnAtCursor = true;

    [Tooltip("Offset from cursor position towards camera (negative = in front of surface)")]
    public float spawnOffsetFromSurface = 0.3f;

    [Tooltip("Distance from camera to spawn if no object is hit (when spawnAtCursor=true)")]
    public float defaultSpawnDistance = 5f;

    [Tooltip("Fixed spawn point (used when spawnAtCursor=false)")]
    public Transform spawnPoint;

    [Tooltip("Initial velocity applied to spawned cubes (relative to world)")]
    public Vector3 initialVelocity = new Vector3(0, -2f, 0);

    [Tooltip("Additional forward velocity from spawn point (pushes cubes away from surface)")]
    public float forwardVelocity = 1f;

    [Tooltip("Add random velocity variance for shower effect")]
    public bool useShowerEffect = false;

    [Tooltip("Random spread range for shower spawn position (XZ plane)")]
    public float showerSpreadRadius = 0.1f;

    [Tooltip("Random velocity variance (applied to each axis)")]
    public Vector3 velocityVariance = new Vector3(0.05f, 0.05f, 0.05f);

    [Tooltip("Automatically destroy cubes after this many seconds (0 = never)")]
    public float cubeLifetime = 5f;

    [Header("Input")]
    [Tooltip("Mouse button to use (0=left, 1=right, 2=middle)")]
    public int mouseButton = 0;

    [Tooltip("Optional: only spawn when clicking on objects with this tag (leave empty for any click)")]
    public string clickTargetTag = "";

    [Tooltip("Spawn interval when holding down the mouse button (seconds)")]
    public float spawnInterval = 0.1f;

    private float nextSpawnTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(mouseButton))
        {
            // Check spawn interval
            if (Time.time < nextSpawnTime)
                return;

            Vector3 spawnPosition;
            Vector3 spawnNormal = Vector3.up; // Default normal direction
            bool shouldSpawn = false;

            // Determine spawn position
            if (spawnAtCursor)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                // Check if we need to verify click target tag
                if (!string.IsNullOrEmpty(clickTargetTag))
                {
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        if (hit.collider.CompareTag(clickTargetTag))
                        {
                            spawnPosition = hit.point - hit.normal * spawnOffsetFromSurface;
                            spawnNormal = hit.normal;
                            shouldSpawn = true;
                        }
                        else
                        {
                            spawnPosition = Vector3.zero;
                        }
                    }
                    else
                    {
                        spawnPosition = Vector3.zero;
                    }
                }
                else
                {
                    // No tag filter - spawn at raycast hit or default distance
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        spawnPosition = hit.point - hit.normal * spawnOffsetFromSurface;
                        spawnNormal = hit.normal;
                    }
                    else
                    {
                        spawnPosition = ray.GetPoint(defaultSpawnDistance);
                        spawnNormal = -ray.direction;
                    }
                    shouldSpawn = true;
                }
            }
            else
            {
                // Use fixed spawn point
                spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
                spawnNormal = spawnPoint != null ? spawnPoint.forward : Vector3.forward;
                shouldSpawn = true;
            }

            if (shouldSpawn)
            {
                SpawnFuelCube(spawnPosition, spawnNormal);
                nextSpawnTime = Time.time + spawnInterval;
            }
        }
    }

    void SpawnFuelCube(Vector3 position, Vector3 surfaceNormal)
    {
        if (fuelCubePrefab == null)
        {
            Debug.LogWarning("FuelCubeSpawner: fuelCubePrefab is not assigned!");
            return;
        }

        // Apply shower effect spread if enabled
        if (useShowerEffect)
        {
            Vector2 randomOffset = Random.insideUnitCircle * showerSpreadRadius;
            position += new Vector3(randomOffset.x, 0, randomOffset.y);
        }

        GameObject cube = Instantiate(fuelCubePrefab, position, Quaternion.identity);

        // Apply initial velocity
        Rigidbody rb = cube.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Base velocity (world space, typically downward)
            Vector3 velocity = initialVelocity;
            
            // Add outward push from surface (makes it spray away from click point)
            velocity += surfaceNormal * forwardVelocity;
            
            // Add random variance for shower effect
            if (useShowerEffect)
            {
                velocity += new Vector3(
                    Random.Range(-velocityVariance.x, velocityVariance.x),
                    Random.Range(-velocityVariance.y, velocityVariance.y),
                    Random.Range(-velocityVariance.z, velocityVariance.z)
                );
            }
            
            rb.linearVelocity = velocity;
        }
        else
        {
            Debug.LogWarning("FuelCubeSpawner: Spawned cube has no Rigidbody!");
        }

        // Auto-destroy after lifetime
        if (cubeLifetime > 0)
        {
            Destroy(cube, cubeLifetime);
        }

        Debug.Log("Fuel cube spawned at " + position);
    }

    // Test spawning from editor context menu
    [ContextMenu("Spawn Test Cube")]
    void SpawnTestCube()
    {
        Vector3 testPos = spawnPoint != null ? spawnPoint.position : transform.position;
        Vector3 testNormal = spawnPoint != null ? spawnPoint.forward : Vector3.forward;
        SpawnFuelCube(testPos, testNormal);
    }
}
