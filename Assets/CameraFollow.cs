using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public StateSwitcher stateSwitcher;
    public float smoothSpeed = 50f;
    public float distance = 8f;
    public float height = 3f;

    private Transform target;
    private Rigidbody targetRb;

    void Update()
    {
        CheckAndSwitchTarget();
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;

        if (targetRb != null && targetRb.velocity.magnitude < 0.1f)
        {
            transform.position = desiredPosition;
            Quaternion instantRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = instantRotation;
        }
        else 
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }

    private void CheckAndSwitchTarget()
    {
        Transform newTarget = null;
        if (stateSwitcher.batmobile.activeSelf)
        {
            newTarget = stateSwitcher.batmobile.transform;
        }
        else if (stateSwitcher.batman.activeSelf)
        {
            newTarget = stateSwitcher.batman.transform;
        }

        if (newTarget != target && newTarget != null)
        {
            target = newTarget;
            targetRb = target.GetComponent<Rigidbody>();

            Vector3 immediatePosition = target.position - target.forward * distance + Vector3.up * height;
            transform.position = immediatePosition;

            Quaternion immediateRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = immediateRotation;
        }
    }
}