using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class WeaponContainer : MonoBehaviour {

    public GameController.Player Player;

    public WeaponBehavior Weapon;
    public List<WeaponTypeItem> WeaponBehaviors;

    private Dictionary<Weapon.WeaponType, WeaponBehavior> _weapons;
    public Dictionary<Weapon.WeaponType, WeaponBehavior> WeaponsDictionary
    {
        get
        {
            if (_weapons == null)
            {
                _weapons = new Dictionary<Weapon.WeaponType, WeaponBehavior>();
                WeaponBehaviors.ForEach(w => _weapons.Add(w.WeaponType, w.Weapon));
            }
            return _weapons;
        }
    }

    [Serializable]
    public class WeaponTypeItem
    {
        public Weapon.WeaponType WeaponType;
        public WeaponBehavior Weapon;
    }

    public void SelectWeapon(Weapon weapon)
    {
        RemoveWeapon();
        var weaponObject = Instantiate(WeaponsDictionary[weapon.Type].gameObject) as GameObject;
        weaponObject.transform.SetParent(transform, false);
        Weapon = weaponObject.GetComponent<WeaponBehavior>();

        Weapon.Player = Player;
        Weapon.WeaponState = weapon;
        Weapon.StateUpdated();
        Weapon.GetReady();
    }

    public void RemoveWeapon()
    {
        if (Weapon != null) {
            Destroy(Weapon.gameObject);
            Weapon = null;
        }
    }
}
