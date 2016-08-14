using UnityEngine;
using System.Collections;
using System;

public class CatapultBehavior : WeaponBehavior {

    public float TimeToReset;
    public HingeJoint ArmJoint;

    private bool _fired;
    private float _fireTime;
    
    void Update() {
        if (_fired && Time.fixedDeltaTime > _fireTime + TimeToReset)
        {
            ArmJoint.useSpring = false;
        }
    }
    public override void Fire() {
        _fired = true;
        _fireTime = Time.fixedDeltaTime;
        ArmJoint.useSpring = true;
    }
}
