using UnityEngine;

/// <summary>
/// این اسکریپت مسئول دنبال کردن دوربین در بازی است.
/// دوربین به صورت نرم از پشت هدف (بتمن یا بت‌موبیل) دنبال می‌کند و جهت آن همیشه به سمت هدف است.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// اسکریپت StateSwitcher که مشخص می‌کند دوربین در حال حاضر باید کدام آبجکت (بتمن/بت‌موبیل) را دنبال کند.
    /// </summary>
    public StateSwitcher stateSwitcher;

    /// <summary>
    /// سرعت نرمی حرکت و چرخش دوربین. (Lerp/Slerp Factor)
    /// </summary>
    public float smoothSpeed = 50f;

    /// <summary>
    /// فاصله دوربین از پشت هدف.
    /// </summary>
    public float distance = 8f;

    /// <summary>
    /// ارتفاع دوربین نسبت به هدف.
    /// </summary>
    public float height = 3f;

    private Transform target;
    private Rigidbody targetRb;

    /// <summary>
    /// در هر فریم، بررسی می‌کند که آیا هدف دوربین باید تغییر کند یا خیر.
    /// </summary>
    void Update()
    {
        CheckAndSwitchTarget();
    }

    /// <summary>
    /// اعمال حرکت‌های فیزیکی (دنبال کردن) در FixedUpdate برای جلوگیری از لرزش.
    /// اگر هدف ثابت باشد، دوربین فوراً به موقعیت مورد نظر می‌پرد.
    /// </summary>
    void FixedUpdate()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position - target.forward * distance + Vector3.up * height;
        if (targetRb != null && targetRb.velocity.magnitude < 0.1f)
        {
            transform.position = desiredPosition;

            Quaternion instantRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = instantRotation;
        }
        else 
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;

            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// بر اساس وضعیت فعال بودن بتمن یا بت‌موبیل در StateSwitcher، هدف دوربین را تعیین می‌کند.
    /// اگر هدف تغییر کند، دوربین فوراً روی هدف جدید قرار می‌گیرد.
    /// </summary>
    private void CheckAndSwitchTarget()
    {
        Transform newTarget = null;

        if (stateSwitcher.batmobile != null && stateSwitcher.batmobile.activeSelf)
        {
            newTarget = stateSwitcher.batmobile.transform;
        }
        else if (stateSwitcher.batman != null && stateSwitcher.batman.activeSelf)
        {
            newTarget = stateSwitcher.batman.transform;
        }

        if (newTarget != target && newTarget != null)
        {
            target = newTarget;
            targetRb = target.GetComponent<Rigidbody>();

            Vector3 immediatePosition = target.position - target.forward * distance + Vector3.up * height;
            transform.position = immediatePosition;

            Quaternion immediateRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = immediateRotation;
        }
    }
}