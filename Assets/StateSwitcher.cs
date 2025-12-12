using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    public GameObject batman;
    public GameObject batmobile;

    private bool isInBatmobile = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isInBatmobile = !isInBatmobile;
            SwitchState();
        }
    }

    void SwitchState()
    {
        if (isInBatmobile)
        {
            batman.SetActive(false);
            batmobile.SetActive(true);
        }
        else
        {
            batman.SetActive(true);
            batmobile.SetActive(false);
        }
    }
}
