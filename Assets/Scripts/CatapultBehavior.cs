using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.DataStructures;

public class CatapultBehavior : WeaponBehavior {
    
    public HingeJoint ArmJoint;
    public Transform BasePosition;

    public override ProjectileBehavior Fire() {
        Projectile.Mass *= PowerModifier;

        ArmJoint.useSpring = true;
        ArmJoint.spring = new JointSpring
        {
            damper = ArmJoint.spring.damper,
            targetPosition = ArmJoint.spring.targetPosition,
            spring = CatapultPowerConversion()
        };
        Projectile.Activate(1);
        return base.Fire();
    }

    private float CatapultPowerConversion()
    {
        return (WeaponState.Power * (3.5f * Projectile.Mass)) + 400 + 975 * Projectile.Mass;
    }
}
