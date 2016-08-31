using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawnPoint;
    public Weapon WeaponState;
    public GameController.Player Player;

    private GameObject Projectile;

    public virtual GameObject Fire() {
        var p = Projectile;
        Projectile.transform.SetParent(null, true);
        Projectile = null;
        return p;
    }
    public virtual void GetReady() {
        if (Projectile == null)
        {
            Projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity) as GameObject;
            Projectile.transform.SetParent(transform, true);
            Projectile.GetComponent<Rigidbody>().mass = WeaponState.ProjectileMass(Player);
            Projectile.GetComponent<Damager>().Damage = WeaponState.ProjectileDamage(Player);
        }
    }
    public virtual void SetState(Weapon weaponState) {
        WeaponState = weaponState;
        StateUpdated();
    }

    public virtual void StateUpdated()
    {
    }
}
