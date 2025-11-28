using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this to the ParticleSystem GameObject that represents the fuel spray.
/// It forwards particle collision events to any EngineHoleFuelReceiver on the
/// collided GameObject.
/// Requirements: ParticleSystem Collision module must have "Send Collision Messages" enabled.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ParticleFuelCollisionForwarder : MonoBehaviour
{
    ParticleSystem ps;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Called on the ParticleSystem's GameObject when a particle collides with another GameObject
    void OnParticleCollision(GameObject other)
    {
        if (ps == null)
            ps = GetComponent<ParticleSystem>();

        // Gather collision events (not strictly needed here but keeps API use correct)
        int num = ps.GetCollisionEvents(other, collisionEvents);

        if (num <= 0)
            return;

        // Try to find an EngineHoleFuelReceiver on the object we hit
        var receiver = other.GetComponent<EngineHoleFuelReceiver>();
        if (receiver != null)
        {
            receiver.ReceiveFuelHit();
        }
    }
}
