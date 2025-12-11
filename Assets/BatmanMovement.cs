using UnityEngine;
using System.Collections;

public class BatmanMovement : MonoBehaviour
{
    public enum BatmanState
    {
        Normal,
        Stealth,
        Alert
    }

    public BatmanState currentState = BatmanState.Normal;

    public float speed = 5f;
    public float boostSpeed = 10f;
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

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = boostSpeed;
        }

        Vector3 movement = new Vector3(moveRight, 0, moveForward) * currentSpeed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

        if (currentState == BatmanState.Alert)
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = BatmanState.Alert;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            currentState = BatmanState.Normal;
        }

        switch (currentState)
        {
            case BatmanState.Normal:
                speed = 5f;
                break;
            case BatmanState.Stealth:
                speed = 3f;
                break;
            case BatmanState.Alert:
                speed = 7f;
                break;
        }
    }

    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmanState.Alert)
        {
            if (alertLight != null)
            {
                alertLight.enabled = !alertLight.enabled;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
