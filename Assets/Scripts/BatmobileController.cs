using UnityEngine;
using System.Collections;

/// <summary>
/// کنترلر اصلی بت‌موبیل که حرکت (کنترل تانکی) و مدیریت حالت‌های مختلف (عادی، مخفی‌کاری، هشدار) را بر عهده دارد.
/// </summary>
public class BatmobileController : MonoBehaviour
{
    // --- سیستم حالت‌ها ---
    /// <summary>
    /// حالت‌های مختلف بت‌موبیل: Normal (عادی), Stealth (مخفی‌کاری) و Alert (هشدار).
    /// </summary>
    public enum BatmobileState
    {
        Normal,
        Stealth,
        Alert
    }

    public BatmobileState currentState = BatmobileState.Normal;

    public float normalSpeed = 15f;
    public float stealthSpeed = 5f; 
    public float alertSpeed = 50f; 
    public float turnSpeed = 60f;

    private float currentSpeed;
    private Rigidbody rb;
    private float moveInput;
    private float turnInput;

    public Light environmentLight;
    public AudioSource alarmSound;
    public float flashSpeed = 0.2f; 

    private bool isFlashing = false;

    private Color normalLightColor = Color.white;
    private Color stealthLightColor = Color.gray;
    private Color alertLightColor1 = Color.red;
    private Color alertLightColor2 = Color.blue;

    /// <summary>
    /// تنظیمات اولیه در ابتدای بازی (مانند Rigidbody و قفل کردن محورها).
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        if (environmentLight == null)
        {
            Debug.LogError("Environment light is not assigned!");
        }

        if (alarmSound != null)
        {
            alarmSound.Stop();
        }
    }

    /// <summary>
    /// مدیریت ورودی‌های کاربر برای حرکت و تغییر حالت‌ها در هر فریم.
    /// </summary>
    void Update()
    {
        HandleStateInput(); 

        moveInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        ManageLightingAndSound();
    }

    /// <summary>
    /// اعمال منطق فیزیک، محاسبه سرعت بر اساس حالت و اعمال حرکت/چرخش (کنترل تانکی).
    /// </summary>
    void FixedUpdate()
    {
        switch (currentState)
        {
            case BatmobileState.Stealth:
                currentSpeed = stealthSpeed;
                break;
            case BatmobileState.Alert:
                currentSpeed = alertSpeed;
                break;
            case BatmobileState.Normal:
            default:
                currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? alertSpeed : normalSpeed;
                break;
        }

        float rotationAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, rotationAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);

        Vector3 movement = transform.forward * moveInput * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// تغییر حالت با کلیدهای N (عادی), C (مخفی‌کاری), Space (هشدار).
    /// </summary>
    void HandleStateInput()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            currentState = BatmobileState.Normal;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            currentState = BatmobileState.Stealth;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            currentState = BatmobileState.Alert;
        }
    }

    /// <summary>
    /// تغییر نور، صدا و شروع/توقف فلاش نور بر اساس حالت فعلی.
    /// </summary>
    void ManageLightingAndSound()
    {
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

            if (environmentLight != null)
            {
                environmentLight.intensity = 3f;
            }
        }
        else
        {
            if (alarmSound.isPlaying)
            {
                alarmSound.Stop();
            }

            if (isFlashing)
            {
                StopCoroutine("FlashLight");
                isFlashing = false;
            }

            if (environmentLight != null)
            {
                environmentLight.enabled = true;
                environmentLight.intensity = 1f;
                environmentLight.color = normalLightColor;
            }
        }

        if (currentState == BatmobileState.Stealth)
        {
            if (environmentLight != null)
            {
                environmentLight.intensity = 0.2f;
                environmentLight.color = stealthLightColor;
            }
        }
    }

    /// <summary>
    /// کوروتین مسئول چشمک‌زن کردن نور بین قرمز و آبی در حالت Alert.
    /// </summary>
    IEnumerator FlashLight()
    {
        isFlashing = true;
        bool toggle = false;

        while (currentState == BatmobileState.Alert)
        {
            if (environmentLight != null)
            {
                environmentLight.color = toggle ? alertLightColor1 : alertLightColor2;
                toggle = !toggle;
            }

            yield return new WaitForSeconds(flashSpeed);
        }

        isFlashing = false; 
    }
}