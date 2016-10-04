using UnityEngine;
using System.Collections;

public class DestroyedCrystal : MonoBehaviour {

    public float SpinTime;
    public float SpinAcceleration;
    
    private ConstantRotator _rotator;
    private float _spinStart;

	// Use this for initialization
	void Start () {
        _rotator = GetComponentInChildren<ConstantRotator>();
        _spinStart = Time.fixedTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if ((Time.fixedTime - _spinStart) < SpinTime)
        {
            _rotator.speed = new Vector3(_rotator.speed.x, _rotator.speed.y + SpinAcceleration, _rotator.speed.z);
        }
        else
        {
            Destroy(gameObject);
        }
	}
}
