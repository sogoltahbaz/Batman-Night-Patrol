using UnityEngine;

public class BatSignal : MonoBehaviour
{
    public Light batSignalLight;  
    public float rotationSpeed = 10f; 
    private bool isBatSignalOn = false;  

    /// <summary>
    /// این متد در ابتدای بازی اجرا می‌شود و مسئول تنظیم وضعیت اولیه نور است.
    /// - نور بت‌سیگنال غیرفعال می‌شود.
    /// </summary>
    void Start()
    {
        if (batSignalLight != null)
        {
            batSignalLight.enabled = false; 
        }
    }

    /// <summary>
    /// این متد در هر فریم اجرا می‌شود و مسئول بررسی فشردن کلید "B" برای روشن و خاموش کردن نور است.
    /// - با فشردن کلید "B" وضعیت نور بت‌سیگنال تغییر می‌کند.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBatSignal();
        }

        if (isBatSignalOn)
        {
            batSignalLight.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime); 
        }
    }

    /// <summary>
    /// این متد برای روشن و خاموش کردن نور بت‌سیگنال استفاده می‌شود.
    /// - وضعیت نور تغییر می‌کند (روشن یا خاموش).
    /// </summary>
    void ToggleBatSignal()
    {
        if (batSignalLight != null)
        {
            isBatSignalOn = !isBatSignalOn;  
            batSignalLight.enabled = isBatSignalOn;  
        }
    }
}
