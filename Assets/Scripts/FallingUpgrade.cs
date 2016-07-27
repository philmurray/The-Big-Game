using UnityEngine;
using System.Collections;
using System;
using Assets.Scripts.DataStructures;
using System.Collections.Generic;

public class FallingUpgrade : MonoBehaviour {

    public PlayerState.Upgrade Upgrade;
    public Renderer UpgradeRenderer;

    public bool IsPremium;

    public float VelocityNormal;
    public float VelocityPremium;
    public float StopAtPremium;
    public float StopForPremium;

    private Rigidbody rb;
    private float StopStart;

    public void Start() {
        rb = GetComponent<Rigidbody>();
        SetVelocity();
    }

    public void FixedUpdate() {
        if (IsPremium)
        {
            if (transform.localPosition.y < StopAtPremium && StopStart == 0.0f)
            {
                StopStart = Time.fixedTime;
                rb.velocity = Vector3.zero;
            }
            if (StopStart + StopForPremium < Time.fixedTime)
            {
                SetVelocity();
            }
        }
    }

    public void SetUpgrade(PlayerState.Upgrade upgrade, Material material, bool isPremium) {
        Upgrade = upgrade;
        UpgradeRenderer.material = material;
        IsPremium = isPremium;
    }

    private void SetVelocity()
    {
        rb.velocity = new Vector3(0, IsPremium ? VelocityPremium : VelocityNormal, 0);
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
