using UnityEngine;
using System.Collections;

public class ConstantRotator : MonoBehaviour {
    public Vector3 speed;
    public Vector3 Randomness;

    void Start() {
        speed += (Randomness * (Random.value - 0.5f));
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(speed * Time.deltaTime, Space.World);
	}
}
