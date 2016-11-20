using UnityEngine;
using System.Collections;
using System;

public class Destructable : MonoBehaviour {
    
    public float Health;
    public bool DestroyMe;

    private Rigidbody _rigidBody;
    private RealBlockBehavior _blockBehavior;
    private Vector3 _previousVelocity;

    private Rigidbody Rigidbody
    {
        get
        {
            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody>();
            }
            return _rigidBody;
        }
    }

    public virtual void Start ()
    {
        _blockBehavior = GetComponent<RealBlockBehavior>();
    }

    void FixedUpdate ()
    {
        _previousVelocity = Rigidbody.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        var changeInVelocity = Vector3.Magnitude((Rigidbody.velocity - _previousVelocity));
        if (changeInVelocity > 1)
        {
            var force = changeInVelocity / (Time.fixedDeltaTime * 10) * Rigidbody.mass;
            InflictDamage(force, collision);

            var destructable = collision.gameObject.GetComponent<Destructable>();
            if (destructable != null)
            {
                destructable.InflictDamage(force, collision);
            }

            _previousVelocity = Rigidbody.velocity;
        }
    }

    public virtual void InflictDamage(float damage, Collision collision)
    {
        if (!Destroyed)
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
        if (_blockBehavior != null)
        {
            var initialHealth = _blockBehavior.InitialHealth;
            if (Health / initialHealth < 0.33f)
            {
                _blockBehavior.SetModel("VeryDamaged");
            }
            else if (Health / initialHealth < 0.66f)
            {
                _blockBehavior.SetModel("Damaged");
            }
        }
    }

    public virtual void OnDestroyed(Collision collision)
    {
        if (DestroyMe)
        {
            Destroy(gameObject);
        }
        if (_blockBehavior != null)
        {
            _blockBehavior.OnDestroyed();
        }
    }

    public bool Destroyed
    {
        get
        {
            return Health < 0;
        }
    }
}
