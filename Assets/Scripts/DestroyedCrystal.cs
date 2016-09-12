using UnityEngine;
using System.Collections;

public class DestroyedCrystal : MonoBehaviour {

    public float SpinTime;
    public float SpinAcceleration;
    public GameObject Explosion;
    public float ExplosionRadius;
    public float ExplosionForce;
    public float ExplosionUpwardForce;

    private PositionTracker _positionTracker;
    private ConstantRotator _rotator;
    private float _spinStart;

	// Use this for initialization
	void Start () {
        _positionTracker = GetComponent<PositionTracker>();
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
            Instantiate(Explosion, transform.position, transform.rotation, transform.parent);

            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = hit.GetComponentInParent<Rigidbody>();
                }

                if (rb != null)
                {
                    rb.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius, ExplosionUpwardForce);
                }
            }

            Destroy(gameObject);
            foreach (Transform t in transform)
            {
                Destroy(t.gameObject);
            }
        }
	}
}
