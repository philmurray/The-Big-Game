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
        var destructable = collision.gameObject.GetComponent<Destructable>();
        if (destructable != null)
        {
            var force = Vector3.Magnitude((_rigidBody.velocity - _previousVelocity) / Time.fixedDeltaTime * _rigidBody.mass) * Damage;
            destructable.InflictDamage(force, collision);
        }
    }

}
