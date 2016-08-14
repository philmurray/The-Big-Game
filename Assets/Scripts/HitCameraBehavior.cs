using UnityEngine;
using System.Collections;

public class HitCameraBehavior : MonoBehaviour {

    public Transform Target;
    public float Speed = 10.0f;

    private Vector3 point;
    private Camera cam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        cam = GetComponent<Camera>();
        point = Target.position;
        transform.LookAt(point);

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        if (cam.enabled)
        {
            transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * Speed);
        }
    }

    public void Reset()
    {
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
    }
}
