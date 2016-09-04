using UnityEngine;
using System.Collections;
using System;

public class Destructable : MonoBehaviour {

    public float DamageThreshold;
    public float Health;

    void OnCollisionEnter(Collision collision)
    {
        var rb = collision.rigidbody;
        if (rb != null && Health > 0)
        {
            float damageFactor = 1;
            var damager = collision.gameObject.GetComponent<Damager>();

            if (damager != null) {
                damageFactor = damager.Damage;
            }
            var magnitude = Vector3.Magnitude(collision.relativeVelocity) * rb.mass;
            if (magnitude > DamageThreshold)
            {
                var damage = magnitude * damageFactor;

                Health -= magnitude * damageFactor;
                if (Health < 0)
                {
                    OnDestroyed(collision);
                }
                else
                {
                    OnDamaged(collision);
                }
            }
        }
    }

    public virtual void OnDamaged(Collision collision)
    {

    }

    public virtual void OnDestroyed(Collision collision)
    {
        Destroy(gameObject);
    }
}
