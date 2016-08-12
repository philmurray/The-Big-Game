using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class ShootingSceneController : MonoBehaviour {

    public static ShootingSceneController instance;

    void Awake()
    {
        instance = this;
    }
    public enum States { Intro, PlayerShoot, Firing, Summary };
    public States State = States.Intro;

    [Serializable]
    public class PlayerObjects {
        public GameController.Player Player;
        public BlockContainer BlockContainer;
        public Transform WeaponContainer;
        public Camera ShootCamera;
        public Camera FireCamera;
        public Camera HitCamera;
    }
    public List<PlayerObjects> PlayersList;

    private Dictionary<GameController.Player, PlayerObjects> _playersDictionary;
    public Dictionary<GameController.Player, PlayerObjects> PlayersDictionary {
        get {
            if (_playersDictionary == null) {
                _playersDictionary = new Dictionary<GameController.Player, PlayerObjects>();
                PlayersList.ForEach(p => _playersDictionary.Add(p.Player, p));
            }
            return _playersDictionary;
        }
    }
    public PlayerObjects ActivePlayer
    {
        get
        {
            return PlayersDictionary[GameController.instance.ActivePlayer];
        }
    }

    public enum Weapons {
        Crossbow,
        Catapult
    };

    [Serializable]
    public class WeaponObjects
    {
        public Weapons Weapon;
        public GameObject WeaponPrefab;
        public GameObject WeaponMenu;
        public SelectButton SelectionButton;
    }
    public List<WeaponObjects> WeaponsList;

    private Dictionary<Weapons, WeaponObjects> _weaponsDictionary;
    public Dictionary<Weapons, WeaponObjects> WeaponsDictionary
    {
        get
        {
            if (_weaponsDictionary == null)
            {
                _weaponsDictionary = new Dictionary<Weapons, WeaponObjects>();
                WeaponsList.ForEach(p => _weaponsDictionary.Add(p.Weapon, p));
            }
            return _weaponsDictionary;
        }
    }

    public Weapons ActiveWeaponType;
    public WeaponObjects ActiveWeapon {
        get {
            return WeaponsDictionary[ActiveWeaponType];
        }
    }

    public Camera IntroCamera;
    public Camera ProjectileFollowCamera;

    void Start()
    {
        HUDController.instance.gameObject.SetActive(false);

        foreach (var p in PlayersList)
        {
            p.BlockContainer.SetBlocks(GameController.instance.GetPlayer(p.Player).Blocks);
        }

        if (State == States.PlayerShoot) {
            State = States.Intro;
            IntroDone();
        }
    }

    public void IntroDone() {
        if (State == States.Intro)
        {
            IntroCamera.enabled = false;
            StartPlayerShoot();
        }
    }

    public void StartPlayerShoot() {
        State = States.PlayerShoot;
        StartPlayerTurn(GameController.Player.One);
        HUDController.instance.gameObject.SetActive(true);
    }

    private void StartPlayerTurn(GameController.Player player) {
        GameController.instance.ActivePlayer = player;
        ActivePlayer.ShootCamera.enabled = true;
        SelectCatapult();
    }

    public void SelectCatapult() {
        SelectWeapon(Weapons.Catapult);
    }

    public void SelectCrossbow() {
        SelectWeapon(Weapons.Crossbow);
    }

    public void SelectWeapon(Weapons weapon) {
        ActiveWeaponType = weapon;
        WeaponsList.ForEach(w => w.SelectionButton.Select(false));
        ActiveWeapon.SelectionButton.Select(true);
        var weaponObject = Instantiate(ActiveWeapon.WeaponPrefab) as GameObject;
        weaponObject.transform.SetParent(ActivePlayer.WeaponContainer,false);
    }

    public void PlayerReady()
    {
        ActivePlayer.ShootCamera.enabled = false;
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            if (GameController.instance.PlayerTwo.IsHuman)
            {
                StartPlayerTurn(GameController.Player.Two);
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

    public void StartFiring() {

    }
}
