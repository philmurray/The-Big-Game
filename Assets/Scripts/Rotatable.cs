using UnityEngine;
using System.Collections;

public class Rotatable : MonoBehaviour {

    public Transform Axis;
    public float RotateStep;
    public float RotateSpeed;

    private float RotateProgress;
    private float CurrentAngle;

    public void Update()
    {
        if (RotateProgress != 0.0f)
        {
            bool gtz = RotateProgress > 0;
            float rotation = gtz ? RotateSpeed : -RotateSpeed;

            CurrentAngle += rotation;
            transform.RotateAround(Axis.position, Vector3.up, rotation);

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

    public void SetAngle(float angle)
    {
        float rotation = CurrentAngle - angle;
        transform.RotateAround(Axis.position, Vector3.up, rotation);
        CurrentAngle = angle;
    }
}
