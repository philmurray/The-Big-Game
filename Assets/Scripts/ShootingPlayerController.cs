using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.DataStructures;

public class ShootingPlayerController : MonoBehaviour {

    public GameController.Player Player;
    public BlockContainer BlockContainer;
    public WeaponContainer WeaponContainer;

    public Camera AimCamera;
    public Rotatable WeaponRotater;

    public Camera FireCamera;
    public float FireCameraWait;
    public float FireWait;

    public Camera HitCamera;
    public Transform HitTarget;
    public float HitTargetDistance;
    
    private GameObject _incomingProjectile;

    void Start() {
        BlockContainer.SetBlocks();
    }

    void FixedUpdate() {
        if (_incomingProjectile != null) {
            if (!HitCamera.enabled)
            {
                var distance = Vector3.Distance(HitTarget.position, _incomingProjectile.transform.position);
                if (distance < HitTargetDistance)
                {
                    StartHitting();
                }
            }
            var hasChanged = _incomingProjectile.GetComponent<PositionTracker>().HasChanged;
            if (!hasChanged) {

                foreach (var pt in BlockContainer.GetComponentsInChildren<PositionTracker>()) {
                    if (pt.HasChanged) {
                        hasChanged = true;
                        break;
                    }
                }
            }
            if (!hasChanged)
            {
                StopHitting();
            }
        }
    }
    public void StartTargeted()
    {
        ToggleBlocks(true);
        ToggleWeapon(false);
    }

    public void StartAiming()
    {
        AimCamera.enabled = true;
        ToggleBlocks(false);
        ToggleWeapon(true);
        WeaponContainer.SelectWeapon(GameController.instance.GetPlayer(Player).Weapon);
        UpdateWeapon();
    }
    public void EndAiming() {
        AimCamera.enabled = false;
        ToggleBlocks(true);
        ToggleWeapon(false);
    }

    public void StartShooting()
    {
        ToggleBlocks(false);
        ToggleWeapon(true);
        FireCamera.enabled = true;
        StartCoroutine(WaitToFire());
    }

    private IEnumerator WaitToFire()
    {
        yield return new WaitForSeconds(FireWait);
        GameObject projectile = WeaponContainer.Weapon.Fire();
        yield return new WaitForSeconds(FireCameraWait);
        ShootingSceneController.instance.StartProjectileFollow(projectile);
        EndShooting();
    }

    public void EndShooting()
    {
        FireCamera.enabled = false;
        ToggleBlocks(true);
        WeaponContainer.RemoveWeapon();
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

    public void WaitForProjectile(GameObject projectile)
    {
        _incomingProjectile = projectile;
    }

    private void StartHitting()
    {
        ShootingSceneController.instance.StopProjectileFollow();
        HitCamera.enabled = true;
    }

    private void StopHitting()
    {
        ShootingSceneController.instance.StopPlayerFiring();
        _incomingProjectile.transform.SetParent(BlockContainer.transform);
        _incomingProjectile = null;
        HitCamera.GetComponent<HitCameraBehavior>().Reset();
        HitCamera.enabled = false;
    }

    public void UpdateWeapon()
    {
        WeaponRotater.SetAngle(GameController.instance.GetPlayer(Player).Weapon.HorizontalAngle);
        WeaponContainer.Weapon.StateUpdated();
    }
}
