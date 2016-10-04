using UnityEngine;
using System.Collections;

public class ReplaceOnDestroy : MonoBehaviour {

    public GameObject Replacement;

    void OnDestroy()
    {
        var go = Instantiate(Replacement, transform.position, transform.rotation, transform.parent) as GameObject;
        var rigidBody = GetComponent<Rigidbody>();
        var goRigidBody = go.GetComponent<Rigidbody>();
        if (rigidBody != null && goRigidBody != null)
        {
            goRigidBody.velocity = rigidBody.velocity;
            goRigidBody.angularVelocity = rigidBody.angularVelocity;
        }
    }
}
