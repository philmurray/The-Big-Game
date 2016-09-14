using UnityEngine;
using System.Collections;

public class PositionTracker : MonoBehaviour {

    public bool HasChanged;
    public bool IgnorePosition;

	// Update is called once per frame
	void FixedUpdate () {
        if (!IgnorePosition)
        {
            HasChanged = transform.hasChanged;
            transform.hasChanged = false;
        }
	}
}
