using System.Collections;
using UnityEngine;

/// <summary>
/// Attach this to the EngineHole GameObject (with Collider set to Trigger) to receive fuel from
/// transparent cube objects or particles. When cubes enter/stay in the trigger, fuel is added continuously.
/// When particles hit, fuel is added based on collision events.
///
/// Requirements:
/// - The EngineHole object should have a Collider with "Is Trigger" enabled for cubes.
/// - Fuel cubes should have Rigidbody and Collider components.
/// - For particles: ParticleSystem must have "Send Collision Messages" enabled.
/// - Optionally assign a tag to fuel cubes for filtering (e.g. "FuelCube").
/// - Assign a reference to the GameManager in the inspector (or the script will
///   try to find one automatically).
/// </summary>
public class EngineHoleFuelReceiver : MonoBehaviour
{
    [Tooltip("Reference to the GameManager (optional, will try to find automatically)")]
    public GameManager gameManager;

    [Tooltip("How often to add fuel while fuel cubes are in contact (seconds)")]
    public float tickInterval = 0.1f;

    [Tooltip("Amount of fuel added per tick (default 1 => 1% when maxFuel==100)")]
    public float amountPerTick = 1f;

    [Tooltip("Optional tag to filter fuel cubes (leave empty to accept any collider)")]
    public string fuelCubeTag = "FuelCube";

    [Tooltip("How long (s) to keep injecting after last trigger contact")] 
    public float hitGracePeriod = 0.15f;

    float lastHitTime = -999f;
    bool injecting = false;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Called when a fuel cube enters the trigger collider.
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (IsFuelCube(other))
        {
            ReceiveFuelHit();
        }
    }

    /// <summary>
    /// Called every frame while a fuel cube stays inside the trigger.
    /// </summary>
    void OnTriggerStay(Collider other)
    {
        if (IsFuelCube(other))
        {
            lastHitTime = Time.time;

            if (!injecting)
                StartCoroutine(InjectLoop());
        }
    }

    /// <summary>
    /// Check if the collider is a valid fuel cube.
    /// </summary>
    bool IsFuelCube(Collider other)
    {
        // If tag is specified, check it; otherwise accept any collider
        if (!string.IsNullOrEmpty(fuelCubeTag))
        {
            return other.CompareTag(fuelCubeTag);
        }
        return true;
    }

    /// <summary>
    /// Called when particles from a ParticleSystem collide with this object.
    /// The ParticleSystem must have "Send Collision Messages" enabled.
    /// Note: The collider must NOT be a trigger for particle collision to work.
    /// </summary>
    void OnParticleCollision(GameObject other)
    {
        Debug.Log("Particle collision detected from: " + other.name);
        
        // Update last hit time and start injection if not already running
        ReceiveFuelHit();
    }

    /// <summary>
    /// Called externally (e.g. by a ParticleSystem forwarder) when fuel particles
    /// hit this EngineHole. This replaces relying on Unity calling
    /// OnParticleCollision on this object (Unity invokes OnParticleCollision on
    /// the ParticleSystem GameObject instead).
    /// </summary>
    public void ReceiveFuelHit()
    {
        lastHitTime = Time.time;

        if (!injecting)
            StartCoroutine(InjectLoop());
    }

    IEnumerator InjectLoop()
    {
        injecting = true;

        // Keep injecting while we recently received particle collisions.
        while (Time.time - lastHitTime <= hitGracePeriod)
        {
            if (gameManager != null)
            {
                gameManager.AddFuel(amountPerTick);
            }
            else
            {
                Debug.LogWarning("EngineHoleFuelReceiver: GameManager not found.");
            }

            yield return new WaitForSeconds(tickInterval);
        }

        injecting = false;
    }

    // Optional helper for testing in editor: simulate a short fuel hit
    [ContextMenu("SimulateFuelHit")] 
    void SimulateFuelHit()
    {
        ReceiveFuelHit();
    }
}
