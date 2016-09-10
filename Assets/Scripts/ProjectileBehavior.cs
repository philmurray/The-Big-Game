﻿using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Scripts.DataStructures;

public class ProjectileBehavior : MonoBehaviour {

    public GameController.Player Player;
    public Weapon Weapon;
    public float Damage;

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
        float force = 0;
        var destructable = collision.gameObject.GetComponent<Destructable>();

        if (destructable != null && _rigidBody)
        {
            var changeInVelocity = Vector3.Magnitude((_rigidBody.velocity - _previousVelocity));
            if (changeInVelocity > 1)
            {
                force = changeInVelocity / (Time.fixedDeltaTime * 10) * _rigidBody.mass;
            }
        }

        var damage = force * Damage;
        destructable.InflictDamage(force, collision);
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
