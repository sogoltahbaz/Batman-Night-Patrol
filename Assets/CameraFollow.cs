using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform batman;
    public Transform batmobile;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    private Transform target;

    void Start()
    {
        target = batman;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (target == batman)
                target = batmobile;
            else
                target = batman;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Vector3 direction = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * smoothSpeed);

        transform.position = target.position - transform.forward * offset.magnitude;
    }
}
