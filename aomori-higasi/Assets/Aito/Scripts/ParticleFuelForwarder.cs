using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this to the ParticleSystem GameObject.
/// When particles collide with an EngineHole, this script forwards the collision
/// to the EngineHoleFuelReceiver component.
/// 
/// Requirements:
/// - ParticleSystem must have Collision module enabled
/// - Collision Type = World
/// - Send Collision Messages = ON
/// </summary>
public class ParticleFuelForwarder : MonoBehaviour
{
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("ParticleFuelForwarder: Collision detected with " + other.name);

        // Try to get EngineHoleFuelReceiver from the collided object
        EngineHoleFuelReceiver receiver = other.GetComponent<EngineHoleFuelReceiver>();
        
        if (receiver != null)
        {
            Debug.Log("ParticleFuelForwarder: Forwarding to EngineHoleFuelReceiver");
            receiver.ReceiveFuelHit();
        }
        else
        {
            Debug.LogWarning("ParticleFuelForwarder: No EngineHoleFuelReceiver found on " + other.name);
        }
    }
}
