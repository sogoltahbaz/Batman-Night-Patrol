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

    private Color normalLightColor = Color.white;  
    private Color boostLightColor1 = Color.red;    
    private Color boostLightColor2 = Color.blue;  

    private bool isBoostFlashing = false; 

    /// <summary>
    /// این متد در ابتدای بازی اجرا می‌شود و مسئول تنظیمات اولیه است.
    /// - اتصال به Rigidbody برای کنترل فیزیک ماشین.
    /// - تنظیمات اولیه نور و صدای آلارم.
    /// </summary>
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

        if (environmentLight == null)
        {
            Debug.LogError("Environment light is not assigned!");
        }
    }

    /// <summary>
    /// این متد هر فریم اجرا می‌شود و مسئول حرکت و چرخش بت‌موبیل است.
    /// - حرکت جلو و عقب با استفاده از Input عمودی.
    /// - چرخش با استفاده از Input افقی.
    /// - در صورت نگه داشتن دکمه Shift، حالت Boost فعال می‌شود.
    /// </summary>
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
                environmentLight.color = isBoostFlashing ? boostLightColor1 : boostLightColor2;  
                isBoostFlashing = !isBoostFlashing; 
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
                environmentLight.color = normalLightColor;  
            }

            isFlashing = false;
        }
    }

    /// <summary>
    /// این متد برای چشمک‌زن کردن نور در حالت Boost استفاده می‌شود.
    /// - نور در حالت Boost باید به صورت چشمک‌زن تغییر کند.
    /// </summary>
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
