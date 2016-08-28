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

    public override void StateUpdated()
    {
        ArmJoint.spring = new JointSpring
        {
            damper = ArmJoint.spring.damper,
            targetPosition = ArmJoint.spring.targetPosition,
            spring = ShootingSceneController.instance.CatapultPowerConversion(WeaponState.Power)
        };
        base.StateUpdated();
    }
}
