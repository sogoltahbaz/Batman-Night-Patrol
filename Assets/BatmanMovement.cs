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
    public float rotationSpeed = 200f; // سرعت چرخش جدید

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

    // متغیر حذف شده که اکنون برای رفع خطا اضافه می‌شود
    private bool isAlertFlashing = false; // <--- این خط اضافه شد/تایید شد

    private float moveForward;
    private float moveRotate;

    /// <summary>
    /// این متد برای تنظیمات اولیه در ابتدای بازی استفاده می‌شود.
    /// </summary>
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

    /// <summary>
    /// گرفتن ورودی‌ها و مدیریت حالت
    /// </summary>
    void Update()
    {
        HandleState(); // مدیریت تغییر وضعیت بتمن (Normal, Stealth, Alert)
        ManageLightingAndSound(); // مدیریت نور و صدا

        // گرفتن ورودی‌های حرکت و چرخش
        moveForward = Input.GetAxis("Vertical"); // جلو/عقب (W/S یا فلش بالا/پایین)
        moveRotate = Input.GetAxis("Horizontal"); // چرخش چپ/راست (A/D یا فلش چپ/راست)
    }

    /// <summary>
    /// اعمال فیزیک و حرکت (در FixedUpdate برای هماهنگی با Rigidbody)
    /// </summary>
    void FixedUpdate()
    {
        // 1. تنظیم سرعت بر اساس حالت
        if (currentState == BatmanState.Alert)
        {
            currentSpeed = boostSpeed;
        }
        else if (currentState == BatmanState.Stealth)
        {
            currentSpeed = normalSpeed / 2;
        }
        else // Normal
        {
            currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? boostSpeed : normalSpeed;
        }

        // 2. اعمال چرخش (استفاده از Horizontal Input)
        float rotationAmount = moveRotate * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, rotationAmount, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 3. اعمال حرکت (استفاده از Vertical Input)
        Vector3 movement = transform.forward * moveForward * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    /// <summary>
    /// در این متد وضعیت‌های مختلف بتمن تغییر می‌کند.
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
    /// این متد مسئول تغییر نور و صدای آلارم در هر حالت است.
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

            // متوقف کردن کوروتین و تنظیم isFlashing
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

            // سرعت در FixedUpdate تنظیم می‌شود، این خط حذف شد تا تکرار نشود.
        }
    }

    /// <summary>
    /// این متد مسئول چشمک‌زن کردن نور در حالت Alert است.
    /// </summary>
    IEnumerator FlashLight()
    {
        isFlashing = true;

        while (currentState == BatmanState.Alert)
        {
            if (environmentLight != null)
            {
                // استفاده از isAlertFlashing که اکنون تعریف شده است.
                environmentLight.color = isAlertFlashing ? alertLightColor1 : alertLightColor2;
                isAlertFlashing = !isAlertFlashing;
            }

            yield return new WaitForSeconds(flashSpeed);
        }
        isFlashing = false; // تنظیم مجدد پس از پایان حلقه
    }
}