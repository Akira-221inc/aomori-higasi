using System.Collections;
using UnityEngine;

/// <summary>
/// Attach this to the EngineHole GameObject (with Collider) to receive fuel from
/// a ParticleSystem-based fuel effect. When particles collide with this object
/// the script will add fuel every tickInterval seconds (default 0.1s) by
/// amountPerTick (default 1 => 1%).
///
/// Requirements:
/// - The EngineHole object should have a Collider (isTrigger not required for
///   particle collision). The ParticleSystem must have "Send Collision Messages"
///   enabled so that OnParticleCollision is called.
/// - Assign a reference to the GameManager in the inspector (or the script will
///   try to find one automatically).
/// </summary>
public class EngineHoleFuelReceiver : MonoBehaviour
{
    [Tooltip("Reference to the GameManager (optional, will try to find automatically)")]
    public GameManager gameManager;

    [Tooltip("How often to add fuel while being hit by the fuel effect (seconds)")]
    public float tickInterval = 0.1f;

    [Tooltip("Amount of fuel added per tick (default 1 => 1% when maxFuel==100)")]
    public float amountPerTick = 1f;

    [Tooltip("How long (s) to keep considering the fuel effect active since the last particle hit")] 
    public float hitGracePeriod = 0.15f;

    float lastHitTime = -999f;
    bool injecting = false;

    void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
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
