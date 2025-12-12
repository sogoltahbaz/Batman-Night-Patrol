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

    public float normalSpeed = 5f;
    public float boostSpeed = 20f;
    private float currentSpeed;
    private Rigidbody rb;

    public Light environmentLight; 
    public AudioSource alarmSound;
    public float flashSpeed = 1f;
    private bool isFlashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (alarmSound != null)
        {
            alarmSound.Stop();
        }

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (GetComponent<Light>())
        {
            Destroy(GetComponent<Light>());
        }
    }

    void Update()
    {
        HandleState();

        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");

        if (currentState == BatmanState.Alert)
        {
            currentSpeed = boostSpeed;  
        }
        else
        {
            currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? boostSpeed : normalSpeed;
        }

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

            if (environmentLight != null)
            {
                environmentLight.intensity = 2f; 
            }
        }
        else
        {
            if (alarmSound.isPlaying)
            {
                alarmSound.Stop();
            }

            isFlashing = false;

            if (environmentLight != null)
            {
                environmentLight.intensity = 1f; 
            }
        }

        if (currentState == BatmanState.Stealth)
        {
            if (environmentLight != null)
            {
                environmentLight.intensity = 0.2f;  
            }

            currentSpeed = normalSpeed / 2;  
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
    }

    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmanState.Alert)
        {
            if (environmentLight != null)
            {
                environmentLight.enabled = !environmentLight.enabled;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
