using UnityEngine;
using System.Collections;
using System;

public class Destructable : MonoBehaviour {
    
    public float Health;

    private Rigidbody _rigidBody;
    private Vector3 _previousVelocity;

    public virtual void Start ()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate ()
    {
        _previousVelocity = _rigidBody.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        var changeInVelocity = Vector3.Magnitude((_rigidBody.velocity - _previousVelocity));
        if (changeInVelocity > 1)
        {
            var force = changeInVelocity / (Time.fixedDeltaTime * 10) * _rigidBody.mass;
            InflictDamage(force, collision);

            var destructable = collision.gameObject.GetComponent<Destructable>();
            if (destructable != null)
            {
                destructable.InflictDamage(force, collision);
            }

            _previousVelocity = _rigidBody.velocity;
        }
    }

    public virtual void InflictDamage(float damage, Collision collision)
    {
        if (Health > 0)
        {
            Health -= damage;
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

    public virtual void OnDamaged(Collision collision)
    {

    }

    public virtual void OnDestroyed(Collision collision)
    {
        Destroy(gameObject);
    }
}
