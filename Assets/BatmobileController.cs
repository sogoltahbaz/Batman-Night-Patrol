using UnityEngine;
using System.Collections;

public class BatmobileController : MonoBehaviour
{
    public float normalSpeed = 15f;
    public float boostSpeed = 50f;
    public float turnSpeed = 60f;

    private float currentSpeed;

    public Light alertLight;
    public AudioSource alarmSound;
    public float flashSpeed = 1f;
    private bool isFlashing = false;

    void Update()
    {
        currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? boostSpeed : normalSpeed;

        float move = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);

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

    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (Input.GetKey(KeyCode.LeftShift))
        {
            if (alertLight != null)
            {
                alertLight.enabled = !alertLight.enabled;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
    }
}
