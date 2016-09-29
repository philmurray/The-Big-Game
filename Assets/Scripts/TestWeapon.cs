using UnityEngine;
using System.Collections;

public class TestWeapon : MonoBehaviour {

    private WeaponBehavior Weapon;
    private float _angleChange;
    public float VerticalAngleChangeSpeed;
    public float VerticalAngleMax;

	// Use this for initialization
	void Start () {
        Weapon = GetComponent<WeaponBehavior>();
        Weapon.GetReady();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Weapon.WeaponState.VerticalAngle = Mathf.Clamp(Weapon.WeaponState.VerticalAngle + _angleChange, 0, VerticalAngleMax);
        Weapon.StateUpdated();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Weapon.Fire();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _angleChange = VerticalAngleChangeSpeed;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _angleChange = -VerticalAngleChangeSpeed;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            _angleChange = 0;
        }
    }
}
