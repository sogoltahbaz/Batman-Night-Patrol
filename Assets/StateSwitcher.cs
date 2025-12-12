using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    public GameObject batman;
    public GameObject batmobile;

    private bool isInBatmobile = false;

    void Start()
    {
        batman.SetActive(true);
        batmobile.SetActive(false);
    }

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
