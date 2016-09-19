using UnityEngine;
using System.Collections;

public class TestWeapon : MonoBehaviour {

    private WeaponBehavior Weapon;

	// Use this for initialization
	void Start () {
        Weapon = GetComponent<WeaponBehavior>();
        Weapon.GetReady();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Weapon.Fire();
        }
    }
}
