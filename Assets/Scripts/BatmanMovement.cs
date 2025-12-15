using UnityEngine;
using System.Collections;

public class BatmanMovement : MonoBehaviour
{
    // --- سیستم حالت‌ها ---
    /// <summary>
    /// حالت‌های مختلف شخصیت بتمن: Normal (عادی), Stealth (مخفی‌کاری) و Alert (هشدار).
    /// هر حالت پارامترهای سرعت، نور و صدا را کنترل می‌کند.
    /// </summary>
    public enum BatmanState
    {
        Normal,
        Stealth,
        Alert
    }

    public BatmanState currentState = BatmanState.Normal;

    public float normalSpeed = 5f;
    public float boostSpeed = 20f;
    public float rotationSpeed = 200f;

    private float currentSpeed;
    private Rigidbody rb;

    public Light environmentLight;
    public AudioSource alarmSound;
    public float flashSpeed = 1f;
    private bool isFlashing = false;

    private Color normalLightColor = Color.white;
    private Color stealthLightColor = Color.gray;
    private Color alertLightColor1 = Color.red;
    private Color alertLightColor2 = Color.blue;

    private bool isAlertFlashing = false;

    private float moveForward;
    private float moveRotate;

    /// <summary>
    /// این متد برای تنظیمات اولیه در ابتدای بازی استفاده می‌شود.
    /// شامل گرفتن کامپوننت Rigidbody و تنظیم اولیه چرخش مدل.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.rotation = Quaternion.Euler(0, 90f, 0);

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

    /// <summary>
    /// مدیریت ورودی‌ها، تغییر حالت‌ها و مدیریت نور/صدا در هر فریم.
    /// </summary>
    void Update()
    {
        HandleState(); 
        ManageLightingAndSound(); 

        moveForward = Input.GetAxis("Vertical");
        moveRotate = Input.GetAxis("Horizontal");
    }

    /// <summary>
    /// اعمال منطق فیزیک، حرکت و چرخش (کنترل تانکی) در FixedUpdate برای هماهنگی با Rigidbody.
    /// </summary>
    void FixedUpdate()
    {
        if (currentState == BatmanState.Alert)
        {
            currentSpeed = boostSpeed;
        }
        else if (currentState == BatmanState.Stealth)
        {
            currentSpeed = normalSpeed / 2;
        }
        else 
        {
            currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? boostSpeed : normalSpeed;
        }

        float rotationAmount = moveRotate * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, rotationAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        Vector3 movement = transform.forward * moveForward * currentSpeed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// مدیریت تغییر وضعیت بتمن از طریق ورودی‌های کاربر (N, C, Space).
    /// </summary>
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

    /// <summary>
    /// این متد مسئول تغییر نور محیطی (environmentLight) و صدای آلارم (alarmSound) در هر حالت است.
    /// </summary>
    void ManageLightingAndSound()
    {
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

            if (isFlashing)
            {
                StopCoroutine("FlashLight");
            }
            isFlashing = false;

            if (environmentLight != null)
            {
                environmentLight.intensity = 1f;
                environmentLight.color = normalLightColor;
            }
        }

        if (currentState == BatmanState.Stealth)
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

        while (currentState == BatmanState.Alert)
        {
            if (environmentLight != null)
            {
                environmentLight.color = isAlertFlashing ? alertLightColor1 : alertLightColor2;
                isAlertFlashing = !isAlertFlashing;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
        isFlashing = false;
    }
}