using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

public class CrossbowBehavior : WeaponBehavior
{
    public Animator ArmAnimator;
    public float BoltSpeed;
    public float AnimationSpeed;
    public Transform MainComponent;
    public Transform MainRotator;

    private float _angle;

    public override ProjectileBehavior Fire()
    {
        ArmAnimator.SetTrigger("Fire");
        ArmAnimator.SetFloat("Fire Speed", AnimationSpeed);

        Projectile.GetComponent<Rigidbody>().velocity = Projectile.transform.right * BoltSpeed;

        //Projectile.Mass *= PowerModifier;
        //Projectile.Activate(1);
        return base.Fire();
    }

    public override void StateUpdated()
    {
        float rotation = _angle - WeaponState.VerticalAngle;
        MainComponent.RotateAround(MainRotator.position, MainRotator.transform.forward, -rotation);
        _angle = WeaponState.VerticalAngle;
    }

}
