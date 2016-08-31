using UnityEngine;
using System.Collections;
using System;

public class Destructable : MonoBehaviour {

    public float Health {
        get {
            return _currentHealth;
        }
        set {
            _initialHealth = value;
            if (_currentHealth == 0.0f)
            {
                _currentHealth = _initialHealth;
            }
        }
    }
    private float _currentHealth;
    private float _initialHealth;

    void OnCollisionEnter(Collision collision)
    {
        var rbb = GetComponent<RealBlockBehavior>();
        var rb = collision.rigidbody;
        if (rbb != null && rb != null)
        {
            float damageFactor = 1;
            var damager = collision.gameObject.GetComponent<Damager>();

            if (damager != null) {
                damageFactor = damager.Damage;
            }
            var magnitude = Vector3.Magnitude(collision.relativeVelocity) * rb.mass;
            if (magnitude > 10)
            {
                Debug.Log(rbb.Block.Type + " " + magnitude);
                var damage = magnitude * damageFactor;
                Debug.Log(rbb.Block.Type + " " + _currentHealth + " - " + damage);

                _currentHealth -= magnitude * damageFactor;
                if (_currentHealth < 0)
                {
                    rb.velocity = collision.relativeVelocity;
                    Destroy(gameObject);
                }
                else if (_currentHealth / _initialHealth < 0.33f)
                {
                    rbb.SetModel("VeryDamaged");
                }
                else if (_currentHealth / _initialHealth < 0.66f)
                {
                    rbb.SetModel("Damaged");
                }
            }
        }
    }
}
