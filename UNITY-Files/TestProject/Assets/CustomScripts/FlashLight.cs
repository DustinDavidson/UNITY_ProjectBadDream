using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light flashlight;
    public bool isOn = false;
    public int batteryLife = 100;
    public int drainRate = 5; // battery drain per second
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            DrainBattery();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
       if(batteryLife > 0 && !isOn)
       {
           isOn = true;
           flashlight.enabled = true;
           Debug.Log("Flashlight turned ON");
       }
       else if(isOn)
       {
           isOn = false;
           flashlight.enabled = false;
           Debug.Log("Flashlight turned OFF");
       }
       else
       {
           Debug.Log("Battery dead. Cannot turn on flashlight.");
       }
    }

    public void DrainBattery()
    {
        if(isOn && batteryLife > 0)
        {
            batteryLife -= drainRate;
            batteryLife = Mathf.Clamp(batteryLife, 0, 100);
            Debug.Log("Battery drained. Current battery life: " + batteryLife + "%");
            if(batteryLife == 0)
            {
                isOn = false;
                Debug.Log("Battery dead. Flashlight turned OFF");
            }
        }
    }

}
