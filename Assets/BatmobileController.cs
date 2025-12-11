using UnityEngine;
using System.Collections;

public class BatmobileController : MonoBehaviour
{
    public float speed = 15f; // سرعت حرکت
    public float turnSpeed = 60f; // سرعت چرخش

    public Light alertLight;  // نور هشدار
    public AudioSource alarmSound;  // صدای آلارم
    public float flashSpeed = 1f;  // سرعت چشمک‌زن نور
    private bool isFlashing = false;  // وضعیت نور چشمک‌زن

    private enum BatmobileState
    {
        Normal,
        Alert
    }

    private BatmobileState currentState = BatmobileState.Normal;

    void Update()
    {
        HandleState();

        // حرکت به جلو و عقب
        float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        // چرخش به چپ و راست
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

        // حرکت بت‌موبیل
        transform.Translate(Vector3.forward * move);
        transform.Rotate(Vector3.up * turn);

        // مدیریت نور هشدار و صدای آلارم در حالت Alert
        if (currentState == BatmobileState.Alert)
        {
            if (!isFlashing)
            {
                StartCoroutine(FlashLight());  // شروع چشمک‌زن نور
            }

            if (!alarmSound.isPlaying)
            {
                alarmSound.Play();  // پخش صدای آلارم
            }
        }
        else
        {
            // خاموش کردن نور و صدای آلارم زمانی که از حالت Alert خارج می‌شویم
            if (alertLight != null)
            {
                alertLight.enabled = false;  // خاموش کردن نور
            }

            if (alarmSound.isPlaying)
            {
                alarmSound.Stop();  // متوقف کردن صدای آلارم
            }

            isFlashing = false;
        }
    }

    void HandleState()
    {
        // تغییر حالت‌ها با استفاده از کلیدها
        if (Input.GetKeyDown(KeyCode.Space))  // حالت Alert
        {
            currentState = BatmobileState.Alert;
        }
        else if (Input.GetKeyDown(KeyCode.N))  // برگشت به حالت Normal
        {
            currentState = BatmobileState.Normal;
        }
    }

    // متد برای چشمک‌زدن نور در حالت Alert
    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmobileState.Alert)
        {
            if (alertLight != null)
            {
                alertLight.enabled = !alertLight.enabled;  // روشن و خاموش کردن نور
            }

            yield return new WaitForSeconds(flashSpeed);  // مدت زمان چشمک‌زدن
        }
    }
}
