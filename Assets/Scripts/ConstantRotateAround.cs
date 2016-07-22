using UnityEngine;
using System.Collections;

public class ConstantRotateAround : MonoBehaviour {
    public float speed;
    public Vector3 point;
    public Vector3 axis;
    public float Randomness;

    void Start() {
        speed += Random.Range(-Randomness, Randomness);
    }

	// Update is called once per frame
	void Update () {
        transform.RotateAround(point, axis, Time.deltaTime * speed);
	}
}
