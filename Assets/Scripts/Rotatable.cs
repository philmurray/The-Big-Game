using UnityEngine;
using System.Collections;

public class Rotatable : MonoBehaviour {

    public Vector3 Axis;
    public float RotateStep;
    public float RotateSpeed;

    private float RotateProgress;

    public void Update()
    {
        if (RotateProgress != 0.0f)
        {
            bool gtz = RotateProgress > 0;
            float rotation = gtz ? RotateSpeed : -RotateSpeed;
            transform.RotateAround(Axis, Vector3.up, rotation);

            RotateProgress -= rotation;
            if (gtz != (RotateProgress > 0))
            {
                RotateProgress = 0.0f;
            }
        }
    }

    public void RotateRight() {
        if (RotateProgress == 0.0f) {
            RotateProgress = -RotateStep;
        }
    }

    public void RotateLeft()
    {
        if (RotateProgress == 0.0f)
        {
            RotateProgress = RotateStep;
        }
    }
}
