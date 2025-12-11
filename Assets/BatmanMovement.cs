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

    public Light alertLight;  // نور هشدار
    public AudioSource alarmSound;  // صدای آلارم
    public float flashSpeed = 1f;  // سرعت چشمک‌زن نور
    private bool isFlashing = false;  // وضعیت نور چشمک‌زن

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // خاموش کردن نور و آلارم در ابتدا
        if (alertLight != null)
        {
            alertLight.enabled = false;
        }

        if (alarmSound != null)
        {
            alarmSound.Stop();
        }

        // محدود کردن حرکت در محور Y
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        HandleState();

        // گرفتن ورودی حرکت از کیبورد
        float moveForward = Input.GetAxis("Vertical");
        float moveRight = Input.GetAxis("Horizontal");

        float currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))  // حالت دویدن با Shift
        {
            currentSpeed = boostSpeed;
        }

        // حرکت فقط در محور X و Z
        Vector3 movement = new Vector3(moveRight, 0, moveForward) * currentSpeed * Time.deltaTime;

        // حرکت با استفاده از MovePosition به جای velocity
        rb.MovePosition(transform.position + movement);  // حرکت در محور X و Z فقط، بدون تغییر در محور Y

        // مدیریت نور هشدار و صدای آلارم در حالت Alert
        if (currentState == BatmanState.Alert)
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
        if (Input.GetKeyDown(KeyCode.C))
        {
            currentState = BatmanState.Stealth;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = BatmanState.Alert;
        }
        else if (Input.GetKeyDown(KeyCode.N))  // برای بازگشت به حالت عادی
        {
            currentState = BatmanState.Normal;
        }

        // تغییر سرعت بر اساس حالت
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

    // متد برای چشمک‌زدن نور در حالت Alert
    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmanState.Alert)
        {
            if (alertLight != null)
            {
                alertLight.enabled = !alertLight.enabled;  // روشن و خاموش کردن نور
            }

            yield return new WaitForSeconds(flashSpeed);  // مدت زمان چشمک‌زدن
        }
    }
}
