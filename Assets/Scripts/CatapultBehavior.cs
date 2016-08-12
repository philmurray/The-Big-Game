using UnityEngine;
using System.Collections;

public class CatapultBehavior : MonoBehaviour {

    public HingeJoint ArmJoint;

    public void Fire() {
        ArmJoint.useSpring = true;
    }

}
