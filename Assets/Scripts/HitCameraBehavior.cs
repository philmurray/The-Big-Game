using UnityEngine;
using System.Collections;

public class HitCameraBehavior : MonoBehaviour {

    public Transform Target;
    public float RotateSpeed;
    public float LookSpeed;
    public float ZoomSpeed;

    private Vector3 point;
    private Camera cam;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private float originalFOV;

    private Transform focusTarget;

    void Start()
    {
        cam = GetComponent<Camera>();
        point = Target.position;
        transform.LookAt(point);

        originalPosition = transform.localPosition;
        originalRotation = transform.localRotation;
        originalFOV = cam.fieldOfView;
    }

    void Update()
    {
        if (cam.enabled)
        {
            if (focusTarget != null)
            {
                Vector3 dir = focusTarget.position - transform.position;
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, LookSpeed * Time.deltaTime);

                cam.fieldOfView -= ZoomSpeed;
            }
            else
            {
                transform.RotateAround(point, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * RotateSpeed);
            }
        }
    }

    public bool CameraEnabled
    {
        get
        {
            return cam != null && cam.enabled;
        }
        set
        {
            if (cam == null) return;
            cam.enabled = value;
        }
    }

    public void FocusOn(Transform t)
    {
        focusTarget = t;
    }

    public void Reset()
    {
        focusTarget = null;
        transform.localPosition = originalPosition;
        transform.localRotation = originalRotation;
        cam.fieldOfView = originalFOV;
    }
}
