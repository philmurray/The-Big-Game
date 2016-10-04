using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : MonoBehaviour {

    public float Delay;

    public float Force;
    public float Radius;
    public float UpwardForce;

    public float LifeTime;
    public List<Rigidbody> ExclusionList;

    private float _explodeWaitStart;
    private bool _exploded;
    private float _explodeStart;

    // Use this for initialization
    void Start () {
        _explodeWaitStart = Time.fixedTime;
	}

    public void Explode()
    {
        _exploded = true;
        _explodeStart = Time.fixedTime;

        if (Force > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, Radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = hit.GetComponentInParent<Rigidbody>();
                }

                if (rb != null && !ExclusionList.Contains(rb))
                {
                    rb.AddExplosionForce(Force, transform.position, Radius, UpwardForce);
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
        if (!_exploded && Time.fixedTime - _explodeWaitStart > Delay)
        {
            Explode();
        }
        else if (_exploded && Time.fixedTime - _explodeStart > LifeTime)
        {
            Destroy(gameObject);
        }
	}
}
