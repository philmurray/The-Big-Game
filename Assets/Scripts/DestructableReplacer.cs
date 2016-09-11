using UnityEngine;
using System.Collections;

public class DestructableReplacer : Destructable {

    public GameObject Replacement;
    
    public override void OnDestroyed(Collision collision)
    {
        base.OnDestroyed(collision);

        var go = Instantiate(Replacement, transform.position, transform.rotation, transform.parent) as GameObject;

        var myRigidbody = GetComponent<Rigidbody>();
        var newRigidbody = go.GetComponent<Rigidbody>();
        if (myRigidbody != null && newRigidbody != null)
        {
            newRigidbody.velocity = myRigidbody.velocity;
            newRigidbody.angularVelocity = myRigidbody.angularVelocity;
        }

    }

}
