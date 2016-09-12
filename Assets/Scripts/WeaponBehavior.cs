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


    protected float PowerModifier = 1;
    protected ProjectileBehavior Projectile;

    public virtual ProjectileBehavior Fire() {
        var p = Projectile;
        Projectile.transform.SetParent(null, true);
        Projectile = null;
        return p;
    }
    public virtual void GetReady() {
        if (Projectile == null)
        {
            var go = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity) as GameObject;
            Projectile = go.GetComponent<ProjectileBehavior>();
            Projectile.transform.SetParent(transform, true);

            Projectile.Weapon = WeaponState;
            Projectile.Player = Player;
        }
    }
    public virtual void SetState(Weapon weaponState) {
        WeaponState = weaponState;
    }

    public void ApplyPowerUpgrades(List<AffectsWeaponPower> affectsWeaponPower)
    {
        PowerModifier = 1;
        foreach (var a in affectsWeaponPower)
        {
            PowerModifier *= a.PowerAffect;
        }
    }
}
