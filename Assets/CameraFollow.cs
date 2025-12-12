using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform batman;
    public Transform batmobile;
    public float smoothSpeed = 0.125f;  
    public float distance = 8f;  
    public float height = 3f;   

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

        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed);
    }
}
