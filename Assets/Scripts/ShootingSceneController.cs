using UnityEngine;
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
    public ShootingPlayerController OtherPlayer(GameController.Player player)
    {
        return player == GameController.Player.One ? PlayersDictionary[GameController.Player.Two] : PlayersDictionary[GameController.Player.One];
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
    public Camera ExtroCamera;

    public float MaxHorizontalAngleMinor;
    public float HorizontalAngleSpeedMinor;

    public float MaxHorizontalAngleMajor;
    public float HorizontalAngleSpeedMajor;

    public float MaxVerticalAngle;
    public float VerticalAngleSpeed;

    public float PowerSpeed;
    public Text PowerText;

    public RoundOverModalBehavior RoundOverModal;

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
        HUDController.instance.Refresh();
        SelectWeaponMenu(GameController.instance.ActivePlayerWeapon.Type);
    }

    public void SelectCatapult() {
        SelectWeapon(Weapon.WeaponType.Catapult);
    }

    public void SelectCrossbow() {
        SelectWeapon(Weapon.WeaponType.Crossbow);
    }
    public void SelectWeapon(Weapon.WeaponType weapon) {
        if (GameController.instance.ActivePlayerWeapon.Type != weapon)
        {
            ActivePlayer.SelectWeapon(weapon);
            SelectWeaponMenu(weapon);
        }
    }

    public void SelectWeaponMenu(Weapon.WeaponType weapon) {
        foreach (var weaponItem in WeaponsList) {
            weaponItem.SelectionButton.Select(false);
            weaponItem.WeaponMenu.SetActive(false);
        }
        ActiveWeapon.SelectionButton.Select(true);
        ActiveWeapon.WeaponMenu.SetActive(true);

        UpdatePowerText();
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

    public void StopPlayerFiring()
    {
        if (GameController.instance.ActivePlayer == GameController.Player.One)
        {
            StartPlayerFiring(GameController.Player.Two);
        }
        else
        {
            bool p1 = GameController.instance.PlayerOne.Blocks.Exists(b => b.Type == Block.BlockType.Crystal);
            bool p2 = GameController.instance.PlayerTwo.Blocks.Exists(b => b.Type == Block.BlockType.Crystal);

            if (!p1 || !p2)
            {
                EndRound(p1, p2);
            }
            else
            {
                StartAim();
            }
        }
    }

    public void EndRound(bool playerOneWins, bool playerTwoWins)
    {
        State = States.Summary;
        ExtroCamera.enabled = true;

        if (playerOneWins)
        {
            GameController.instance.AddScore(GameController.Player.One);
            if (playerTwoWins)
            {
                GameController.instance.AddScore(GameController.Player.Two);
                RoundOverModal.ShowTie();
            }
            else
            {
                RoundOverModal.ShowWinner(GameController.Player.One);
            }
        }
        else
        {
            GameController.instance.AddScore(GameController.Player.Two);
            RoundOverModal.ShowWinner(GameController.Player.Two);
        }
    }

    public void AimLeftMinor()
    {
        GameController.instance.ActivePlayerWeapon.MinorHorizontalAngle += HorizontalAngleSpeedMinor;
        if (GameController.instance.ActivePlayerWeapon.MinorHorizontalAngle > MaxHorizontalAngleMinor) {
            GameController.instance.ActivePlayerWeapon.MinorHorizontalAngle = MaxHorizontalAngleMinor;
        }
        ActivePlayer.UpdateWeapon();
    }

    public void AimRightMinor()
    {
        GameController.instance.ActivePlayerWeapon.MinorHorizontalAngle -= HorizontalAngleSpeedMinor;
        if (GameController.instance.ActivePlayerWeapon.MinorHorizontalAngle < -MaxHorizontalAngleMinor)
        {
            GameController.instance.ActivePlayerWeapon.MinorHorizontalAngle = -MaxHorizontalAngleMinor;
        }
        ActivePlayer.UpdateWeapon();
    }

    public void AimRightMajor()
    {
        GameController.instance.ActivePlayerWeapon.MajorHorizontalAngle += HorizontalAngleSpeedMajor;
        if (GameController.instance.ActivePlayerWeapon.MajorHorizontalAngle > MaxHorizontalAngleMajor)
        {
            GameController.instance.ActivePlayerWeapon.MajorHorizontalAngle = MaxHorizontalAngleMajor;
        }
        ActivePlayer.UpdateWeapon();
    }

    public void AimLeftMajor()
    {
        GameController.instance.ActivePlayerWeapon.MajorHorizontalAngle -= HorizontalAngleSpeedMajor;
        if (GameController.instance.ActivePlayerWeapon.MajorHorizontalAngle < -MaxHorizontalAngleMajor)
        {
            GameController.instance.ActivePlayerWeapon.MajorHorizontalAngle = -MaxHorizontalAngleMajor;
        }
        ActivePlayer.UpdateWeapon();
    }

    public void MorePower()
    {
        GameController.instance.ActivePlayerWeapon.Power += PowerSpeed;
        if (GameController.instance.ActivePlayerWeapon.Power > 100)
        {
            GameController.instance.ActivePlayerWeapon.Power = 100;
        }
        UpdatePowerText();
        ActivePlayer.UpdateWeapon();
    }

    private void UpdatePowerText()
    {
        PowerText.text = Mathf.RoundToInt(GameController.instance.ActivePlayerWeapon.Power).ToString();
    }

    public void LessPower()
    {
        GameController.instance.ActivePlayerWeapon.Power -= PowerSpeed;
        if (GameController.instance.ActivePlayerWeapon.Power < 0)
        {
            GameController.instance.ActivePlayerWeapon.Power = 0;
        }
        UpdatePowerText();
        ActivePlayer.UpdateWeapon();
    }

    public void AimUp()
    {
        GameController.instance.ActivePlayerWeapon.VerticalAngle += VerticalAngleSpeed;
        if (GameController.instance.ActivePlayerWeapon.VerticalAngle > MaxVerticalAngle)
        {
            GameController.instance.ActivePlayerWeapon.VerticalAngle = MaxVerticalAngle;
        }
        ActivePlayer.UpdateWeapon();
    }

    public void AimDown()
    {
        GameController.instance.ActivePlayerWeapon.VerticalAngle -= VerticalAngleSpeed;
        if (GameController.instance.ActivePlayerWeapon.VerticalAngle < 0)
        {
            GameController.instance.ActivePlayerWeapon.VerticalAngle = 0;
        }
        ActivePlayer.UpdateWeapon();
    }
}
