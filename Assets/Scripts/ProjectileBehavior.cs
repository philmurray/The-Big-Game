using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.DataStructures;

public class ProjectileBehavior : MonoBehaviour {

    public GameController.Player Player;
    public Weapon Weapon;
    public float Damage;

    public bool ExplosionActive;
    public float ExplosionDelay;
    public float ExplosionForce;
    public float ExplosionRadius;
    public float ExplosionUpwardForce;

    private Rigidbody _rigidBody;
    private Vector3 _previousVelocity;

    public float Mass
    {
        get
        {
            return _rigidBody == null ? 0.0f : _rigidBody.mass;
        }
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();

        InitializeMass();
        InitializeDamage();
    }

    void FixedUpdate()
    {
        if (_rigidBody)
        {
            _previousVelocity = _rigidBody.velocity;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float changeInVelocity = Vector3.Magnitude((_rigidBody.velocity - _previousVelocity));
        float force = 0;
        bool impact = changeInVelocity > 1;

        var destructable = collision.gameObject.GetComponent<Destructable>();

        if (destructable != null)
        {
            if (_rigidBody != null)
            {
                if (impact)
                {
                    force = changeInVelocity / (Time.fixedDeltaTime * 10) * _rigidBody.mass;
                }
            }

            var damage = force * Damage;
            destructable.InflictDamage(force, collision);

        }

        if (ExplosionForce > 0 && ExplosionActive && impact)
        {
            ExplosionActive = false;
            StartCoroutine(Explosion(collision.contacts[0].point));
        }
    }

    public IEnumerator _delayedActivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        ExplosionActive = true;
    }
    public void Activate()
    {
        ExplosionActive = true;
    }
    public void Activate(float delay)
    {
        StartCoroutine(_delayedActivate(delay));
    }

    private IEnumerator Explosion(Vector3 explosionPos)
    {
        yield return new WaitForSeconds(ExplosionDelay);

        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = hit.GetComponentInParent<Rigidbody>();
            }

            if (rb != null && rb != _rigidBody)
            {
                rb.AddExplosionForce(ExplosionForce, explosionPos, ExplosionRadius, ExplosionUpwardForce);
            }
        }
    }

    private void InitializeMass()
    {
        if (_rigidBody != null)
        {
            float mass = _rigidBody.mass;
            foreach (var upgradeOptions in GameController.instance.GetPlayer(Player).State.FindUpgradesWithOption("AffectsProjectileMass"))
            {
                if (upgradeOptions["AffectsProjectileMass"].Split(' ').Contains(Weapon.Type.ToString()))
                {
                    mass *= float.Parse(upgradeOptions["MassModifier"]);
                }
            }
            _rigidBody.mass = mass;
        }
    }

    private void InitializeDamage()
    {
        float damage = Damage;
        foreach (var upgradeOptions in GameController.instance.GetPlayer(Player).State.FindUpgradesWithOption("AffectsProjectileDamage"))
        {
            if (upgradeOptions["AffectsProjectileDamage"].Split(' ').Contains(Weapon.Type.ToString()))
            {
                damage *= float.Parse(upgradeOptions["DamageModifier"]);
            }
        }
        Damage = damage;
    }

}
