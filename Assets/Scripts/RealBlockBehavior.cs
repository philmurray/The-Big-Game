using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

public class RealBlockBehavior : BlockBehavior {
    private Rigidbody Rigidbody;
    public override void Start ()
    {
        base.Start();

        Rigidbody = GetComponent<Rigidbody>();

        var mass = Block.Mass(Player);
        Rigidbody.mass = mass;
        if (Mathf.Approximately(mass, 0.0f))
        {
            Rigidbody.isKinematic = true;
        }

        var destructable = GetComponent<Destructable>();
        if (destructable != null)
        {
            destructable.Health = Block.Health(Player);
        }
    }
}
