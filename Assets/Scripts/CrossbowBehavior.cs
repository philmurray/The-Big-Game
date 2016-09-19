using UnityEngine;
using System.Collections;

public class CrossbowBehavior : WeaponBehavior
{
    public Animator ArmAnimator;

    public override ProjectileBehavior Fire()
    {
        ArmAnimator.StartPlayback();

        //Projectile.Mass *= PowerModifier;
        //Projectile.Activate(1);
        return base.Fire();
    }

}
