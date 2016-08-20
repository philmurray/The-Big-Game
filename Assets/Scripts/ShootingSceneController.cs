﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Assets.Scripts.DataStructures;

public class ShootingSceneController : MonoBehaviour {

    public static ShootingSceneController instance;
    void Awake()
    {
        instance = this;
    }
    public enum States { Intro, Aiming, Firing, Summary };

    public States State = States.Intro;
    
    public List<ShootingPlayerController> PlayersList;

    private Dictionary<GameController.Player, ShootingPlayerController> _playersDictionary;
    public Dictionary<GameController.Player, ShootingPlayerController> PlayersDictionary {
        get {
            if (_playersDictionary == null) {
                _playersDictionary = new Dictionary<GameController.Player, ShootingPlayerController>();
                PlayersList.ForEach(p => _playersDictionary.Add(p.Player, p));
            }
            return _playersDictionary;
        }
    }
    public ShootingPlayerController ActivePlayer
    {
        get
        {
            return PlayersDictionary[GameController.instance.ActivePlayer];
        }
    }
    public ShootingPlayerController OtherPlayer
    {
        get
        {
            return PlayersDictionary[GameController.instance.OtherPlayer];
        }
    }

    [Serializable]
    public class WeaponObjects
    {
        public Weapon.WeaponType Weapon;
        public GameObject WeaponMenu;
        public SelectButton SelectionButton;
    }
    public List<WeaponObjects> WeaponsList;

    private Dictionary<Weapon.WeaponType, WeaponObjects> _weaponsDictionary;
    public Dictionary<Weapon.WeaponType, WeaponObjects> WeaponsDictionary
    {
        get
        {
            if (_weaponsDictionary == null)
            {
                _weaponsDictionary = new Dictionary<Weapon.WeaponType, WeaponObjects>();
                WeaponsList.ForEach(p => _weaponsDictionary.Add(p.Weapon, p));
            }
            return _weaponsDictionary;
        }
    }

    public WeaponObjects ActiveWeapon {
        get {
            return WeaponsDictionary[GameController.instance.ActivePlayerWeapon.Type];
        }
    }

    public Camera IntroCamera;
    public Camera ProjectileFollowCamera;

    void Start()
    {
        HUDController.instance.gameObject.SetActive(false);

        PlayersList.ForEach(p => p.StartTargeted());

        if (State == States.Intro)
        {
            IntroStart();
        }
        else if (State == States.Aiming)
        {
            State = States.Intro;
            IntroDone();
        }
    }

    public void IntroStart()
    {
        IntroCamera.enabled = true;
    }
    public void IntroDone() {
        if (State == States.Intro)
        {
            IntroCamera.enabled = false;
            StartAim();
        }
    }

    public void StartAim() {
        State = States.Aiming;
        StartPlayerAim(GameController.Player.One);
        HUDController.instance.gameObject.SetActive(true);
    }

    private void StartPlayerAim(GameController.Player player) {
        GameController.instance.ActivePlayer = player;
        ActivePlayer.StartAiming();
        SelectWeaponMenu(GameController.instance.ActivePlayerWeapon.Type);
    }

    public void SelectCatapult() {
        SelectWeapon(Weapon.WeaponType.Catapult);
    }

    public void SelectCrossbow() {
        SelectWeapon(Weapon.WeaponType.Crossbow);
    }
    public void SelectWeapon(Weapon.WeaponType weapon) {
        ActivePlayer.SelectWeapon(weapon);
        SelectWeaponMenu(weapon);
    }

    public void SelectWeaponMenu(Weapon.WeaponType weapon) {
        WeaponsList.ForEach(w => w.SelectionButton.Select(false));
        ActiveWeapon.SelectionButton.Select(true);
    }

    public void PlayerReady()
    {
        ActivePlayer.EndAiming();
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            if (GameController.instance.PlayerTwo.IsHuman)
            {
                StartPlayerAim(GameController.Player.Two);
            }
            else
            {
                //TODO: AI?
            }
        }
        else {
            StartFiring();
        }
    }

    public void StartFiring()
    {
        State = States.Firing;
        HUDController.instance.gameObject.SetActive(false);
        StartPlayerFiring(GameController.Player.One);
    }

    private void StartPlayerFiring(GameController.Player player)
    {
        GameController.instance.ActivePlayer = player;
        ActivePlayer.StartShooting();
    }

    public void StartProjectileFollow(GameObject projectile) {
        ProjectileFollowCamera.enabled = true;
        ProjectileFollowCamera.gameObject.GetComponent<FollowProjectileBehavior>().Target = projectile;
        OtherPlayer.WaitForProjectile(projectile);
    }
    public void StopProjectileFollow() {
        ProjectileFollowCamera.enabled = false;
        ProjectileFollowCamera.gameObject.GetComponent<FollowProjectileBehavior>().Target = null;
    }

    public void StopPlayerFiring()
    {
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            StartPlayerFiring(GameController.Player.Two);
        }
        else
        {
            StartAim();
        }
    }
}
