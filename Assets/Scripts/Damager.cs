using UnityEngine;
using System.Collections;

public class Damager : MonoBehaviour {

    public float Damage;

    private Rigidbody _rigidBody;
    private Vector3 _previousVelocity;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        _previousVelocity = _rigidBody.velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        var changeInVelocity = Vector3.Magnitude((_rigidBody.velocity - _previousVelocity));
        if (changeInVelocity > 1)
        {
            var destructable = collision.gameObject.GetComponent<Destructable>();
            if (destructable != null)
            {
                var force = changeInVelocity / (Time.fixedDeltaTime * 10) * _rigidBody.mass * Damage;
                destructable.InflictDamage(force, collision);
            }
        }
    }

}
