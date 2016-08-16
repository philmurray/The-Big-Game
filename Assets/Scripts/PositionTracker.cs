using UnityEngine;
using System.Collections;

public class PositionTracker : MonoBehaviour {

    public bool HasChanged;

    private Vector3 lastPosition;

    void Start() {
        lastPosition = transform.position;
    }

	// Update is called once per frame
	void FixedUpdate () {
        HasChanged = lastPosition != transform.position;
        lastPosition = transform.position;
	}
}
