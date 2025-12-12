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

    private Color normalLightColor = Color.white;
    private Color stealthLightColor = Color.gray;
    private Color alertLightColor1 = Color.red;
    private Color alertLightColor2 = Color.blue;

    private bool isAlertFlashing = false;

    /// <summary>
    /// این متد برای تنظیمات اولیه در ابتدای بازی استفاده می‌شود.
    /// - اتصال به Rigidbody برای فیزیک و حرکت.
    /// - تنظیمات اولیه نور و صدای آلارم.
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
    /// در این متد، حرکت و چرخش بتمن مدیریت می‌شود.
    /// همچنین، نور و صدای آلارم طبق حالت‌های مختلف تغییر می‌کند.
    /// </summary>
    void Update()
    {
        HandleState();  // مدیریت تغییر وضعیت بتمن (Normal, Stealth, Alert)

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
            Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 10f));
        }

        ManageLightingAndSound();  
    }

    /// <summary>
    /// در این متد وضعیت‌های مختلف بتمن تغییر می‌کند.
    /// - عادی (Normal): حالت پیش‌فرض
    /// - مخفی‌کاری (Stealth): سرعت کاهش پیدا می‌کند و نور کم می‌شود
    /// - هشدار (Alert): سرعت افزایش می‌یابد و نور تغییر می‌کند
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
    /// - در حالت **Alert**، نور چشمک‌زن می‌شود و آلارم پخش می‌شود.
    /// - در حالت **Normal**، نور به رنگ سفید و شدت معمولی باز می‌گردد.
    /// - در حالت **Stealth**، نور کم می‌شود و رنگ آن خاکی می‌شود.
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

            currentSpeed = normalSpeed / 2;
        }
    }

    /// <summary>
    /// این متد مسئول چشمک‌زن کردن نور در حالت Alert است.
    /// نور بین رنگ‌های قرمز و آبی تغییر می‌کند.
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
    }
}
