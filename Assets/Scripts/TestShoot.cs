using UnityEngine;
using System.Collections;

public class TestShoot : MonoBehaviour {
    public float speed;
    public float fireSpeed;

    private Rigidbody rb;
    private Vector3 vel;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        vel = new Vector3(0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (rb.isKinematic)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.isKinematic = false;
                rb.velocity = new Vector3(0, 0, fireSpeed);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                vel = new Vector3(vel.x, speed, vel.z);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                vel = new Vector3(vel.x, -speed, vel.z);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                vel = new Vector3(-speed, vel.y, vel.z);
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                vel = new Vector3(speed, vel.y, vel.z);
            }


            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                vel = new Vector3(vel.x, 0, vel.z);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                vel = new Vector3(vel.x, 0, vel.z);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                vel = new Vector3(0, vel.y, vel.z);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                vel = new Vector3(0, vel.y, vel.z);
            }

            transform.position = transform.position + vel;
        }
    }
}
