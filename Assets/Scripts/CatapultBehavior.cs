using UnityEngine;
using System.Collections;
using System;

public class CatapultBehavior : WeaponBehavior {
    
    public HingeJoint ArmJoint;
    public Transform BasePosition;

    public override GameObject Fire() {
        ArmJoint.useSpring = true;
        return base.Fire();
    }
}
