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
    public enum States { Intro, PlayerShoot, ResolveShots, Summary };
    public States State = States.Intro;

    [Serializable]
    public class PlayerObjects {
        public GameController.Player Player;
        public BlockContainer BlockContainer;
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
            GameController.instance.ActivePlayer = p.Player;
            p.BlockContainer.SetBlocks(GameController.instance.GetPlayer(p.Player).Blocks);
        }
    }

    public void IntroDone() {
        if (State == States.Intro) {
            State = States.PlayerShoot;
            IntroCamera.enabled = false;
            StartPlayerTurn(GameController.Player.One);
            SelectCatapult();
            HUDController.instance.gameObject.SetActive(true);
        }
    }

    private void StartPlayerTurn(GameController.Player player) {
        GameController.instance.ActivePlayer = player;
        PlayersDictionary[player].ShootCamera.enabled = true;
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
    }
}
