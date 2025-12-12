using UnityEngine;
using System.Collections;

public class BatmobileController : MonoBehaviour
{
    public float normalSpeed = 15f;
    public float boostSpeed = 50f;
    public float turnSpeed = 60f;

    private float currentSpeed;
    private Rigidbody rb;

    public Light environmentLight;
    public AudioSource alarmSound;
    public float flashSpeed = 1f;

    private bool isFlashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); 

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;  
        }
        else
        {
            rb.useGravity = false;  
        }
    }

    void Update()
    {
        currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? boostSpeed : normalSpeed;

        float move = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        Vector3 moveDirection = transform.forward * move; 
        rb.MovePosition(rb.position + moveDirection); 
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0)); 

        if (Input.GetKey(KeyCode.LeftShift))
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

            if (environmentLight != null)
            {
                environmentLight.intensity = 1f;
            }

            isFlashing = false;
        }
    }

    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (Input.GetKey(KeyCode.LeftShift))
        {
            if (environmentLight != null)
            {
                environmentLight.enabled = !environmentLight.enabled;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
