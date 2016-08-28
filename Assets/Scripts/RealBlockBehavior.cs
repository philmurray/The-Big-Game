using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;

public class RealBlockBehavior : BlockBehavior {
    private float Health;
    private Rigidbody Rigidbody;
    private bool Destructable;
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

        Health = Block.Health(Player);
        if (Mathf.Approximately(Health, 0.0f))
        {
            Destructable = false;
        }
    }
}
