using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class WeaponBehavior : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform ProjectileSpawnPoint;
    public GameObject Projectile;
    public abstract void Fire();
    public void GetReady() {
        Projectile = Instantiate(ProjectilePrefab, ProjectileSpawnPoint.position, Quaternion.identity) as GameObject;
    }
}
