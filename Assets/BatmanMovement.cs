using UnityEngine;
using System.Collections;

public class BatmanMovement : MonoBehaviour
{
    public enum BatmanState
    {
        Normal,
        Stealth
    }

    public BatmanState currentState = BatmanState.Normal;

    public float normalSpeed = 5f;
    public float boostSpeed = 20f;
    private float currentSpeed;
    private Rigidbody rb;

    public Light alertLight;
    public AudioSource alarmSound;
    public float flashSpeed = 1f;
    private bool isFlashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (alertLight != null)
        {
            alertLight.enabled = false;
        }

        if (alarmSound != null)
        {
            alarmSound.Stop();
        }

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleState();

        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");

        currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? boostSpeed : normalSpeed;

        Vector3 movement = new Vector3(moveRight, 0, moveForward) * currentSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (movement.magnitude > 0)
        {
            if (moveForward > 0)
            {
                Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
            }
            else if (moveForward < 0)
            {
                Quaternion toRotation = Quaternion.LookRotation(-movement.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f);
            }
        }

        if (currentState == BatmanState.Stealth)
        {
            if (!isFlashing)
            {
                StartCoroutine(FlashLight());
            }

            if (!alarmSound.isPlaying)
            {
                alarmSound.Play();
            }
        }
        else
        {
            if (alertLight != null)
            {
                alertLight.enabled = false;
            }

            if (alarmSound.isPlaying)
            {
                alarmSound.Stop();
            }

            isFlashing = false;
        }
    }

    void HandleState()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentState = BatmanState.Stealth;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            currentState = BatmanState.Normal;
        }
    }

    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmanState.Stealth)
        {
            if (alertLight != null)
            {
                alertLight.enabled = !alertLight.enabled;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
