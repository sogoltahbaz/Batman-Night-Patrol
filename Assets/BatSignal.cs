using UnityEngine;

public class BatSignal : MonoBehaviour
{
    public Light batSignalLight;  
    public float rotationSpeed = 10f; 
    private bool isBatSignalOn = false;  

    void Start()
    {
        if (batSignalLight != null)
        {
            batSignalLight.enabled = false; 
        }
    }

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

    void ToggleBatSignal()
    {
        if (batSignalLight != null)
        {
            isBatSignalOn = !isBatSignalOn;  
            batSignalLight.enabled = isBatSignalOn;  
        }
    }
}
