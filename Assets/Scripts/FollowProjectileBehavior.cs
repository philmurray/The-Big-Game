using UnityEngine;
using System.Collections;

public class FollowProjectileBehavior : MonoBehaviour {

    public GameObject Target;
	
	// Update is called once per frame
	void Update () {
        if (Target != null)
        {
            transform.LookAt(Target.transform);
        }
    }
}
