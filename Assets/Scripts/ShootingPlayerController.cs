using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class ShootingPlayerController : MonoBehaviour {

    public enum Phase { Waiting, Aiming, Shooting, WaitingForProjectile, Resolving };
    public Phase CurrentPhase;

    public GameController.Player Player;
    public BlockContainer BlockContainer;
    public WeaponContainer WeaponContainer;

    public Camera AimCamera;
    public Rotatable WeaponRotaterMinor;
    public Rotatable WeaponRotaterMajor;

    public Camera FireCamera;
    public float FireCameraWait;
    public float FireWait;

    public HitCameraBehavior HitCamera;
    public Transform HitTarget;
    public float HitTargetDistance;
    public float CrystalZoomSpeed;
    public float MaxResolutionTime;
    public float MinResolutionTime;
    private float _resolutionStartTime;

    public Camera ProjectileFollowCamera;
    
    private ProjectileBehavior _incomingProjectile;

    void Start() {
        BlockContainer.SetBlocks();
        CurrentPhase = Phase.Waiting;
    }

    void FixedUpdate() {
        if (CurrentPhase == Phase.WaitingForProjectile)
        {
            var distance = Vector3.Distance(HitTarget.position, _incomingProjectile.transform.position);
            if (distance < HitTargetDistance)
            {
                StartHitting();
            }
        }
        else if (CurrentPhase == Phase.Resolving)
        {
            WaitForResolution();
        }
    }
    public void StartTargeted()
    {
        ToggleBlocks(true);
        ToggleWeapon(false);
    }

    public void StartAiming()
    {
        CurrentPhase = Phase.Aiming;
        AimCamera.enabled = true;
        ToggleBlocks(false);
        ToggleWeapon(true);
        WeaponContainer.SelectWeapon(GameController.instance.GetPlayer(Player).Weapon);
        UpdateWeapon();
    }
    public void EndAiming()
    {
        CurrentPhase = Phase.Waiting;
        ApplyModifiersToWeapon();
        AimCamera.enabled = false;
        ToggleBlocks(true);
        ToggleWeapon(false);
    }

    public void StartShooting()
    {
        CurrentPhase = Phase.Shooting;
        ToggleBlocks(false);
        ToggleWeapon(true);
        FireCamera.enabled = true;
        StartCoroutine(WaitToFire());
    }

    private IEnumerator WaitToFire()
    {
        yield return new WaitForSeconds(FireWait);
        var projectile = WeaponContainer.Weapon.Fire();
        yield return new WaitForSeconds(FireCameraWait);

        StartProjectileFollow(projectile);
        if (WeaponContainer.Weapon.WeaponState.Type == Weapon.WeaponType.Crossbow)
        {
            ShootingSceneController.instance.OtherPlayer(Player).StartHitting();
        }

        EndShooting();
    }

    public void EndShooting()
    {
        CurrentPhase = Phase.Waiting;
        FireCamera.enabled = false;
        ToggleBlocks(true);
        WeaponContainer.RemoveWeapon();
    }


    public void StartProjectileFollow(ProjectileBehavior projectile)
    {
        ProjectileFollowCamera.enabled = true;
        ProjectileFollowCamera.gameObject.GetComponent<FollowProjectileBehavior>().Target = projectile;
        ShootingSceneController.instance.OtherPlayer(Player).WaitForProjectile(projectile);
    }
    public void StopProjectileFollow()
    {
        ProjectileFollowCamera.enabled = false;
        ProjectileFollowCamera.gameObject.GetComponent<FollowProjectileBehavior>().Target = null;
    }

    public void SelectWeapon(Weapon.WeaponType weapon)
    {
        GameController.instance.GetPlayer(Player).Weapon.Type = weapon;
        WeaponContainer.SelectWeapon(GameController.instance.GetPlayer(Player).Weapon);
    }

    private void ToggleWeapon(bool visible)
    {
        ToggleActive(WeaponContainer.gameObject, visible);
    }

    private void ToggleBlocks(bool visible)
    {
        ToggleActive(BlockContainer.gameObject, visible);
    }

    private void ToggleActive(GameObject gameobject, bool active) {
        gameobject.SetActive(active);
        foreach (var child in gameobject.transform)
        {
            var go = child as GameObject;
            if (go != null)
            {
                go.SetActive(active);
            }
        }
    }

    public void WaitForProjectile(ProjectileBehavior projectile)
    {
        CurrentPhase = Phase.WaitingForProjectile;
        _incomingProjectile = projectile;
        _incomingProjectile.transform.SetParent(BlockContainer.transform);
    }

    private void StartHitting()
    {
        CurrentPhase = Phase.Resolving;
        ShootingSceneController.instance.OtherPlayer(Player).StopProjectileFollow();
        HitCamera.CameraEnabled = true;
        _resolutionStartTime = Time.fixedTime;
    }

    private void WaitForResolution()
    {
        var timePassed = Time.fixedTime - _resolutionStartTime;
        if (timePassed < MinResolutionTime)
        {
            return;
        }

        if (timePassed > MaxResolutionTime)
        {
            StopHitting();
            return;
        }

        var hasChanged = false;
        foreach (var pt in BlockContainer.GetComponentsInChildren<PositionTracker>())
        {
            if (pt.HasChanged)
            {
                hasChanged = true;
                break;
            }
        }
        var destroyedCrystal = BlockContainer.GetComponentInChildren<DestroyedCrystal>();
        if (destroyedCrystal != null)
        {
            HitCamera.FocusOn(destroyedCrystal.transform);
            hasChanged = true;
        }

        if (!hasChanged)
        {
            StopHitting();
        }
    }

    private void StopHitting()
    {
        CurrentPhase = Phase.Waiting;
        ShootingSceneController.instance.StopPlayerFiring();
        _incomingProjectile = null;
        HitCamera.Reset();
        HitCamera.CameraEnabled = false;
    }

    public void UpdateWeapon()
    {
        WeaponRotaterMinor.SetAngle(GameController.instance.GetPlayer(Player).Weapon.MinorHorizontalAngle);
        WeaponRotaterMajor.SetAngle(GameController.instance.GetPlayer(Player).Weapon.MajorHorizontalAngle);
        WeaponContainer.Weapon.StateUpdated();
    }

    private void ApplyModifiersToWeapon()
    {
        List<FlagBehavior> mods = new List<FlagBehavior>();
        foreach (var mod in BlockContainer.GetComponentsInChildren<FlagBehavior>())
        {
            if (!mod.Destroyed)
            {
                mods.Add(mod);
            }
        }
        WeaponContainer.Weapon.ApplyPowerUpgrades(mods);
    }

}
