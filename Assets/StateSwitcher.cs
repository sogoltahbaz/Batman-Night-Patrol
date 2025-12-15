using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    public GameObject batman;
    public GameObject batmobile;

    private bool isInBatmobile = false;

    /// <summary>
    /// این متد در ابتدای بازی اجرا می‌شود و مسئول تنظیم وضعیت اولیه است.
    /// </summary>
    void Start()
    {
        batman.SetActive(true);
        batmobile.SetActive(false);
    }

    /// <summary>
    /// بررسی فشردن کلید "Q" برای تغییر وضعیت.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isInBatmobile = !isInBatmobile;
            SwitchState();
        }
    }

    /// <summary>
    /// این متد برای سوییچ کردن بین حالت‌های بتمن و بت‌موبیل استفاده می‌شود.
    /// </summary>
    void SwitchState()
    {
        if (isInBatmobile) 
        {
            Rigidbody batmanRb = batman.GetComponent<Rigidbody>();
            if (batmanRb != null)
            {
                batmanRb.velocity = Vector3.zero;
                batmanRb.angularVelocity = Vector3.zero;
            }
            batmobile.transform.position = batman.transform.position;
            batmobile.transform.rotation = batman.transform.rotation;

            batman.SetActive(false);
            batmobile.SetActive(true);
        }
        else 
        {
            Rigidbody batmobileRb = batmobile.GetComponent<Rigidbody>();
            if (batmobileRb != null)
            {
                batmobileRb.velocity = Vector3.zero;
                batmobileRb.angularVelocity = Vector3.zero;
            }

            batman.transform.position = batmobile.transform.position;
            batman.transform.rotation = batmobile.transform.rotation;

            batman.SetActive(true);
            batmobile.SetActive(false);
        }
    }
}