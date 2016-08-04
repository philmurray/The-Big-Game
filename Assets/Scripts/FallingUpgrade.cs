using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class FallingUpgrade : MonoBehaviour {

    public PlayerState.Upgrade Upgrade;

    private float Velocity;
    private float StopAt;
    private float StopFor;

    private Rigidbody rb;
    private float StopStart;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, Velocity, 0);
    }

    public void FixedUpdate() {
        if (StopFor > 0)
        {
            if (transform.localPosition.y < StopAt && StopStart == 0.0f)
            {
                StopStart = Time.fixedTime;
                rb.velocity = Vector3.zero;
            }
            if (StopStart + StopFor < Time.fixedTime)
            {
                rb.velocity = new Vector3(0, Velocity, 0);
            }
        }
    }

    public void SetUpgrade(float velocity, float stopAt, float stopFor) {
        Velocity = velocity;
        StopAt = stopAt;
        StopFor = stopFor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground") {
            Destroy(gameObject);
        }

        if (other.tag == "Player") {
            Destroy(gameObject);
            UpgradeSceneController.instance.ApplyUpgrade(Upgrade);
        }
    }
}
