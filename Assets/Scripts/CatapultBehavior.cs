using UnityEngine;
using System.Collections;
using System;

public class CatapultBehavior : WeaponBehavior {
    
    public HingeJoint ArmJoint;
    public Transform BasePosition;

    public override ProjectileBehavior Fire() {
        ArmJoint.useSpring = true;
        return base.Fire();
    }

    public override void StateUpdated()
    {
        ArmJoint.spring = new JointSpring
        {
            damper = ArmJoint.spring.damper,
            targetPosition = ArmJoint.spring.targetPosition,
            spring = CatapultPowerConversion()
        };
        base.StateUpdated();
    }

    private float CatapultPowerConversion()
    {
        return (WeaponState.Power * (2425 * Projectile.Mass - 100)) / 100 + 400 + 975 * Projectile.Mass;
    }
}
