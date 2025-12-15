using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// مدیریت عملکرد دکمه‌ها در صفحه اصلی (Main Menu).
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// بارگذاری صحنه اصلی بازی (SampleScene).
    /// به عنوان متد OnClick برای دکمه Start استفاده می‌شود.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// خروج از برنامه (فقط در بیلد نهایی کار می‌کند).
    /// به عنوان متد OnClick برای دکمه Quit استفاده می‌شود.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();

    }
}