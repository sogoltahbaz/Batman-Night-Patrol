using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    public GameObject batman;
    public GameObject batmobile;

    private bool isInBatmobile = false;

    /// <summary>
    /// این متد در ابتدای بازی اجرا می‌شود و مسئول فعال‌سازی بتمن و غیرفعال‌سازی بت‌موبیل است.
    /// - بتمن فعال است و بت‌موبیل غیرفعال است.
    /// </summary>
    void Start()
    {
        batman.SetActive(true);
        batmobile.SetActive(false);
    }

    /// <summary>
    /// این متد هر فریم اجرا می‌شود و مسئول بررسی فشردن کلید "Q" برای تغییر وضعیت بین بتمن و بت‌موبیل است.
    /// - با فشردن "Q" وضعیت بین بتمن و بت‌موبیل تغییر می‌کند.
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
    /// - وقتی بتمن فعال است، بت‌موبیل غیر فعال می‌شود و بالعکس.
    /// - موقعیت و چرخش مدل‌ها به مدل جدید منتقل می‌شود.
    /// </summary>
    void SwitchState()
    {
        if (isInBatmobile)
        {
            batmobile.transform.position = batman.transform.position;
            batmobile.transform.rotation = batman.transform.rotation;

            batman.SetActive(false);
            batmobile.SetActive(true);
        }
        else
        {
            batman.transform.position = batmobile.transform.position;
            batman.transform.rotation = batmobile.transform.rotation;

            batman.SetActive(true);
            batmobile.SetActive(false);
        }
    }
}
