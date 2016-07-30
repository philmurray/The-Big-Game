using UnityEngine;
using System.Collections;
using Assets.Scripts.DataStructures;
using System;
using System.Collections.Generic;

public class BlockBehavior : MonoBehaviour {
    public Block Block;
    public Material BadMaterial;
    public Material GoodMaterial;
    public bool Draggable;

    private bool isDragging;
    private bool isGood;
    private float height;

    private List<BlockBehavior> collisions = new List<BlockBehavior>();

    public void OnMouseDown()
    {
        if (Draggable)
        {
            isDragging = true;
        }
    }

    public void OnMouseUp() {
        isDragging = false;
        if (!isGood) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider) {
        var b = collider.gameObject.GetComponent<BlockBehavior>();
        if (b != null && !collisions.Contains(b)) {
            collisions.Add(b);
        }
    }

    void OnTriggerLeave(Collider collider)
    {
        var b = collider.gameObject.GetComponent<BlockBehavior>();
        if (b != null && collisions.Contains(b))
        {
            collisions.Remove(b);
        }
    }

    void Update()
    {
        if (isDragging)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0, 1, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float distance;
            if (plane.Raycast(ray, out distance))
            {
                transform.position = ray.GetPoint(distance);
                transform.position = new Vector3(Mathf.Round(transform.position.x), 1, Mathf.Round(transform.position.z));
            }
        }
    }
}
