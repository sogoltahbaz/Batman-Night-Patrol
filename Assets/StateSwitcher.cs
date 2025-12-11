using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    public GameObject batman; // ارجاع به شیء بتمن
    public GameObject batmobile; // ارجاع به شیء بت‌موبیل

    private bool isInBatmobile = false;

    void Update()
    {
        // اگر کلید B فشرده بشه، وضعیت تغییر کنه
        if (Input.GetKeyDown(KeyCode.B))
        {
            isInBatmobile = !isInBatmobile;
            SwitchState();
        }
    }

    void SwitchState()
    {
        if (isInBatmobile)
        {
            // بت‌موبیل فعال بشه و بتمن غیرفعال بشه
            batman.SetActive(false);
            batmobile.SetActive(true);
        }
        else
        {
            // بتمن فعال بشه و بت‌موبیل غیرفعال بشه
            batman.SetActive(true);
            batmobile.SetActive(false);
        }
    }
}
