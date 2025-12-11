using UnityEngine;
using System.Collections;

public class BatmobileController : MonoBehaviour
{
    public float speed = 15f;
    public float turnSpeed = 60f;

    public Light alertLight;
    public AudioSource alarmSound;
    public float flashSpeed = 1f;
    private bool isFlashing = false;

    private enum BatmobileState
    {
        Normal,
        Alert
    }

    private BatmobileState currentState = BatmobileState.Normal;

    void Update()
    {
        HandleState();

        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);

        if (currentState == BatmobileState.Alert)
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = BatmobileState.Alert;
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            currentState = BatmobileState.Normal;
        }
    }

    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmobileState.Alert)
        {
            if (alertLight != null)
            {
                alertLight.enabled = !alertLight.enabled;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
