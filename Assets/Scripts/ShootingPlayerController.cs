using UnityEngine;
using System.Collections;
using System;

public class ShootingPlayerController : MonoBehaviour {

    public GameController.Player Player;
    public BlockContainer BlockContainer;
    public Transform WeaponContainer;

    public Camera AimCamera;

    public Camera FireCamera;
    public float FireCameraWait;
    public float FireWait;

    public Camera HitCamera;
    public Transform HitTarget;
    public float HitTargetDistance;

    private WeaponBehavior _weapon;
    private GameObject _incomingProjectile;

    void Start() {
        BlockContainer.SetBlocks(GameController.instance.GetPlayer(Player).Blocks);
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

    public void StartAiming()
    {
        AimCamera.enabled = true;
        ToggleBlocks(false);
        ToggleWeapon(true);
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
        _weapon.Fire();
        yield return new WaitForSeconds(FireCameraWait);
        ShootingSceneController.instance.StartProjectileFollow(_weapon.Projectile);
        EndShooting();
    }

    public void EndShooting()
    {
        FireCamera.enabled = false;
        ToggleBlocks(true);
        ToggleWeapon(false, false);
    }

    public void SelectWeapon(WeaponBehavior weapon)
    {
        var weaponObject = Instantiate(weapon.gameObject) as GameObject;
        weaponObject.transform.SetParent(WeaponContainer, false);
        _weapon = weaponObject.GetComponent<WeaponBehavior>();
        _weapon.GetReady();
    }
    private void ToggleWeapon(bool visible)
    {
        ToggleWeapon(visible, true);
    }

    private void ToggleWeapon(bool visible, bool withProjectile)
    {
        if (_weapon != null)
        {
            ToggleActive(_weapon.gameObject, visible);
            if (withProjectile && _weapon.Projectile != null)
            {
                ToggleActive(_weapon.Projectile, visible);
            }
        }
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
        HitCamera.enabled = false;
    }
}
